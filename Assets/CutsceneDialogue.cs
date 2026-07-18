using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum CutsceneSpeaker { SPEAKER_1, SPEAKER_2};

public class CutsceneDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textBox;
    [SerializeField] private float _letterDelayTime;
    [SerializeField] private TextMeshProUGUI _c1Name;
    [SerializeField] private TextMeshProUGUI _c2Name;
    [SerializeField] private float _lookUpAmount = 2;
    [SerializeField] private float _nextPaperwaitTime = 0.6f;
    [SerializeField] private GameObject _playerNameEntryParent;
    [SerializeField] private Vector2 _rotLimits;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _backing;
    [SerializeField] private GameObject _helperParent;
    [SerializeField] private Sound _nextLineSound;
    [SerializeField] private Sound _letterSound;

    private int _currentLineIndex;
    private string _currentLineLeft = "";
    private float _letterCountdown;
    private List<CutsceneLine> _dialogue = new List<CutsceneLine>();
    private CutsceneLine _currentLine => _dialogue[_currentLineIndex];

    private SpawnedCharacter _speaker1;
    private SpawnedCharacter _speaker2;
    private Transform _camera;
    private Quaternion _targetCamRot = new Quaternion();
    private bool _lerpingCam;
    private List<GameObject> _spawnedBackings = new List<GameObject>();

    private void Start()
    {
        _nextLineSound = Instantiate(_nextLineSound);
        _letterSound = Instantiate(_letterSound);
    }

    private void Update()
    {
        if (_lerpingCam && _camera != null && _camera.gameObject.activeInHierarchy) {
            _camera.localRotation = Quaternion.Slerp(_camera.localRotation, _targetCamRot, 5 * Time.deltaTime);
        }

        if (_currentLineLeft.Length == 0) return;

        _letterCountdown -= Time.deltaTime;
        if (_letterCountdown <= 0) {
            _letterCountdown = _letterDelayTime;

            _textBox.text += _currentLineLeft[0];
            _letterSound.Play(restart: false);
            if (_currentLineLeft.Length > 0) _currentLineLeft = _currentLineLeft.Substring(1);
        }
    }

    private void OnDisable()
    {
        foreach (var s in _spawnedBackings) Destroy(s);
        _spawnedBackings.Clear();
    }

    public async void StartDialogue(List<string> lines, List<string> names, SpawnedCharacter speaker1, SpawnedCharacter speaker2, Transform camera)
    {
        _lerpingCam = true; //defaults to true
        _targetCamRot = camera.localRotation;
        _camera = camera;
        _speaker1 = speaker1;
        _speaker2 = speaker2;

        _backing.GetComponent<StickerRandomizer>().Randomize();

        _c1Name.text = "";
        _c2Name.text = "";

        _playerNameEntryParent.SetActive(false);

        if (speaker1) {
            var original = _camera.localRotation;
            _camera.LookAt(speaker1.transform.position + Vector3.up * _lookUpAmount);
            if (_lerpingCam) {
                _targetCamRot = _camera.localRotation;
                _camera.localRotation = original;
            }
        }

        gameObject.SetActive(true);

        if (names.Count > 0) _c1Name.text = names[0];
        if (names.Count > 1) _c2Name.text = names[1];

        _textBox.text = "";
        _currentLineIndex = -1;
        _dialogue.Clear();
        ParseDialogue(lines, names[0], names.Count > 1 ? names[1] : "");

        await Task.Delay(1000);

        Next();
    }

    private void ParseDialogue(List<string> rawLines, string c1Name, string c2Name)
    {
        foreach (var line in rawLines) {

            if (!line.Contains(":")) {
                var metaLine = ParseMetaLine(line.Trim().ToUpper());
                if (metaLine != null) _dialogue.Add(metaLine);
                continue;
            }

            var parts = line.Split(":");
            var lineData = parts[0].ToUpper().Trim();

            var newLine = new CutsceneLine(CutsceneSpeaker.SPEAKER_1, parts[1]);
            newLine.Format(c1Name, c2Name);
            if (lineData.Contains("C2")) newLine.Speaker = CutsceneSpeaker.SPEAKER_2;

            _dialogue.Add(newLine);
        }
    }

    private string ReplaceKeywords(string rawString)
    {
        var dict = SaveSystem.GetDialogueDict();

        return Regex.Replace(rawString, @"<(.*?)>", match =>
        {
            string key = match.Groups[1].Value.Trim().ToUpper();
            if (dict.TryGetValue(key, out var value)) return value;
            return match.Value;
        });
    }

    private CutsceneLine ParseMetaLine(string line)
    {
        var newLine = new CutsceneLine(CutsceneSpeaker.SPEAKER_1);

        //parsing commands
        if (line.Trim()[0] == '/') {
            line = line.Trim().ToUpper();
            if (line.Contains("ShowNameEntry".ToUpper())) {
                newLine.SetCommand(CutsceneCommand.SHOW_NAME_ENTRY);
                return newLine;
            }
        }

        line = line.Trim().ToUpper().Replace(" ", "");
        var parts = line.Split(',');

        if (parts[0].Contains("SETTINGS")) {
            if (parts[1].Contains("LERPCAM")) {
                _lerpingCam = true;
                parts = parts.RemoveAt(1);
            }

            return newLine;
        }

        if (parts[0].Contains("CAM")) {
            var lookTarget = _speaker1.transform;
            if (parts[1].Contains("C2")) lookTarget = _speaker2.transform;

            newLine.SetCamAngle(lookTarget);
            return newLine;
        }

        if (parts[0].Contains("C2")) newLine.Speaker = CutsceneSpeaker.SPEAKER_2;
        parts = parts.RemoveAt(0);
        if (parts.Length == 0) return null;

        var expressionOptions = Utils.EnumToList<Expression>().Select(x => x.ToString().Trim().ToUpper()).ToList();
        for (int i = 0; i < expressionOptions.Count; i++) {
            if (string.Compare(expressionOptions[i], parts[0]) == 0) {
                newLine.SetExpression(Utils.EnumToList<Expression>()[i]);
                parts = parts.RemoveAt(0);

                if (parts.Length == 0) return newLine; 
            }
        }

        var animationOptions = Utils.EnumToList<CharacterAnimations>().Select(x => x.ToString().Trim().ToUpper()).ToList();
        for (int i = 0; i < animationOptions.Count; i++) {
            if (string.Compare(animationOptions[i], parts[0]) == 0) {
                newLine.SetAnimation(Utils.EnumToList<CharacterAnimations>()[i]);
                parts = parts.RemoveAt(0);

                if (parts.Length == 0) return newLine;
            }
        }


        return newLine;
    }

    public void Next()
    {
        if (_currentLineLeft.Length > 0) {
            SkipAnimation();
            return;
        }

        _currentLineIndex += 1;
        _textBox.text = "";

        if (_currentLineIndex >= _dialogue.Count) EndConversation();
        else if (_currentLine.MetaLine) HandleMetaLine();
        else ShowNextLine();
    }

    private void ShowNextLine()
    {
        _nextLineSound.Play();

        if (_currentLineIndex > 1) {

            var newHelperParent = Instantiate(_helperParent, transform.parent);
            newHelperParent.transform.localEulerAngles = transform.localEulerAngles;
            newHelperParent.transform.SetSiblingIndex(_spawnedBackings.Count);

            var NewBacking = Instantiate(_backing, newHelperParent.transform);
            _spawnedBackings.Add(newHelperParent);

            if (_spawnedBackings.Count > 4) {
                Destroy(_spawnedBackings[0]);
                _spawnedBackings.RemoveAt(0);
            }

            _animator.SetTrigger("Next");

            _backing.GetComponent<StickerRandomizer>().Randomize();
            transform.localEulerAngles = Vector3.forward * Mathf.Pow(Utils.Rand(_rotLimits), 2);
        }

        _letterCountdown = _nextPaperwaitTime;

        _currentLineLeft = ReplaceKeywords(_currentLine.Line.Trim());
        _c1Name.gameObject.SetActive(_currentLine.Speaker == CutsceneSpeaker.SPEAKER_1);
        _c2Name.gameObject.SetActive(_currentLine.Speaker == CutsceneSpeaker.SPEAKER_2);
    }

    private void HandleMetaLine()
    {
        var currentSpeaker = _currentLine.Speaker == CutsceneSpeaker.SPEAKER_1 ? _speaker1 : _speaker2;

        if (_currentLine.HasExpression) currentSpeaker.SetExpression(_currentLine.Expression);

        if (_currentLine.HasAnimation) currentSpeaker.SetAnimation(_currentLine.Animation);

        if (_currentLine.HasCamAngle) {
            var original = _camera.localRotation;
            _camera.LookAt(_currentLine.LookPos + Vector3.up * _lookUpAmount);
            if (_lerpingCam) {
                _targetCamRot = _camera.localRotation;
                _camera.localRotation = original;
            }
        }

        if (_currentLine.HasCommand) {

            if (_currentLine.Command == CutsceneCommand.SHOW_NAME_ENTRY) {
                _playerNameEntryParent.SetActive(true);
                return;
            }
        }

        Next();
        return;
    }

    private void SkipAnimation()
    {
        _textBox.text += _currentLineLeft;
        _currentLineLeft = "";
    }

    private void EndConversation()
    {
        CutsceneManager.i.EndCutscene();
        gameObject.SetActive(false);
    }
}
