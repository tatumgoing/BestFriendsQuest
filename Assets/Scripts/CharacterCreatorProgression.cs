using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private ExpressionButtonsController _expressionButtons;
    [SerializeField] private AnalyticsTracker _analyticsTracker;

    [SerializeField] private ColorMenuController _faceColorMenu;

    private void Start()
    {
        HideAll(false);
        _characterController.gameObject.SetActive(false);
        _titleOptions.gameObject.SetActive(true);
    }

    public void Randomize()
    {
        StartNew();

        var allFeatures = Resources.LoadAll<FeatureSOData>("FacialFeatures").OrderByDescending(x => x.Priority).ToList();
        
        var eyes = allFeatures.Where(x => x.SubType == FeatureSubType.EYES).GetRandom();
        var eyebrows = allFeatures.Where(x => x.SubType == FeatureSubType.BROWS).GetRandom();
        var nose = allFeatures.Where(x => x.SubType == FeatureSubType.NOSE).GetRandom();
        var mouth = allFeatures.Where(x => x.SubType == FeatureSubType.LIPS).GetRandom();

        var face = FindObjectOfType<FaceFeatureController>();
        face.Reset();

        face.SetCategory(FeatureCategory.EYES);
        face.AddFeature(eyes);
        _faceColorMenu.SetColor(eyes.DefaultSettings.Color);
        face.SetCurrentColor(eyes.DefaultSettings.Color);
        print("eye default color: " + eyes.DefaultSettings.Color);

        face.SetCategory(FeatureCategory.EYEBROWS);
        face.AddFeature(eyebrows);
        _faceColorMenu.SetColor(eyebrows.DefaultSettings.Color);
        face.SetCurrentColor(eyebrows.DefaultSettings.Color);
        print("eyebrows default color: " + eyebrows.DefaultSettings.Color);

        face.SetCategory(FeatureCategory.NOSE);
        face.AddFeature(nose);
        _faceColorMenu.SetColor(nose.DefaultSettings.Color);
        face.SetCurrentColor(nose.DefaultSettings.Color);
        print("nose default color: " + nose.DefaultSettings.Color);

        face.SetCategory(FeatureCategory.MOUTH);
        face.AddFeature(mouth);
        _faceColorMenu.SetColor(mouth.DefaultSettings.Color);
        face.SetCurrentColor(mouth.DefaultSettings.Color);
        print("mouth default color: " + mouth.DefaultSettings.Color);
    }

    public async void StartNew()
    {
        _characterController.MakeNewID();
        _facialFeaturesTabButton.Select(true);
        FocusFace();
        _expressionButtons.Show();

        _analyticsTracker.StartNew();

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
        var saveString = GameManager.i.SaveCurrent();
        _analyticsTracker.FinishCharacter(saveString);

        UIManager.i.Fade.Appear();

        var fadeTime = Mathf.RoundToInt(UIManager.i.Fade.FadeTime * 1000);
        await Task.Delay(fadeTime);

        if (GameManager.i.DemoMode) SceneManager.LoadScene(4);
        else SceneManager.LoadScene(2);
    }

    private void HideAll(bool hideTitle = true)
    {
        if (hideTitle && _titleOptions.gameObject.activeInHierarchy) _titleOptions.SetTrigger("Exit");
        _facialOptions.SetActive(false);
        _bodyOptions.SetActive(false);
        _profileOptions.SetActive(false);
    }
}
