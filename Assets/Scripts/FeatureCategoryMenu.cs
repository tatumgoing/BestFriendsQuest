using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureCategoryMenu : MonoBehaviour
{
    [SerializeField] private GameObject _tabs;
    [SerializeField] private FaceMenu _faceMenuController;
    [SerializeField] private Animator _animator;
    [SerializeField] private List<SelectableItem> _submenuButtons = new List<SelectableItem>();
    [SerializeField] private SelectableItem _faceCategoryButton;

    private void OnEnable()
    {
        _tabs.SetActive(false);
    }

    public void SelectCategory(int category)
    {
        _faceCategoryButton.Deselect();
        _submenuButtons[category].Select(true, false);

        _tabs.SetActive(true);
        _animator.SetTrigger("Exit");

        _faceMenuController.OpenCategory((FeatureCategory) category);
    }
}
