using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] public List<CharacterData> allCharacters = new List<CharacterData>();

    //bad bad evil temporary variable bad bad bad

    public List<Sprite> characterSprites = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        CharacterData Johnny = new CharacterData();
        CharacterData Sally = new CharacterData();
        CharacterData Goobert = new CharacterData();

        Johnny.characterName = "Johnny";
        Sally.characterName = "Sally";
        Goobert.characterName = "Goobert";

        allCharacters.Add(Johnny);
        allCharacters.Add(Sally);
        allCharacters.Add(Goobert); 
    }

    // Update is called once per frame
    void Update()
    {
        //bad bad evil temporary code bad bad bad
        for (int i = 0; i < allCharacters.Count; i++) {

            allCharacters[i].UpdateIcon(characterSprites[i]);
        }

    }

}
