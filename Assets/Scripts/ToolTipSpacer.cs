using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipSpacer : MonoBehaviour
{
    [ReadOnly] public bool Updating;

    [SerializeField] private RectTransform _rightEdge;
    [SerializeField] private RectTransform _tooltipParent;
    [SerializeField] private float _edgeOffset = 50;


    private void Update()
    {
        if (!Updating) return;

        var screenPos = RectTransformUtility.WorldToScreenPoint(null, _rightEdge.position);
        var screenOverFlow = screenPos.x - (Screen.width - _edgeOffset);
        
        if (screenOverFlow > 0) {
            var anchoredPos = _tooltipParent.anchoredPosition;
            anchoredPos.x -= screenOverFlow;
            _tooltipParent.anchoredPosition = Vector2.Lerp(_tooltipParent.anchoredPosition, anchoredPos, 15 * Time.deltaTime);
        }
    }
}
