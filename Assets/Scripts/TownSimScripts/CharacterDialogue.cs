using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterDialogue : MonoBehaviour
{
    public CharacterData associatedCharacter;

    public List<string> dialogues = new List<string>();
    public TMP_Text dialogueBox;
    public GameObject textBox;

    // Start is called before the first frame update
    void Start()
    {
        //temp dialogues 

        dialogues.Add("Life is like a dream!");
        dialogues.Add("Lorem ipsum- haha Just Kidding Can You Imagine?");
        dialogues.Add("Whats the weather like in your world?");
        dialogues.Add("It's good having so many neighbors in this town. Maybe we ought to have a party soon.");
        dialogues.Add("Thanks for checking in on me!");

    }

    public void DisplayDialogue()
    {
        //enables the text box and then displays the dialogue
        textBox.SetActive(true);

        if(associatedCharacter.hasProblem)
        {
            dialogueBox.text = associatedCharacter.currentProblem.problemDialogue;
        }
        else
        {
            dialogueBox.text = RandomDialogue();
        }
    }
    public string RandomDialogue()
    {
        //finds random dialogue string and returns it
        int index = Random.Range(0, dialogues.Count);
        return dialogues[index];
    }
}
