using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnEnable()
    {
        print(gameObject.name + "ENABLED");
    }

    private void OnDisable()
    {
        print(gameObject.name + "Disabled");
    }
}
