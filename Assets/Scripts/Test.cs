using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using TMPro;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Start()
    {
        _animator.SetBool("Walking", true);
    }
}
