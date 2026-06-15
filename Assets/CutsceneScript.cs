using MyBox;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CutsceneScript
{
    public enum ReqType { HAPPINESS, RELATIONSHIP, PERSONALITY}

    [System.Serializable]
    public class Requirement
    {
        [HideInInspector] public string DisplayName;
        [SerializeField] private ReqType _type;

        [SerializeField, ConditionalField(nameof(_type), true, false, ReqType.RELATIONSHIP)] private CutsceneSpeaker _whichSpeaker;

        [SerializeField, ConditionalField(nameof(_type), false, false, ReqType.HAPPINESS)] private Vector2Int _minMaxHappiness;

        [SerializeField, ConditionalField(nameof(_type), false, false, ReqType.RELATIONSHIP)] private Vector2 _minMaxRelationship;

        [SerializeField, ConditionalField(nameof(_type), false, false, ReqType.PERSONALITY)] private PersonalityType _personality;

        public void OnValidate()
        {
            _minMaxRelationship.x = Mathf.Max(_minMaxRelationship.x, 0);

            _minMaxHappiness.x = Mathf.Max(_minMaxHappiness.x, 0);
            _minMaxHappiness.y = Mathf.Min(_minMaxHappiness.y, 100);

            if (_type == ReqType.PERSONALITY) {
                DisplayName = _whichSpeaker + " must be " + _personality.ToString().ToLower();
            }
            if (_type == ReqType.RELATIONSHIP) {
                DisplayName = "Relationship must be between " + _minMaxRelationship.x + " and " + _minMaxRelationship.y;
            }
            if (_type == ReqType.HAPPINESS) {
                DisplayName = _whichSpeaker.ToString().ToLower() + "'s happiness must be between " + _minMaxHappiness.x + " and " + _minMaxHappiness.y;
            }
        }

        public bool Check(ID c1, ID c2)
        {
            if (_type == ReqType.RELATIONSHIP) {
                return Utils.InRange(CharacterManager.i.GetRelationship(c1, c2), _minMaxRelationship);
            }

            var chosen = _whichSpeaker == CutsceneSpeaker.SPEAKER_1 ? c1 : c2;

            if (_type == ReqType.HAPPINESS) return Utils.InRange(CharacterManager.i.GetHappiness(chosen), _minMaxHappiness);

            if (_type == ReqType.PERSONALITY) return CharacterManager.i.GetPersonality(chosen).Type == _personality;

            return true;
        }
    }

    [HideInInspector] public string DisplayName;
    [SerializeField] private List<Requirement> _requirements;
    [SerializeField] TextAsset _script;

    public bool Monologue => !_script.text.Contains("c2");
    public TextAsset Script => _script;

    public void OnValidate()
    {
        if (_script != null) DisplayName = _script.name;
        else DisplayName = "MISSING SCRIPT";

        foreach (var r in _requirements) r.OnValidate();
    }

    public bool Check(ID c1, ID c2)
    {
        foreach (var r in _requirements) if (!r.Check(c1, c2)) return false;
        return true;
    }
}
