using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreatorProgression : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private CharacterMetaController _characterController;
    [SerializeField] private GameObject _titleOptions;
    [SerializeField] private GameObject _facialOptions;
    [SerializeField] private GameObject _bodyOptions;

    private void Start()
    {
        HideAll();
        _characterController.gameObject.SetActive(false);

        _titleOptions.SetActive(true);
    }

    public void MakeNewCharacter()
    {
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

    private void HideAll()
    {
        _facialOptions.SetActive(false);
        _bodyOptions.SetActive(false);
    }
}
