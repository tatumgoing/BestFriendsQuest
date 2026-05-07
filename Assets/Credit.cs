using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class CreditData
{
    public string Name;
    public string Role;
}

public class Credit : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _roleText;
    [SerializeField] private TextMeshProUGUI _centerText;

    public void Initialize(CreditData data)
    {
        if (data.Role.Length > 2) {
            _nameText.text = data.Name;
            _roleText.text = data.Role;
            _centerText.text = "";
        }
        else {
            _nameText.text = "";
            _roleText.text = "";
            _centerText.text = data.Name;
        }
    }
}
