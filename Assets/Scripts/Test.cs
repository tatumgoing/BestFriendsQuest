using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] private Image _waterImage;
    [SerializeField] private float _speed;

    private void Update()
    {
        _waterImage.material.mainTextureOffset += new Vector2(_speed, 0) * Time.deltaTime;
    }
}
