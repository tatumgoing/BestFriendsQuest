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
    [SerializeField] private ColorMenuController _skinColorMenu;

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

        var skinColor = _skinColorMenu.GetRandomBasicColor();
        _skinColorMenu.SetColor(skinColor);

        var hairColor = Random.ColorHSV(0, 1, 0.6f, 1, 0.1f, 0.75f);

        var face = FindObjectOfType<FaceFeatureController>();
        face.Reset();

        face.SetCategory(FeatureCategory.EYES);
        //print("eye default color: " + eyes.DefaultSettings.Color.ToHex());
        var eyeObj = face.AddFeature(eyes);
        face.Select(eyeObj);
        var eyeColor = Random.ColorHSV(0, 1, 0.6f, 1, 0.2f, 1f);
        _faceColorMenu.SetColor(eyeColor);
        face.SetCurrentColor(eyeColor);

        face.SetCategory(FeatureCategory.EYEBROWS);
        //print("eyebrows default color: " + eyebrows.DefaultSettings.Color.ToHex());
        var eyeBrowsObj = face.AddFeature(eyebrows);
        face.Select(eyeBrowsObj);
        _faceColorMenu.SetColor(hairColor);
        face.SetCurrentColor(hairColor);

        face.SetCategory(FeatureCategory.NOSE);
        //print("nose default color: " + nose.DefaultSettings.Color.ToHex());
        var noseObj = face.AddFeature(nose);
        face.Select(noseObj);
        _faceColorMenu.SetColor(skinColor);
        face.SetCurrentColor(skinColor);

        face.SetCategory(FeatureCategory.MOUTH);
        //print("mouth default color: " + mouth.DefaultSettings.Color.ToHex());
        var mouthObj = face.AddFeature(mouth);
        face.Select(mouthObj);
        var mouthColor = Utils.DarkerMoreSaturatedMoreRed(skinColor);
        _faceColorMenu.SetColor(mouthColor);
        face.SetCurrentColor(mouthColor);

        var allHairs = Resources.LoadAll<FeatureSOData>("HairFeatures").OrderByDescending(x => x.Priority).Where(x => x.IsMainHair).ToList();
        FindObjectOfType<HairController>().AddFeature(allHairs.GetRandom());
        FindObjectOfType<HairController>().SetHairColor(hairColor);

    }

    public void StartNew()
    {
        _characterController.MakeNewID();
        _facialFeaturesTabButton.Select(true);
        FocusFace();
        _expressionButtons.Show();

        _analyticsTracker.StartNew();

        _hairController.UpdateVisuals();

        _skinColorMenu.SetColor(_skinColorMenu.GetDefaultColor());
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
        if (GameManager.i.SendData) _analyticsTracker.FinishCharacter(saveString);

        UIManager.i.Fade.Appear();
        await Task.Delay(Mathf.RoundToInt(UIManager.i.Fade.FadeTime * 1000));

        if (GameManager.i.ResearchMode) {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            return;
        }

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
