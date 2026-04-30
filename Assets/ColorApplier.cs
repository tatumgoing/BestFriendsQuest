using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorApplier : MonoBehaviour
{
    [SerializeField] private List<Material> _exceptions;
    [SerializeField] private Color _mixColor;
    [SerializeField, Range(0,1)] private float _lerpAmount;
    [SerializeField] private bool _advanced;
    [SerializeField, ConditionalField(nameof(_advanced))] private string _fieldName;
    [SerializeField, ConditionalField(nameof(_advanced))] private float _valueDif;
    [SerializeField, ConditionalField(nameof(_advanced))] private float _satDif;

    private void Start()
    {
        ApplyColor(_mixColor);
    }

    [ButtonMethod]
    public void ApplyColor(Color tintColor)
    {
        if (!Application.isPlaying) return;
        var renderer = GetComponent<Renderer>();

        _mixColor = tintColor;

        var tintH = 0f;
        if (_advanced) {
            Color.RGBToHSV(tintColor, out tintH, out var tintS, out var tintV);
        }

        foreach (var mat in renderer.materials) {
            var found = false;
            foreach (var m in _exceptions) {
                if (mat.name.Contains(m.name)) {
                    found = true;
                    break;
                }
            }
            if (found) continue;

            if (_advanced) {
                var color = mat.GetColor(_fieldName);

                Color.RGBToHSV(color, out var h, out var s, out var v);
                color = Color.HSVToRGB(tintH, s + _satDif, v + _valueDif);

                mat.SetColor(_fieldName, color);
            }
            else {
                var color = mat.color;
                color = Color.Lerp(color, tintColor, _lerpAmount);
                mat.color = color;
            }
        }
    }
}
