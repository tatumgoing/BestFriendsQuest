using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ToolTipManager : MonoBehaviour
{
    public static ToolTipManager i;

    [SerializeField] private ToolTipSpacer _tooltipParent;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _letterTime = 0.01f;
    [SerializeField] private Sound _appearSound;

    private Tooltip _currentCaller;
    private string _toDisplay = "";
    private float _letterCooldown;
    private int _currentLetter = 0;
    private bool _hiding;

    private void Awake()
    {
        i = this;
        _currentCaller = null;
        HideToolTip(_currentCaller);
    }

    private void Start()
    {
        _appearSound = Instantiate(_appearSound);
    }

    private void Update()
    {
        if (!_tooltipParent.gameObject.activeInHierarchy || (!_hiding && _currentLetter == _toDisplay.Length-1)) return;

        _letterCooldown -= Time.deltaTime;
        if (_letterCooldown <= 0) {
            _letterCooldown = _letterTime;
            
            if (_hiding) {
                if (_text.text.Length == 0) {
                    _hiding = false;
                    _tooltipParent.gameObject.SetActive(false);
                    return;
                }
                _text.text = _text.text[..^1];
            }
            else {
                _currentLetter += 1;
                _text.text += _toDisplay[_currentLetter];
            }
        }
    }

    public void DisplayToolTip(string message, Tooltip caller)
    {
        if (message.Length == 0) return;

        _appearSound.Play();
        
        _currentCaller = caller;
        _tooltipParent.gameObject.SetActive(true);

        _toDisplay = message;
        _text.text = "";
        _letterCooldown = 0;
        _currentLetter = -1;

        _hiding = false;
        _tooltipParent.Updating = true;

        _tooltipParent.transform.position = Input.mousePosition;
        _tooltipParent.GetComponent<RectTransform>().anchoredPosition += (Vector2.up * 12f);
    }

    public void HideToolTip(Tooltip caller)
    {
        if (_currentCaller != caller) return;

        _tooltipParent.Updating = false;
        _hiding = true;
    }
}
