using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterCreatorProgression : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private CharacterMetaController _characterController;
    [SerializeField] private SelectableItem _facialFeaturesTabButton;
    [SerializeField] private Animator _titleOptions;
    [SerializeField] private GameObject _facialOptions;
    [SerializeField] private GameObject _bodyOptions;
    [SerializeField] private GameObject _profileOptions;
    [SerializeField] private MainHairController _hairController;

    private void Start()
    {
        HideAll(false);
        _characterController.gameObject.SetActive(false);
        _titleOptions.gameObject.SetActive(true);
    }

    public async void StartNew()
    {
        _characterController.MakeNewID();
        _facialFeaturesTabButton.Select(true);
        FocusFace();

        await Task.Delay(100);
        _hairController.UpdateVisuals();
    }

    public void FocusFace()
    {
        HideAll();

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

    public async void FinishCharacter()
    {
        GameManager.i.SaveCurrent();


        UIManager.i.Fade.Appear();

        var fadeTime = Mathf.RoundToInt(UIManager.i.Fade.FadeTime * 1000);
        await Task.Delay(fadeTime);
        SceneManager.LoadScene(2);
    }

    private void HideAll(bool hideTitle = true)
    {
        if (hideTitle && _titleOptions.gameObject.activeInHierarchy) _titleOptions.SetTrigger("Exit");
        _facialOptions.SetActive(false);
        _bodyOptions.SetActive(false);
        _profileOptions.SetActive(false);
    }
}
