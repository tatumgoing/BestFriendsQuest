using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[System.Serializable]
public class HelpMenuInfo
{
    [HideInInspector] public string Name;
    [HideInInspector] public int ID;
    public string Title;
    [TextArea(3, 10)] public string Description;
}

public class HelpPopupController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _destriptionText;

    [SerializeField] private List<HelpMenuInfo> _info;

    private void OnValidate()
    {
        for (var i = 0; i < _info.Count; i++) {
            _info[i].ID = i;
            _info[i].Name = i.ToString();
        }
    }

    public void OpenHelpMenu(int ID)
    {
        var selected = _info.Where(x => x.ID == ID).FirstOrDefault();
        if (selected == null) return;

        _titleText.text = selected.Title;
        _destriptionText.text = selected.Description;
        gameObject.SetActive(true);
    }
}
