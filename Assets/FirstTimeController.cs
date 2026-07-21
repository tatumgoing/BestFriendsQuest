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
    [SerializeField] private float _sineWaveDuration;
    [SerializeField] private float _letterDelayTime = 0.1f;
    [SerializeField] private Sound _letterSound;
    [SerializeField] private Sound _pageTurnSound;

    private float _letterDelayCooldown;
    private string _currentTextRemaining = "";

    private int _selectedIndex = -1;

    private void Start()
    {
        _textBoxParent.SetActive(true);
        _inviteNewButton.SetActive(false);

        _letterSound = Instantiate(_letterSound);
        _pageTurnSound = Instantiate(_pageTurnSound);

        //Next();
    }

    private void Update()
    {
        if (CharacterManager.i.AllCharacters.Count != 0 || !TownGameManager.i.DemoMode) {
            gameObject.SetActive(false);
            return;
        }

        if (_currentTextRemaining.Length > 0) AnimateText();

        AnimateCamera();
    }

    private void AnimateText()
    {
        _letterDelayCooldown -= Time.deltaTime;
        if (_letterDelayCooldown <= 0) {
            _letterDelayCooldown = _letterDelayTime;
            _textBox.text += _currentTextRemaining[0];
            _letterSound.Play(restart: false);
            _currentTextRemaining = _currentTextRemaining.Substring(1);
        }
    }

    private void AnimateCamera()
    {
        var offset = Mathf.Sin(Time.time * _sineWaveFreq) * _sineWaveAmp;
        _cutsceneSet.setCamera(_camPos, _camRot + _sinAxis * offset);

        _sineWaveDuration -= Time.deltaTime;
        if (_sineWaveDuration <= 0) _sineWaveAmp = Mathf.Lerp(_sineWaveAmp, 0, 2 * Time.deltaTime);
    }

    public void Next()
    {
        if (_currentTextRemaining.Length > 0) {
            _textBox.text += _currentTextRemaining;
            _currentTextRemaining = "";
            _letterSound.Play(restart: false);
            return;
        }

        _pageTurnSound.Play();

        _selectedIndex++;

        if (_selectedIndex == _text.Count) {
            _textBoxParent.GetComponent<Animator>().SetTrigger("Hide");
            _inviteNewButton.SetActive(true);
            return;
        }

        _textBox.text = "";
        _currentTextRemaining = _text[_selectedIndex];
    }

    public void InviteNew()
    {
        TownMusicPlayer.i.FadeOutCurrent();
        TownGameManager.i.LoadCharacterCreator();
    }
}
