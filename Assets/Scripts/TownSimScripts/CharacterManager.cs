using MyBox;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager i;

    [Header("Characters")]
    [SerializeField] private GameObject _characterControllerPrefab;
    [SerializeField] public List<CompleteCharacterData> allCharacters = new List<CompleteCharacterData>();
    [SerializeField] private List<RelationshipData> _relationships = new List<RelationshipData>();

    [Header("Problems")]
    [SerializeField, Range(0, 1), Tooltip("Max percent of citizens that can have problems")] private float _maxProblemPercent = 0.3f;
    [SerializeField] private List<ProblemData> _allProblems;

    public List<CompleteCharacterData> AllCharacters => allCharacters;
    public CompleteCharacterData GetRandomCharacter() => AllCharacters[Random.Range(0, AllCharacters.Count)];
    public string GetAge(ID id) => allCharacters.Find(c => c.ID == id).Age.ToString();
    public string GetFavoriteColor(ID id) => Utils.CapitalFirst(allCharacters.Find(c => c.ID == id).FavColor.ToString().ToLower());

    void Awake()
    {
        i = this;
        LoadCharactersFromFile();
        LoadProblems();
    }

    private void Start()
    {
        GenerateProblem();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) RandomizeRelationships(); //TESTING

        var problemRatio = (float)numCharactersWithProblems() / allCharacters.Count;
        if (problemRatio < _maxProblemPercent) GenerateProblem();
    }

    private int numCharactersWithProblems()
    {
        int numProblems = 0;
        foreach (var character in allCharacters) if (character.HasProblem) numProblems++;
        return numProblems;
    }

    public void GenerateProblem()
    {
        var validOptions = AllCharacters.Where(x => !x.HasProblem)  .ToList();
        GenerateProblem(validOptions[Random.Range(0, validOptions.Count)].ID);
    }

    private void GenerateProblem(ID selectedCharacterID)
    {
        var selectedProblem = _allProblems[Random.Range(0, _allProblems.Count)];
        AssignProblem(selectedCharacterID, selectedProblem);

        print("gave problem: " + selectedProblem.name + " to " + GetNameFormatted(selectedCharacterID));
    }

    private void LoadProblems()
    {
        _allProblems = Resources.LoadAll<ProblemData>("Problems").ToList();
    }

    [ButtonMethod]
    public void RandomizeRelationships()
    {
        print("RANDOMIZING RELATIONSHIPS");
        for (int i = 0; i < _relationships.Count; i++) {
            _relationships[i].Value = Random.Range(0, 10f);
        }
    }

    public List<ID> AllIDs()
    {
        return allCharacters.Select(x => x.ID).ToList();
    }

    public CompleteCharacterData GetCharacter(ID id)
    {
        return allCharacters.Find(x =>  x.ID == id);
    }

    public float GetHappiness(ID id)
    {
        var characterData = allCharacters.Find(c => c.ID == id);
        return characterData != null ? characterData.Happiness : 0;
    }

    public Sprite GetPortrait(ID id)
    {
        var characterData = allCharacters.Find(c => c.ID == id);
        return characterData != null ? characterData.Icon : null;
    }

    public string GetName(ID id)
    {
        var characterData = allCharacters.Find(c => c.ID == id);
        return characterData != null ? characterData.Name : "";
    }

    public Pronoun GetPronoun(ID id)
    {
        var characterData = allCharacters.Find(c => c.ID == id);
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
        var birthday = allCharacters.Find(c => c.ID == id).Birthday;
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

    public SpawnedCharacter SpawnCharacter(ID id, Transform spawnSpot) => SpawnCharacter(id, spawnSpot.position, spawnSpot.lossyScale, spawnSpot.eulerAngles);
    private SpawnedCharacter SpawnCharacter(ID id, Vector3 position, Vector3 scale, Vector3 rot)
    {
        var characterData = allCharacters.Find(c => c.ID == id);
        if (characterData == null) return null;
        
        var spawnedCharacter = Instantiate(_characterControllerPrefab, position, Quaternion.Euler(rot)).GetComponent<SpawnedCharacter>();
        spawnedCharacter.LoadFromString(SaveSystem.GetStaticSaveString(id));

        spawnedCharacter.transform.localScale = scale;

        //FIX FOR SCALING BUG, FOR NOW
        ToggleCharacter(spawnedCharacter.gameObject);

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
        var character = allCharacters.Find(c => c.ID == id);
        character?.SetProblem(problem);
    }

    private async void LoadCharactersFromFile()
    {
        var staticSaveStrings = SaveSystem.LoadAllStaticSaveStrings();

        foreach (var s in staticSaveStrings) {
            allCharacters.Add(new CompleteCharacterData(s));
        }

        var newCharacter = SpawnCharacter(AllCharacters[0].ID, transform);
        await Task.Delay(200);
        Destroy(newCharacter.gameObject);
    }

    public float GetRelationship(ID id1, ID id2)
    {
        foreach (var r in _relationships) if (r.Involves(id1, id2)) return r.Value;

        _relationships.Add(new RelationshipData(id1, id2));
        return 0;
    }

    public void SolveAndGenerateProblem(ID id)
    {
        var character = allCharacters.Find(c => c.ID == id);
        character?.SolveProblem();

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
    }

    /// <summary>
    /// increases the happiness of a given character by the specified amount.
    /// to decrease happiness, provide a negative value.
    /// clamped between 0 and 100.
    /// </summary>
    public void IncreaseHappiness(ID id, float increase)
    {
        var character = allCharacters.Find(c => c.ID == id);
        character?.IncreaseHappiness(increase);
    }
}
