using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionButtonsController : MonoBehaviour
{
    [SerializeField] private SelectableItem _neutralButton;

    private bool _clicking;

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
