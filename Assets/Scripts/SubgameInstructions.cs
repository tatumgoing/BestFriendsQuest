using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class InstructionData
{
    public SubgameType Type;
    public GameObject Parent;
}

public class SubgameInstructions : MonoBehaviour
{
    [SerializeField] private SubgameController _controller;
    [SerializeField] private List<InstructionData> _instructions;
    [SerializeField] private TextMeshProUGUI _titleText;

    public void Show(SubgameType type)
    {
        var titleString = "How to ";
        if (type == SubgameType.CHOPPING) titleString += "Chop";
        else if (type == SubgameType.STIRRING) titleString += "Stir";
        else if (type == SubgameType.BOILING) titleString += "Boil";
        else if (type == SubgameType.GRILLING) titleString += "Grill";
        else if (type == SubgameType.STEAMING) titleString += "Steam";
        else titleString += "Cook";
        _titleText.text = titleString;

        foreach (var i in _instructions) i.Parent.SetActive(i.Type == type);

        gameObject.SetActive(true);
    }

    public void Continue()
    {
        gameObject.SetActive(false);
        _controller.StartCountdown();
    }
}
