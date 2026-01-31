using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum GameMode { SIMPLE, ADVANCED}

[RequireComponent(typeof(InputController))]
public class GameManager : MonoBehaviour
{
    public static GameManager i;
    public const int idLength = 4;
    private void Awake() { i = this; }

    [SerializeField] private GameMode _mode;
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] Fade _fade;
    [SerializeField] MusicPlayer _music;
    public Transform Camera;

    [Header("saving")]
    [SerializeField] private CharacterMetaController _character;
    [SerializeField] private string _saveFileName = "characters.txt";
 
    public bool Advanced => _mode == GameMode.ADVANCED;
    [HideInInspector] public UnityEvent OnModeChange;
    private List<ModeExlusiveItem> _modeExlusiveItems = new List<ModeExlusiveItem>();

    public string CharactersSavePath => System.IO.Path.Combine(Application.streamingAssetsPath, _saveFileName);

    private void Start()
    {
        _fade.Disappear();

        _modeExlusiveItems = FindObjectsByType<ModeExlusiveItem>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        UpdateMode();
    }

    private void UpdateMode()
    {
        _modeExlusiveItems.ForEach(item => item.UpdateMode(_mode));
        OnModeChange.Invoke();
    }

    private void Update()
    {
        if (InputController.GetDown(Control.PAUSE)) TogglePause();
    }

    public void SaveCurrent()
    {
        var newData = _character.GetSaveString();

        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(CharactersSavePath));
        if (!File.Exists(CharactersSavePath)) {
            File.WriteAllText(CharactersSavePath, newData);
            return;
        }
        
        var characters = File.ReadAllText(CharactersSavePath).Split("\n").ToList();
        bool found = false;
        for (int i = 0; i < characters.Count; i++) {
            
            if (characters[i].Length > 0 && characters[i][..idLength] == _character.ID) {
                //print("FOUND ID MATCHED");
                characters[i] = newData;
                found = true;
                break;
            }
        }
        if (!found) {
            //print("target ID not found, adding new character");
            characters.Add(newData);
        }

        File.WriteAllText(CharactersSavePath, string.Join("\n", characters));
        //print("saved sucessfully to: " + Path);
    }

    public void LoadCharacterByID()
    {

    }

    public void LoadFromSave()
    {
        if (!File.Exists(CharactersSavePath)) return;

        var saveString = File.ReadAllText(CharactersSavePath);
        _character.LoadFromString(saveString);
    }

    void TogglePause()
    {
        if (Time.timeScale == 0) Resume();
        else Pause();
    }

    public void Resume()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
        AudioManager.i.Resume();
    }

    public void Pause()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0;
        AudioManager.i.Pause();
    }

    [ButtonMethod]
    public void LoadMenu()
    {
        Resume();
        StartCoroutine(FadeThenLoadScene(0));
    }

    [ButtonMethod]
    public void EndGame()
    {
        Resume();
        StartCoroutine(FadeThenLoadScene(2));
    }

    IEnumerator FadeThenLoadScene(int num)
    {
        _fade.Appear(); 
        _music.FadeOutCurrent(_fade.FadeTime);
        yield return new WaitForSeconds(_fade.FadeTime + 0.5f);
        Destroy(AudioManager.i.gameObject);
        SceneManager.LoadScene(num);
    }

}
