using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class TownGameManager : MonoBehaviour
{
    public static TownGameManager i;

    [SerializeField] private List<AreaData> _areas = new List<AreaData>();

    [SerializeField] private int _characterCreatorSceneIndex = 1;
    [SerializeField] private bool _demoMode;
    [SerializeField] private NeighborhoodController _neighborhoodController;

    public List<ItemData> GetAllItems() => allItems;
    public ItemData GetItemByID(ID id) => allItems.Where(x => x.ID == id).FirstOrDefault();

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
        allItems = Resources.LoadAll<ItemData>("Items").ToList();

        var itemIDs = new Dictionary<ID, ItemData>();
        foreach (var i in allItems) {
            if (itemIDs.ContainsKey(i.ID)) {
                Debug.LogError("Duplicate ID found for item " + i.Name + " and " + itemIDs[i.ID].Name + ". This will cause problems with saving/loading inventory. Please regenerate the ID of one of these items.");
            }
            itemIDs[i.ID] = i;
        }

        LoadInventory();
    }

    void Start()
    {
        currency = PlayerPrefs.GetFloat("PlayerCurrency", 100);
        ChangeCurrency(0);

        //bad liine of temp code
        ChangeScene(sceneUIList[sceneUIList.Count - 1], true);
    }

    /// <summary>
    /// Returns a list of all items that the player has at least 1 of
    /// </summary>
    public List<ItemData> GetInventoryItems()
    {
        var items = allItems;
        return items.Where(x => this.items.ContainsKey(x) && this.items[x] > 0).ToList();
    } 

    /// <summary>
    /// Called from a minigameController when completing a problem-based minigame.
    /// returns to the room of the character and triggers the completion dialogue.
    /// </summary>
    public async void GoToRoom(ID id)
    {
        await ChangeArea(AreaName.TOWN);
        _neighborhoodController.ShowRoom(id);
    }

    /// <summary>
    /// Given an ID, goes to and starts the minigame to solve that character's problem
    /// warning: only call if certain that this character has a minigame-type problem
    /// </summary>
    public async void QuickStartMinigame(ID character)
    {
        var minigame = CharacterManager.i.GetProblem(character).Minigame;
        var selected = _areas.Where(x => x._minigameController?.GetMinigameType() == minigame).FirstOrDefault();
        if (selected == default) return;

        await ChangeArea(selected.Type);
        selected._minigameController.StartProblemMinigame(character);
    }

    public async Task ChangeArea(AreaName targetArea)
    {
        await FadeScreen(true);

        if (_demoMode) {
            targetArea = AreaName.PARK;
            sceneUIList[^1].gameObject.SetActive(false);
        }
        foreach (var a in _areas) a.SetActiveState(a.Type == targetArea);

        await FadeScreen(false);        
    }

    public void BuyItem(ItemData item)
    {
        if (item.Cost > currency) return;
        ChangeCurrency(-item.Cost);
        AddInventory(item);
    }

    public async void ChangeScene(GameObject newSceneUI, bool firstLaunch = false)
    {
        if (_demoMode && !newSceneUI.name.ContainsInsensitive("title")) {
            await ChangeArea(AreaName.PARK);
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

    public void LoadCharacterCreator()
    {
        SceneManager.LoadScene(_characterCreatorSceneIndex);
    }


    [Header(":::::::::")]
    [SerializeField] private CharacterManager _characterManager;

    [Header ("Inventory")]
    public float currency;
    
    public List<RecordsManager> recordsManagers = new List<RecordsManager>();

    [SerializeField] private List<ItemData> allItems = new List<ItemData> ();
    
    public List<string> itemNames= new List<string> (); 
    public List<int> itemCounts = new List<int> ();
    public Dictionary<ItemData, int> items = new Dictionary<ItemData, int> ();

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

    public void AddInventory(ItemData newItem)
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

    public void SubtractInventory(ItemData newItem)
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
        //Debug.Log(inventory);
    }

    private void LoadInventory()
    {
        if (!PlayerPrefs.HasKey("Inventory"))return;

        string inventory = PlayerPrefs.GetString("Inventory");

        var inventoryList = inventory.Split(':');
        items.Clear();


        foreach (var itemString in inventoryList) { 
            var parts = itemString.Split(',');
            if (parts.Length != 2) continue;
            
            var parsedItem = GetItemFromName(parts[0]);
            if (parsedItem == null) continue;

            ItemData newItem = parsedItem;
            items.Add(newItem, int.Parse(parts[1]));
        }

        UpdateInventoryInspector();
    }


    private ItemData GetItemFromName(string coolItem)
    {
        foreach (var item in allItems)
        {
            if (item.Name.Equals(coolItem))
            {
                return item;
            }
        }

        //Debug.LogError(coolItem + " doesn't exist, what the hell?");
        return null;
    }

    public void GiveMoney()
    {
        ChangeCurrency(100);
    }

    public void UpdateRecordDisplay(RecordsManager rManager, ItemType type)
    {
        rManager.ClearRecords();

        foreach (ItemData i in allItems)
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
}
