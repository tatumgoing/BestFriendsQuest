using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUIController : MonoBehaviour
{
    [SerializeField] private GameObject _initialBacking;
    [SerializeField] private GameObject _closeButton;

    private void OnEnable()
    {
        _closeButton.SetActive(!_initialBacking.activeInHierarchy);
    }
}
