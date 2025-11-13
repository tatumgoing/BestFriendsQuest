using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureCategoryMenu : MonoBehaviour
{
    [SerializeField] private GameObject _tabs;
    [SerializeField] private FaceMenu _faceMenuController;
    [SerializeField] private Animator _animator;

    private void OnEnable()
    {
        _tabs.SetActive(false);
    }

    public void SelectCategory(int category)
    {
        _tabs.SetActive(true);
        _animator.SetTrigger("Exit");

        _faceMenuController.OpenCategory((FeatureCategory) category);
    }
}
