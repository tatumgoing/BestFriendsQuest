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
    private void Awake() { i = this; }

    [SerializeField] private bool _demoMode;
    [SerializeField] private GameMode _mode;
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] Fade _fade;
    [SerializeField] MusicPlayer _music;
    public Transform Camera;
    [SerializeField] private GameObject _tutorial;
    [SerializeField] private GameObject _editExistingButtonParent;

    [Header("saving")]
    [SerializeField] private CharacterMetaController _character;
    [SerializeField] private string _saveFileName = "characters.txt";
 
    public bool Advanced => _mode == GameMode.ADVANCED;
    [HideInInspector] public UnityEvent OnModeChange;
    private List<ModeExlusiveItem> _modeExlusiveItems = new List<ModeExlusiveItem>();

    private string UndoStateString; //WIP

    public bool DemoMode => _demoMode;

    public string CharactersSavePath => System.IO.Path.Combine(Application.streamingAssetsPath, _saveFileName);

    private void Start()
    {
        _fade.Disappear();

        _modeExlusiveItems = FindObjectsByType<ModeExlusiveItem>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        UpdateMode();

        _tutorial.SetActive(_demoMode);
        _editExistingButtonParent.SetActive(!_demoMode);
    }

    [ButtonMethod]
    public void DeleteAllCharacters()
    {
        var file = File.CreateText(CharactersSavePath);
        file.Write("");
        file.Close();
    }

    private void UpdateMode()
    {
        _modeExlusiveItems.ForEach(item => item.UpdateMode(_mode));
        OnModeChange.Invoke();
    }

    private void Update()
    {
        if (InputController.GetDown(Control.PAUSE)) TogglePause();

        //UNDO WIP
        if (Input.GetMouseButtonUp(0)) SaveUndoState();
        if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftControl)) Undo();
    }

    private void SaveUndoState() //WIP
    {

    }

    public void Undo() //WIP
    {
        print("I WANT TO UNDO");
    }

    public string SaveCurrent()
    {
        var newData = _character.GetSaveString();

        if (!Advanced) return newData;

        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(CharactersSavePath));
        if (!File.Exists(CharactersSavePath)) {
            File.WriteAllText(CharactersSavePath, newData);
            return newData;
        }
        
        var characters = File.ReadAllText(CharactersSavePath).Split("\n").ToList();
        bool found = false;
        for (int i = 0; i < characters.Count; i++) {
            
            if (characters[i].Length > 1 && characters[i][..SaveSystem.IDLength] == _character.ID) {
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

        return newData;
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
