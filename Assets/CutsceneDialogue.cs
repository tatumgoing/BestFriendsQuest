using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum CutsceneSpeaker { SPEAKER_1, SPEAKER_2};

public class CutsceneDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textBox;
    [SerializeField] private float _letterDelayTime;
    [SerializeField] private GameObject _c1Parent;
    [SerializeField] private GameObject _c2Parent;
    [SerializeField] private TextMeshProUGUI _c1Name;
    [SerializeField] private TextMeshProUGUI _c2Name;
    [SerializeField] private float _lookUpAmount = 2;

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
            if (_currentLineLeft.Length > 0) _currentLineLeft = _currentLineLeft.Substring(1);
        }
    }

    public async void StartDialogue(List<string> lines, List<string> names, SpawnedCharacter speaker1, SpawnedCharacter speaker2, Transform camera)
    {
        _lerpingCam = false;
        _targetCamRot = camera.localRotation;
        _camera = camera;
        _speaker1 = speaker1;
        _speaker2 = speaker2;

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

    private CutsceneLine ParseMetaLine(string line)
    {
        line = line.Trim().ToUpper().Replace(" ", "");
        var parts = line.Split(',');

        //print("Parsing meta line. line: " + line + ", parts: " + string.Join(", ", parts));

        var newLine = new CutsceneLine(CutsceneSpeaker.SPEAKER_1);

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
            _textBox.text += _currentLineLeft;
            _currentLineLeft = "";
            return;
        }

        _currentLineIndex += 1;
        _textBox.text = "";

        if (_currentLineIndex >= _dialogue.Count) {
            EndConversation();
            return;
        }

        if (_currentLine.MetaLine) {
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

            Next();
            return;
        }

        _currentLineLeft = _currentLine.Line;
        _c1Parent.SetActive(_currentLine.Speaker == CutsceneSpeaker.SPEAKER_1);
        _c2Parent.SetActive(_currentLine.Speaker == CutsceneSpeaker.SPEAKER_2);
    }

    private void EndConversation()
    {
        CutsceneManager.i.EndCutscene();
        gameObject.SetActive(false);
    }
}
