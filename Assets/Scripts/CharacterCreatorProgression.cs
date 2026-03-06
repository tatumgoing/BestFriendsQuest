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
        var eyes = allFeatures.Where(x => x.SubType == FeatureSubType.EYES).ToList();

        var face = FindObjectOfType<FaceFeatureController>();
        face.Reset();
        face.AddFeature(eyes.GetRandom());
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
