using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainHairOption : MonoBehaviour
{
    [SerializeField] private Image _image;

    private FeatureSOData _feature;
    private MainHairController _controller;

    public void Initialize(FeatureSOData feature, MainHairController controller)
    {
        _feature = feature;
        _image.sprite = feature.Icon;
        _controller = controller;
    }

    public void Select()
    {
        _controller.Select(_feature, this);
    }

}
