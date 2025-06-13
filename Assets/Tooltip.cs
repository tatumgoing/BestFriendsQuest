using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string _message;

    private const float _hoverTime = 0.7f;

    private float _enterTime = Mathf.Infinity;
    private bool _showing;

    private void Start()
    {
        UppercaseFirstLetter();
    }

    private void Update()
    {
        if (Time.time - _enterTime > _hoverTime) Show();
    }

    private void OnDestroy()
    {
        Hide();
    }

    private void OnDisable()
    {
        Hide();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _enterTime = Time.time;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _enterTime = Mathf.Infinity;
        if (_showing) Hide();
    }

    public void UpdateText(string newText)
    {
        _message = newText;
        UppercaseFirstLetter();
    }

    private void UppercaseFirstLetter()
    {
        if (_message.Length == 0) return;

        var firstLetter = _message[0].ToString().ToUpper();
        _message = _message.Substring(1);
        _message = firstLetter + _message;
    }

    private void Show()
    {
        if (_showing) return;

        _showing = true;
        ToolTipManager.i.DisplayToolTip(_message, this);
    }

    private void Hide()
    {
        _enterTime = Mathf.Infinity;

        if (!_showing) return;
        _showing = false;
        ToolTipManager.i.HideToolTip(this);
    }
}
