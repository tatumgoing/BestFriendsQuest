using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField] private Transform _lead;
    void Update()
    {
        transform.position = _lead.position;
    }
}
