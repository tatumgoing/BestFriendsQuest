using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager i;

    [SerializeField] private GameObject _characterControllerPrefab;
    [SerializeField] public List<CompleteCharacterData> allCharacters = new List<CompleteCharacterData>();
    [SerializeField] private List<RelationshipData> _relationships = new List<RelationshipData>();

    public List<CompleteCharacterData> AllCharacters => allCharacters;
    public CompleteCharacterData GetRandomCharacter() => AllCharacters[Random.Range(0, AllCharacters.Count)];

    void Awake()
    {
        i = this;
        LoadCharactersFromFile();
    }

    public List<ID> AllIDs()
    {
        return allCharacters.Select(x => x.ID).ToList();
    }

    public CompleteCharacterData GetCharacter(ID id)
    {
        return allCharacters.Find(x =>  x.ID == id);
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

    public void AssignProblem(ID id, Problem problem)
    {
        var character = allCharacters.Find(c => c.ID == id);
        character?.SetProblem(problem);
    }

    private void LoadCharactersFromFile()
    {
        var staticSaveStrings = SaveSystem.LoadAllStaticSaveStrings();

        foreach (var s in staticSaveStrings) {
            allCharacters.Add(new CompleteCharacterData(s));
        }
        
    }

    private void InitializeTestCharacters()
    {


        /*CharacterData Johnny = new CharacterData();
        CharacterData Sally = new CharacterData();
        CharacterData Goobert = new CharacterData();

        Johnny.CharacterName = "Johnny";
        Sally.CharacterName = "Sally";
        Goobert.CharacterName = "Goobert";

        //testing code to be deleted later

        allCharacters.Add(Johnny);
        allCharacters.Add(Sally);
        allCharacters.Add(Goobert);

        Johnny.Happiness = 100;
        Sally.Happiness = 100;

        Johnny.UpdateRelationship(Sally, 0.25f);
        Sally.UpdateRelationship(Johnny, 0.25f);*/
    }

    public float GetRelationship(ID id1, ID id2)
    {
        foreach (var r in _relationships) if (r.Involves(id1, id2)) return r.Value;

        _relationships.Add(new RelationshipData(id1, id2));
        return 0;
    }

    public void SolveProblem(ID id)
    {
        var character = allCharacters.Find(c => c.ID == id);
        character?.SolveProblem();
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
