using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainHairOption : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private bool _debug;
    [SerializeField] private TextMeshProUGUI _debugText;

    private FeatureSOData _feature;
    private MainHairController _controller;

    public FeatureSOData Feature => _feature;

    public void Initialize(FeatureSOData feature, MainHairController controller)
    {
        _feature = feature;
        _image.sprite = feature.Icon;
        _controller = controller;

#if UNITY_EDITOR
        if (_debug) _debugText.text = feature.name;
#else
        Destroy(_debugText);
#endif
    }

    public void Select()
    {
        _controller.Select(_feature, this);
    }

}
