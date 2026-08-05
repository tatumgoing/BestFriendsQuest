using MyBox;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class AreaCharacterData
{
    public AreaName Area;
    public int Capacity;
}

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager i;

    [Header("Characters")]
    [SerializeField] private GameObject _characterControllerPrefab;
    [SerializeField, ReadOnly] private List<CompleteCharacterData> _allCharacters = new List<CompleteCharacterData>();
    [SerializeField, ReadOnly] private List<RelationshipData> _relationships = new List<RelationshipData>();
    [SerializeField] private List<ColorData> _clothingColors;
    [SerializeField] private List<PersonalityData> _personalities = new List<PersonalityData>();
    [SerializeField] private float _defaultHeight = 3.4f;

    [Header("Areas")]
    [SerializeField] private List<AreaName> _validAreas = new List<AreaName>();
    [SerializeField] private List<AreaCharacterData> _areaData = new List<AreaCharacterData>();
    [SerializeField] private float _minTimeAtLocation = 5; //minimum time that characters will spend in each area
    [SerializeField] private float _moveCheckCooldown = 5; //how often each character will try to move

    [Header("Problems")]
    [SerializeField, Range(0, 1), Tooltip("Max percent of citizens that can have problems")] private float _maxProblemPercent = 0.3f;
    [SerializeField] private List<ProblemData> _allProblems;

    public UnityEvent OnCharacterMove = new UnityEvent();
    public List<CompleteCharacterData> AllCharacters => _allCharacters;
    public CompleteCharacterData GetRandomCharacter() => AllCharacters[Random.Range(0, AllCharacters.Count)];
    public string GetAge(ID id) => _allCharacters.Find(c => c.ID == id).Age.ToString();
    public string GetFavoriteColorString(ID id) => Utils.CapitalFirst(GetFavoriteColor(id).ToString().ToLower());
    public PersonalityData GetPersonality(ID ID) => _personalities[ID %  _personalities.Count];
    private int GetCapacity(AreaName area) => _areaData.Where(x => x.Area == area).First().Capacity;
    public float MinLocationTime  => _minTimeAtLocation;

    void Awake()
    {
        i = this;
    }

    private void Start()
    {
        LoadCharactersFromFile();
        LoadProblems();

        foreach (var characterA in _allCharacters) {
            foreach (var characterB in _allCharacters) {
                if (characterA != characterB) {
                    var loadedValue = SaveSystem.LoadRelationship(characterA.ID, characterB.ID);
                    _relationships.Add(new RelationshipData(characterA.ID, characterB.ID, loadedValue));
                }
            }
        }
        RandomizeRelationships();

        //GenerateProblem();
    }

    private void Update()
    {
        var problemRatio = (float)numCharactersWithProblems() / _allCharacters.Count;
        if (problemRatio < _maxProblemPercent) GenerateProblem();
        TickCharacterLocations();
    }

    public void LoadCurrentQuests()
    {
        var questingChars = SaveSystem.GetAllQuestingCharacters();
        foreach (var c in _allCharacters) if (questingChars.Contains(c.ID)) c.SetArea(AreaName.PORT);
    }

    public void ChangeCharacterLocation(ID id, AreaName area)
    {
        var character = _allCharacters.Find(c => c.ID == id);
        character?.SetArea(area);
    }

    private void TickCharacterLocations()
    {
        foreach (var character in _allCharacters) {
            if (character.TimeAtLocation < _minTimeAtLocation || Time.time - character.TimeWhenMoveCheck < character.MoveCheckCooldown) continue;
            if (Random.Range(0, 1f) < 0.5f) {
                character.SetArea(GetAvailableLocation());
                OnCharacterMove.Invoke();
            }
            else {
                character.TimeWhenMoveCheck = Time.time;
            }
        }
    }

    public string GetLocation(ID id)
    {
        switch (GetCharacter(id).CurrentArea) {
            case AreaName.PARK: return "at the park";
            case AreaName.TOWN: return "in town";
            case AreaName.SHOP: return "buying clothes";
            case AreaName.RESTURAUNT: return "eating out";
            case AreaName.TOWN_HALL: return "at town hall";
            case AreaName.PORT: return "out questing";
            case AreaName.HARDWARE_STORE: return "at the hardware store";
            case AreaName.GROCERY_STORE: return "getting groceries";
        }

        return "";
    }

    public List<ID> GetIDsByArea(AreaName area)
    {
        var charactersInArea = _allCharacters.Where(c => c.CurrentArea == area).ToList();
        return charactersInArea.Select(c => c.ID).ToList();
    }

    [ButtonMethod]
    public void PrintCharacterInventories()
    {
        var saveString = string.Join("|", _allCharacters.Select(c => c.GetInventoryString()));
        print(saveString);
    }

    public void GiveItem(ID id, ItemData item)
    {
        var characterData = _allCharacters.Find(c => c.ID == id);
        characterData.AddToInventory(item);
    }

    public Color GetClothingColor(ID id) => GetClothingColor(GetFavoriteColor(id));
    public Color GetClothingColor(FavoriteColor color)
    {
        var data = _clothingColors.Find(c => c.Color == color);
        return data.UseColor;
    }

    public FavoriteColor GetFavoriteColor(ID id)
    {
        var characterData = _allCharacters.Find(c => c.ID == id);
        return characterData.FavColor;
    }

    public List<ItemData> GetInventory(ID id)
    {
        var characterData = _allCharacters.Find(c => c.ID == id);
        return characterData.Inventory;
    }

    /// <summary>
    /// Call to mark a problem as solved - problem doesn't get completely resolved until the rewards are given,
    /// so calling this means that the rewards can be dispensed when the character with the problem
    /// is spoken to again. 
    /// Set up this way so that minigame problems can be solved when the minigame is completed but the rewards can be given
    /// and the reward dialogue can be said in the character's room.
    /// </summary>
    public void SolveProblem(ID id)
    {
        for (int i = 0; i < _allCharacters.Count; i++) {
            if (_allCharacters[i].ID == id) _allCharacters[i].SolveProblem();
        }
    }

    public void GiveProblemRewards(ID id)
    {
        for (int i = 0; i < _allCharacters.Count; i++) {
            if (_allCharacters[i].ID == id) _allCharacters[i].GiveProblemRewards();
        }
    }

    public ProblemData GetProblem(ID id)
    {
        var characterData = _allCharacters.Find(c => c.ID == id);
        return characterData.CurrentProblem;
    }

    public string GetDialogue(ID id)
    {
        var characterData = _allCharacters.Find(c => c.ID == id);
        return characterData.GetDialogue();
    }

    private int numCharactersWithProblems()
    {
        int numProblems = 0;
        foreach (var character in _allCharacters) if (character.HasProblem) numProblems++;
        return numProblems;
    }

    public void GenerateProblem()
    {
        var validOptions = AllCharacters.Where(x => !x.HasProblem).ToList();
        GenerateProblem(validOptions[Random.Range(0, validOptions.Count)].ID);
    }

    private void GenerateProblem(ID selectedCharacterID)
    {
        var selectedProblem = _allProblems[Random.Range(0, _allProblems.Count)];
        AssignProblem(selectedCharacterID, selectedProblem);

        //print("gave problem: " + selectedProblem.name + " to " + GetNameFormatted(selectedCharacterID));
    }

    private void LoadProblems()
    {
        _allProblems = Resources.LoadAll<ProblemData>("Problems").ToList();
    }


    [ButtonMethod]
    public void RandomizeRelationships()
    {
        //print("Randoming all relationships (for testing)");
        for (int i = 0; i < _relationships.Count; i++) {
            _relationships[i].Value = Random.Range(0, 1.5f);
            SaveRelationship(_relationships[i]);
        }
    }

    public List<ID> AllIDs()
    {
        return _allCharacters.Select(x => x.ID).ToList();
    }

    public CompleteCharacterData GetCharacter(ID id)
    {
        return _allCharacters.Find(x =>  x.ID == id);
    }

    public float GetHappiness(ID id)
    {
        var characterData = _allCharacters.Find(c => c.ID == id);
        return characterData != null ? characterData.Happiness : 0;
    }

    public Sprite GetPortrait(ID id)
    {
        var characterData = _allCharacters.Find(c => c.ID == id);
        return characterData != null ? characterData.Icon : null;
    }

    public string GetName(ID id)
    {
        var characterData = _allCharacters.Find(c => c != null && c.ID == id);
        return characterData != null ? characterData.Name : "";
    }

    public Gender GetGender(ID id)
    {
        var characterData = _allCharacters.Find(c => c != null && c.ID == id);
        return characterData != null ? characterData.Gender : Gender.NONBINARY;
    }

    public Attraction GetAttraction(ID id)
    {
        var characterData = _allCharacters.Find(c => c != null && c.ID == id);
        return characterData != null ? characterData.Attraction : Attraction.NONE;
    } 

    public Pronoun GetPronoun(ID id)
    {
        var characterData = _allCharacters.Find(c => c.ID == id);
        var pronoun = characterData.Pronouns;
        if (pronoun == Pronoun.SHE) pronoun = Pronoun.THEY;
        else if (pronoun == Pronoun.THEY) pronoun = Pronoun.SHE;

        return pronoun;
    }

    public string GetPronounString(ID id)
    {
        var pronoun = GetPronoun(id);
        var formatted = Utils.CapitalFirst(pronoun.ToString().ToLower());
        return formatted;
    }

    public string GetPronounOwnership(ID id)
    {
        var pronoun = GetPronoun(id);
        switch (pronoun) {
            case Pronoun.HE: return "himself";
            case Pronoun.SHE: return "herself";
            default: return "themself";
        }
    }

    public string GetBirthdayFormatted(ID id)
    {
        var birthday = _allCharacters.Find(c => c.ID == id).Birthday;
        return birthday.Month + " / " + birthday.Day + " / " + birthday.Year;
    }

    public string GetNameFormatted(ID id)
    {
        var name = GetName(id);
        for (int i = 0; i < name.Length; i++) {
            if (i == 0 || name[i - 1] == ' ') {
                name = name.Substring(0, i) + char.ToUpper(name[i]) + name.Substring(i + 1);
            }
        }

        return name;
    }

    public SpawnedCharacter SpawnCharacterNormalized(ID id, Transform spawnSpot) {
        var character = SpawnCharacter(id, spawnSpot.position, spawnSpot.lossyScale, spawnSpot.eulerAngles);
        var height = character.GetComponentInChildren<HairController>().transform.position.y - spawnSpot.position.y;
        var extraHeight = height - _defaultHeight;
        character.transform.position -= new Vector3(0, extraHeight, 0);
        return character;
    }

    public SpawnedCharacter SpawnCharacter(ID id, Transform spawnSpot) => SpawnCharacter(id, spawnSpot.position, spawnSpot.lossyScale, spawnSpot.eulerAngles);
    private SpawnedCharacter SpawnCharacter(ID id, Vector3 position, Vector3 scale, Vector3 rot)
    {
        //print("Spawning character with ID: " + id);

        var characterData = _allCharacters.Find(c => c.ID == id);
        if (characterData == null) {
            print("tried to spawn character with id: " + id + ", couldn't find");
            return null;
        }
        
        var spawnedCharacter = Instantiate(_characterControllerPrefab, position, Quaternion.Euler(rot)).GetComponent<SpawnedCharacter>();
        spawnedCharacter.LoadFromString(SaveSystem.GetStaticSaveString(id));

        spawnedCharacter.transform.localScale = scale;

        //FIX FOR SCALING BUG, FOR NOW
        //ToggleCharacter(spawnedCharacter.gameObject);

        return spawnedCharacter;
    }

    private async void ToggleCharacter(GameObject character)
    {
        character.SetActive(false);
        await Task.Delay(100);
        character.SetActive(true);
    }

    public void AssignProblem(ID id, ProblemData problem)
    {
        var character = _allCharacters.Find(c => c.ID == id);
        character?.SetProblem(problem);
    }

    private AreaName GetAvailableLocation()
    {
        var availableAreas = _validAreas.Where(x => GetIDsByArea(x).Count < GetCapacity(x)).ToList();
        var chosenArea = availableAreas[Random.Range(0, availableAreas.Count)];
        return chosenArea;
    }

    private async void LoadCharactersFromFile()
    {
        var staticSaveStrings = SaveSystem.LoadAllStaticSaveStrings();

        foreach (var s in staticSaveStrings) {
            var newCharacter = new CompleteCharacterData(s);
            newCharacter.SetArea(GetAvailableLocation());
            if (TownGameManager.i.DemoMode) newCharacter.SetArea(AreaName.TOWN);
            newCharacter.MoveCheckCooldown = Random.Range(0.5f, 2f) * _moveCheckCooldown;
            _allCharacters.Add(newCharacter);
        }

        if (AllCharacters.Count == 0) return;
        var InitilizeCharacter = SpawnCharacter(AllCharacters[0].ID, transform);
        await Task.Delay(200);
        if (InitilizeCharacter && InitilizeCharacter.gameObject) Destroy(InitilizeCharacter.gameObject);
    }

    public float GetRelationship(ID id1, ID id2)
    {
        foreach (var r in _relationships) if (r.Involves(id1, id2)) return r.Value;

        _relationships.Add(new RelationshipData(id1, id2));
        return 0;
    }

    public void SolveAndGenerateProblem(ID id)
    {
        var character = _allCharacters.Find(c => c.ID == id);
        if (character != null) {
            character.SolveProblem();
            character.GiveProblemRewards();
        }

        GenerateProblem();
    }

    /// <summary>
    /// increases the relationship value between two characters by the specified amount.
    /// to decrease relationship, provide a negative value.
    /// </summary>
    public void IncreaseRelationship(ID id1, ID id2, float increase)
    {
        foreach (var r in _relationships) {
            if (r.Involves(id1, id2)) {
                r.Value += increase;
                return;
            }
        }

        _relationships.Add(new RelationshipData(id1, id2, increase));

        SaveRelationship(id1, id2, GetRelationship(id1, id2));
    }

    private void SaveRelationship(RelationshipData data) => SaveRelationship(data.ID1, data.ID2, data.Value);
    private void SaveRelationship(ID id1, ID id2, float value)
    {
        var firstId = Mathf.Max(id1, id2);
        var secondID = Mathf.Min(id1, id2);
        SaveSystem.SaveRelationship(new ID(firstId), new ID(secondID), value);
    }

    /// <summary>
    /// increases the happiness of a given character by the specified amount.
    /// to decrease happiness, provide a negative value.
    /// clamped between 0 and 100.
    /// </summary>
    public void IncreaseHappiness(ID id, float increase)
    {
        var character = _allCharacters.Find(c => c.ID == id);
        character?.IncreaseHappiness(increase);
    }
}
