using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ExpressionButtonsController : MonoBehaviour
{
    [SerializeField] private SelectableItem _neutralButton;

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
    }

    public void PickExpression()
    {
        _clicking = false;
    }
}
