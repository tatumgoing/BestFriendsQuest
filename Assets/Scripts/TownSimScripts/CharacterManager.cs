using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] public List<CharacterData> allCharacters = new List<CharacterData>();

    //bad bad evil temporary variable bad bad bad

    public List<Sprite> characterSprites = new List<Sprite>();

    void Awake()
    {
        CharacterData Johnny = new CharacterData();
        CharacterData Sally = new CharacterData();
        CharacterData Goobert = new CharacterData();

        Johnny.characterName = "Johnny";
        Sally.characterName = "Sally";
        Goobert.characterName = "Goobert";

        //testing code to be deleted later

        allCharacters.Add(Johnny);
        allCharacters.Add(Sally);
        allCharacters.Add(Goobert);

        Johnny.happiness = 100;
        Sally.happiness = 100;

        foreach (CharacterData character in allCharacters) {

            foreach (CharacterData reloCharacter in allCharacters)
            {
                character.CreateRelationship(reloCharacter);
            }

        }


        for (int i = 0; i < allCharacters.Count; i++)
        {

            allCharacters[i].UpdateIcon(characterSprites[i]);
        }


    }
  

}
