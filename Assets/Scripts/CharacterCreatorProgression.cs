using MyBox;
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
    [SerializeField] private GameObject _faceContinueButtonParent;
    [SerializeField] private SelectableItem _earTabButton;
    [SerializeField] private Animator _titleOptions;
    [SerializeField] private GameObject _facialOptions;
    [SerializeField] private GameObject _bodyOptions;
    [SerializeField] private GameObject _profileOptions;
    [SerializeField] private SelectableItem _skinColorButton;
    [SerializeField] private SelectableItem _dataButton;
    [SerializeField] private MainHairController _hairController;
    [SerializeField] private ExpressionButtonsController _expressionButtons;
    [SerializeField] private BodyCustomizer _bodyCustomizer;
    [SerializeField] private AnalyticsTracker _analyticsTracker;
    [SerializeField] private TextAsset _randomNames;
    [SerializeField] private TextAsset _randomLastNames;
    [SerializeField] private CharacterMetaController _character;

    [SerializeField] private ColorMenuController _faceColorMenu;
    [SerializeField] private ColorMenuController _skinColorMenu;

    private string[] _names;
    private string[] _lastNames;

    private void Start()
    {
        HideAll(false);
        _characterController.gameObject.SetActive(false);
        _titleOptions.gameObject.SetActive(true);

        _names = _randomNames.text.Split("\n");
        _lastNames = _randomLastNames.text.Split("\n");
    }

    private void Update()
    {
        _faceContinueButtonParent.SetActive(_earTabButton.Selected);
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
        var eyeObj = face.AddFeature(eyes);
        face.Select(eyeObj);
        var eyeColor = Random.ColorHSV(0, 1, 0.6f, 1, 0.2f, 1f);
        _faceColorMenu.SetColor(eyeColor);
        face.SetCurrentColor(eyeColor);

        face.SetCategory(FeatureCategory.EYEBROWS);
        var eyeBrowsObj = face.AddFeature(eyebrows);
        face.Select(eyeBrowsObj);
        _faceColorMenu.SetColor(hairColor);
        face.SetCurrentColor(hairColor);

        face.SetCategory(FeatureCategory.NOSE);
        var noseObj = face.AddFeature(nose);
        face.Select(noseObj);
        _faceColorMenu.SetColor(skinColor);
        face.SetCurrentColor(skinColor);

        face.SetCategory(FeatureCategory.MOUTH);
        var mouthObj = face.AddFeature(mouth);
        face.Select(mouthObj);
        var mouthColor = Utils.DarkerMoreSaturatedMoreRed(skinColor);
        _faceColorMenu.SetColor(mouthColor);
        face.SetCurrentColor(mouthColor);

        var allHairs = Resources.LoadAll<FeatureSOData>("HairFeatures").OrderByDescending(x => x.Priority).Where(x => x.IsMainHair).ToList();
        var selectedHair = allHairs.GetRandom();
        FindObjectOfType<HairController>().AddFeature(selectedHair);
        FindObjectOfType<HairController>().SetHairColor(hairColor);


        _bodyCustomizer.MoveHeightSlider(Random.Range(0, 1f));
        _bodyCustomizer.MoveWeightSlider(Random.Range(0, 1f));
        _bodyCustomizer.MoveArmsSlider(Random.Range(0, 1f));
        _bodyCustomizer.MoveWaistSlider(Random.Range(0, 1f));
        _bodyCustomizer.MoveTorsoSlider(Random.Range(0, 1f));
        _bodyCustomizer.MoveLegsSlider(Random.Range(0, 1f));

        var saveString = _character.GetSaveString();
        FindObjectOfType<MainHairController>(true).SetHair(saveString);

        _skinColorButton.Select(true);

        var dataController = FindObjectOfType<DataPanelController>(true);
        var name = _names[Random.Range(0, _names.Length)] + " " + _lastNames[Random.Range(0, _lastNames.Length)];
        dataController.SetData(ProfileDataType.NAME, name);
        dataController.SetData(ProfileDataType.COLOR, Utils.EnumToList<FavoriteColor>().GetRandom().ToString());

        var gender = Utils.EnumToList<Gender>().GetRandom();
        dataController.SetData(ProfileDataType.GENDER, gender.ToString());
        if (gender == Gender.MALE) dataController.SetData(ProfileDataType.PRONOUN, Pronoun.HE.ToString());
        else if (gender == Gender.FEMALE) dataController.SetData(ProfileDataType.PRONOUN, Pronoun.THEY.ToString());
        else if (gender == Gender.NONBINARY) dataController.SetData(ProfileDataType.PRONOUN, Pronoun.SHE.ToString());

        dataController.SetData(ProfileDataType.DAY, Random.Range(1, 28).ToString());
        dataController.SetData(ProfileDataType.MONTH, Random.Range(1, 12).ToString());
        var currentYear = System.DateTime.Now.Year;
        dataController.SetData(ProfileDataType.YEAR, Random.Range(currentYear - 45, currentYear).ToString());

        dataController.SetData(ProfileDataType.ATTRACTION, Utils.EnumToList<Attraction>().GetRandom().ToString());
        _dataButton.Select();
        dataController.ReloadCurrent();
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
