using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPosition : MonoBehaviour
{
    [SerializeField, ReadOnly] private Vector3 position;

    private void Update()
    {
        position = transform.position;
    }
}
