using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable] 
public class TutorialData
{
    [HideInInspector] public string DisplayName;
    [TextArea(2, 10)] public string Text;
    public TextMeshProUGUI TexBox;
    public GameObject Parent;
}

public class CreatorTutorial : MonoBehaviour
{
    [SerializeField] private List<TutorialData> _steps;

    private int _current = -1;
    private bool _changingSlider = false;
    private float _timeWhenLastAdvanced;

    private void OnValidate()
    {
        for (int i = 0; i < _steps.Count; i++) {
            _steps[i].DisplayName = (i+1) + ": " + _steps[i].Text;
        }
    }

    private void Start()
    {
        _timeWhenLastAdvanced = 100000;
        _current = -1;
        Next();
    }

    private void Update()
    {
        if (_changingSlider && Input.GetMouseButtonUp(0) && _current == 5) {
            _changingSlider = false;
            Next();
        }
    }

    public void Next()
    {
        if (Mathf.Abs(Time.time - _timeWhenLastAdvanced) < 0.25f) return;
        _timeWhenLastAdvanced = Time.time;  

        _current++;
        if (_current >= _steps.Count) {
            gameObject.SetActive(false);
            return;
        }

        if (_current > 0) _steps[_current - 1].Parent.SetActive(false);

        _steps[_current].Parent.SetActive(true);
        _steps[_current].TexBox.text = _steps[_current].Text;
    }

    public void OpenEyesCategory()
    {
        if (_current == 1) Next();
    }

    public void SwitchToColorTab()
    {
        if (_current == 6) Next();
    }

    public void OpenBaseAddMenu()
    {
        if (_current == 2) Next();
    }

    public void AddFeature()
    {
        if (_current == 3 || _current == 10) Next();
    }

    public void PickColor()
    {
        if (_current == 7) Next();
    }

    public void AddDetail()
    {
        if (_current == 9) Next();
    }

    public void SwitchToLayersTab()
    {
        if (_current == 8) Next();
    }

    public void StartChangingSlider(float value)
    {
        _changingSlider = true;
    }
}
