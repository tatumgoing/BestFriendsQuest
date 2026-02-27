using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Linq;
using MyBox;
using Unity.VisualScripting;

public enum AreaName {MAP, PARK, TOWN, SHOP, RESTURAUNT, TOWN_HALL, PORT }

[System.Serializable]
public class AreaData
{
    [HideInInspector] public string DisplayName;
    [HideInInspector] public AreaName Type;

    public GameObject UI;
    public GameObject Environment;

    public void Show() => SetActiveState(true);
    public void Hide() => SetActiveState(false);

    public void SetActiveState(bool active)
    {
        if (UI) UI.SetActive(active);
        if (Environment) Environment.SetActive(active);
    }
}

public class TownGameManager : MonoBehaviour
{
    public static TownGameManager i;

    [SerializeField] private List<AreaData> _areas = new List<AreaData>();

    [SerializeField] private bool _demoMode;

    public void GoToMap() => ChangeArea(AreaName.MAP);
    public void GoToPark() => ChangeArea(AreaName.PARK);
    public void GoToTown() => ChangeArea(AreaName.TOWN);
    public void GoToShop() => ChangeArea(AreaName.SHOP);
    public void GoToResturaunt() => ChangeArea(AreaName.RESTURAUNT);
    public void GoToTownHall() => ChangeArea(AreaName.TOWN_HALL);
    public void GoToPort() => ChangeArea(AreaName.PORT);

    private void OnValidate()
    {
        var options = Utils.EnumToList<AreaName>();
        for (int i = 0; i < options.Count; i++) {
            if (i < _areas.Count) {
                _areas[i].Type = options[i];
                _areas[i].DisplayName = options[i].ToString();
            }
        }
    }

    private void Awake()
    {
        i = this;

        //load items from Resources
        foreach (Item item in Resources.LoadAll("Items", typeof(Item))) {
            allItems.Add(item);
        }
    }

    void Start()
    {
        currency = PlayerPrefs.GetFloat("PlayerCurrency", 100);
        ChangeCurrency(0);

        LoadInventory();

        GenerateProblem(new ID());

        //bad liine of temp code
        ChangeScene(sceneUIList[sceneUIList.Count - 1], true);

        //MakeCharacterHouses();
    }

    public async void ChangeArea(AreaName targetArea)
    {
        await FadeScreen(true);

        if (_demoMode) {
            targetArea = AreaName.PARK;
            sceneUIList[^1].gameObject.SetActive(false);
        }
        foreach (var a in _areas) a.SetActiveState(a.Type == targetArea);

        await FadeScreen(false);        
    }

    public async void ChangeScene(GameObject newSceneUI, bool firstLaunch = false)
    {
        if (_demoMode && !newSceneUI.name.ContainsInsensitive("title")) {
            GoToPark();
            return;
        }

        //fades out track
        var musicPlayer = TownMusicPlayer.i;
        if (newSceneUI && musicPlayer.currentTrack != null && musicPlayer.currentTrack.TrackName != newSceneUI.GetComponent<Area>().associatedTrack.TrackName) {
            musicPlayer.StartCoroutine(musicPlayer.FadeTrackOut(musicPlayer.currentTrack));
        }

        //fades in load screen
        if (!firstLaunch) await FadeScreen(true);

        //enable the correct UI object
        foreach (var s in sceneUIList) {
            if (s) s.SetActive(newSceneUI == s);
        }

        await FadeScreen(false);
    }

    [Header(":::::::::")]
    [SerializeField] private CharacterManager _characterManager;

    [Header ("Inventory")]
    public float currency;
    
    public List<RecordsManager> recordsManagers = new List<RecordsManager>();

    [SerializeField]private List<Item> allItems = new List<Item> ();
    
    public List<string> itemNames= new List<string> (); 
    public List<int> itemCounts = new List<int> ();
    public Dictionary<Item, int> items = new Dictionary<Item, int> ();

    [Header("Problems")]

    public List<Problem> allProblems = new List<Problem> ();
    //public CharacterData problemCharacter;

    [Header ("UI Lists")]

    public List<GameObject> sceneList = new List<GameObject>();
    public List<GameObject> sceneUIList = new List<GameObject>();

    [SerializeField] private GameObject _townMapUI;
    public GameObject neighborhoodUI;
    public GameObject minigameUI;
    //public GameObject neighborhood;

    public GameObject fadeScreen;


    public async Task FadeScreen(bool fadeIn)
    {
        if (fadeIn) 
        {
            fadeScreen.SetActive(true);

            var opacity = fadeScreen.GetComponent<Image>();
            float step = 0;
            while (opacity.color.a < 1)
            {
                var tempOpacity = opacity.color;
                tempOpacity.a = step;
                opacity.color = tempOpacity;
                step += 50f * Time.deltaTime;


                await Task.Delay(Mathf.FloorToInt(10000 * Time.deltaTime));

            }
        }
        else
        {
            await Task.Delay(500);

            var opacity = fadeScreen.GetComponent<Image>();
            float step = 1;
            while (opacity.color.a > 0)
            {
                var tempOpacity = opacity.color;
                tempOpacity.a = step;
                opacity.color = tempOpacity;

                step -= 50f * Time.deltaTime;

                await Task.Delay(Mathf.FloorToInt(10000 * Time.deltaTime));

            }

            fadeScreen.SetActive(false);

        }
    }

    

    public void ChangeCurrency(float curChange)
    {
        currency += curChange;

        /*foreach (TMP_Text i in currencyDisplays)
        {
            i.text =  "$" + currency.ToString("F2");
        }*/

        PlayerPrefs.SetFloat("PlayerCurrency", currency);

    }

    public void AddInventory(Item newItem)
    {
        if (items.ContainsKey(newItem))
        {
            items[newItem] += 1;
        }
        else{
            items.Add(newItem, 1);
        }

        //items.Add(inventoryName);

        UpdateInventoryInspector();

        SaveCurrentInventory();
    }

    public void SubtractInventory(Item newItem)
    {
        if (items.ContainsKey(newItem))
        {
            items[newItem] -= 1;
        }
   
        UpdateInventoryInspector();

        SaveCurrentInventory();
    }

    private void UpdateInventoryInspector()
    {
        itemNames.Clear();

        foreach (var i in items.Keys)
        {
            itemNames.Add(i.ToString());
        }

        itemCounts.Clear();

        foreach (var j in items.Values)
        {
            itemCounts.Add(j);
        }
    }

    private void SaveCurrentInventory()
    {
        string inventory= "";
        foreach (var i in items.Keys)
        {
            inventory += i.Name + "," + items[i] + ":";
        }

        PlayerPrefs.SetString("Inventory", inventory);
        Debug.Log(inventory);
    }

    private void LoadInventory()
    {
        if (!PlayerPrefs.HasKey("Inventory"))return;

        string inventory = PlayerPrefs.GetString("Inventory");

        var inventoryList = inventory.Split(':');
        items.Clear();


        foreach (var i in inventoryList) { 
        
            var splitString= i.Split(',');
            if(splitString.Length == 2)
            {
                Item newItem = GetItemFromName(splitString[0]);
                items.Add(newItem, int.Parse(splitString[1]));

            }
        }

        UpdateInventoryInspector();
    }


    private Item GetItemFromName(string coolItem)
    {
        foreach (var item in allItems)
        {
            if (item.Name.Equals(coolItem))
            {
                return item;
            }
        }

        Debug.LogError(coolItem + " doesn't exist, what the hell?");
        return null;
        
    }


    public void GiveMoney()
    {
        ChangeCurrency(100);
    }

    public void UpdateRecordDisplay(RecordsManager rManager, ItemType type)
    {
       
        rManager.ClearRecords();

        foreach (Item i in allItems)
        {
            if (i.Type == type)
            {
                //if held
                if (items.ContainsKey(i) && items[i] != 0 && i.unlocked)
                {
                    rManager.CreateHeldItem(i, items[i], i.Cost);
                }
                //if previously held, but count = 0
                else if (i.unlocked)
                {
                    rManager.CreateUnheldItem(i, 0, i.Cost);
                }                
                // if never held
                else
                {
                    rManager.CreateLockedItem(i);
                }

            }
        
        }

        rManager.UpdateRecordSync();

    }

    private void OpenHouse(CompleteCharacterData character)
    {
        // disable the navigation UI, set active the house game object
        character.RoomScript.Show(character.ID);
    }

    public void GenerateProblem(ID prevID)
    {
        var validOptions = _characterManager.AllCharacters.Where(x => x.ID != prevID).ToList();

        var selectedCharacter = validOptions[Random.Range(0, validOptions.Count())];
        var selectedProblem = allProblems[Random.Range(0, allProblems.Count)];

        CharacterManager.i.AssignProblem(selectedCharacter.ID, selectedProblem);
    }
}
