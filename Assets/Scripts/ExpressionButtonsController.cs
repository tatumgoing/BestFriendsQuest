using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ExpressionButtonsController : MonoBehaviour
{
    [SerializeField] private SelectableItem _neutralButton;
    [SerializeField] private GameObject _tutorial;
    [SerializeField] private float _tutorialOffset;

    private RectTransform _rTransform;
    private float _startX;
    private bool _clicking;
    private bool _showing;

    public async void Show()
    {
        if (_showing || gameObject.activeInHierarchy) return;

        gameObject.SetActive(false);
        _showing = true;

        await Task.Delay(2000);

        gameObject.SetActive(true);

        foreach (var button in GetComponentsInChildren<SelectableItem>()) {
            button.Select(true, false);
            button.Deselect(true, false);
        }

        _showing = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) _clicking = true;
        if (Input.GetMouseButtonUp(0) && _clicking) {
            _neutralButton.Select(true);
        }

        if (!_rTransform) {
            _rTransform = transform.parent.GetComponent<RectTransform>();
            _startX = _rTransform.anchoredPosition.x;
        }
        _rTransform.anchoredPosition = new Vector2(_startX + (_tutorial.activeInHierarchy ? _tutorialOffset : 0), _rTransform.anchoredPosition.y);
    }

    public void PickExpression()
    {
        _clicking = false;
    }
}
