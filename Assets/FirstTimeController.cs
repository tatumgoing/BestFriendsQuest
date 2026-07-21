using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FirstTimeController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textBox;
    [SerializeField, TextArea(2,5)] private List<string> _text = new List<string>();
    [SerializeField] private Vector3 _camPos;
    [SerializeField] private Vector3 _camRot;
    [SerializeField] private CutsceneSets _cutsceneSet;
    [SerializeField] private GameObject _textBoxParent;
    [SerializeField] private GameObject _inviteNewButton;
    [SerializeField] private Vector3 _sinAxis = Vector3.up;
    [SerializeField] private float _sineWaveFreq = 1;
    [SerializeField] private float _sineWaveAmp = 1;

    private int _selectedIndex = -1;

    private void Start()
    {
        _textBoxParent.SetActive(true);
        _inviteNewButton.SetActive(false);

        Next();
    }

    private void Update()
    {
        if (CharacterManager.i.AllCharacters.Count != 0 || !TownGameManager.i.DemoMode) {
            gameObject.SetActive(false);
            return;
        }

        var offset = Mathf.Sin(Time.time * _sineWaveFreq) * _sineWaveAmp;
        _cutsceneSet.setCamera(_camPos, _camRot + _sinAxis * offset);
    }

    public void Next()
    {
        _selectedIndex++;

        if (_selectedIndex == _text.Count) {
            _textBoxParent.GetComponent<Animator>().SetTrigger("Hide");
            _inviteNewButton.SetActive(true);
            return;
        }

        _textBox.text = _text[_selectedIndex];       
    }

    public void InviteNew()
    {
        TownGameManager.i.LoadCharacterCreator();
    }
}
