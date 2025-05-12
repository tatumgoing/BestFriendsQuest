using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreatorProgression : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private CharacterMetaController _characterController;
    [SerializeField] private SelectableItem _facialFeaturesTabButton;
    [SerializeField] private GameObject _titleOptions;
    [SerializeField] private GameObject _facialOptions;
    [SerializeField] private GameObject _bodyOptions;
    [SerializeField] private GameObject _profileOptions;

    private void Start()
    {
        HideAll();
        _characterController.gameObject.SetActive(false);

        _titleOptions.SetActive(true);
    }

    public void StartNew()
    {
        _facialFeaturesTabButton.Select(true);
        FocusFace();
    }

    public void FocusFace()
    {
        HideAll();
        _titleOptions.SetActive(false);

        _facialOptions.SetActive(true);
        _characterController.gameObject.SetActive(true);
        _cameraController.SetHeadDistance();
    }

    public void FocusBody()
    {
        HideAll();
        _bodyOptions.SetActive(true);
        _cameraController.SetBodyDistance();
    }

    public void FocusProfile()
    {
        HideAll();
        _profileOptions.SetActive(true);
    }

    private void HideAll()
    {
        _facialOptions.SetActive(false);
        _bodyOptions.SetActive(false);
        _profileOptions.SetActive(false);
    }
}
