using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainHairOption : MonoBehaviour
{
    [SerializeField] private Image _image;

    private Sprite _sprite;
    private MainHairController _controller;

    public void Initialize(Sprite sprite, MainHairController controller)
    {
        _sprite = sprite;
        _image.sprite = sprite;
        _controller = controller;
    }

    public void Select()
    {
        _controller.Select(_sprite, this);
    }

}
