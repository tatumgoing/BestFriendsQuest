using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TownGameManager : MonoBehaviour
{
    public static TownGameManager i;

    [Header("GameModes")]
    [SerializeField] private bool _steamDemoMode;

    [Header("Non-Location Menus")]
    [SerializeField] private GameObject _titleScreen;
    [SerializeField] private GameObject _mapParent;
    [SerializeField] private GameObject _invParent;
    [SerializeField] private GameObject _recordsParent;
    [SerializeField] private List<ItemData> _allItems = new List<ItemData>();

    [Header("Locations")]
    [SerializeField] private List<AreaData> _areas = new List<AreaData>();
    [SerializeField] private NeighborhoodController _neighborhoodController;

    [Header("Misc")]
    [SerializeField] private int _characterCreatorSceneIndex = 1;
    [SerializeField] private GameObject _fadeScreen;
    [SerializeField, Min(0)] private float _currency;
    [SerializeField] private GameObject _mapStartBacking;
    [SerializeField] private pauseMenuController _pauseMenu;

    private Dictionary<ItemData, int> _inventory = new Dictionary<ItemData, int>();
    private List<RuntimeItemData> _runtimeItemData = new List<RuntimeItemData>();

    public Dictionary<ItemData, int> Inventory => _inventory;
    public float Currency => _currency;
    public ItemData GetItemByID(ID id) => _allItems.Where(x => x.ID == id).FirstOrDefault();
    public bool DemoMode => _steamDemoMode;

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
        _allItems = Resources.LoadAll<ItemData>("Items").ToList();

        var itemIDs = new Dictionary<ID, ItemData>();
        foreach (var i in _allItems) {
            if (itemIDs.ContainsKey(i.ID)) {
                Debug.LogError("Duplicate ID found for item " + i.Name + " and " + itemIDs[i.ID].Name + ". This will cause problems with saving/loading inventory. Please regenerate the ID of one of these items.");
            }
            itemIDs[i.ID] = i;

            _runtimeItemData.Add(new RuntimeItemData(i));
        }

        LoadInventory();
    }

    void Start()
    {
        if (_steamDemoMode) print("Starting game in Steam Demo Mode");
        else print("Starting game in full mode");

        Utils.SetMenus(1);

        _currency = PlayerPrefs.GetFloat("PlayerCurrency", 100);
        ChangeCurrency(0);

        foreach (var a in _areas) a.SetActiveState(false);
        _titleScreen.SetActive(true);
    }

    public void ResetInventory()
    {
        _inventory = new Dictionary<ItemData, int>();
        SaveCurrentInventory();
    }

    public void SetCurrency(int currency)
    {
        _currency = currency;
        ChangeCurrency(0);
    }

    public void ShowInitialMap()
    {
        if (_steamDemoMode && PlayerPrefs.GetInt("DemoStep", 0) < 3) return;

        CharacterManager.i.LoadCurrentQuests();
        _mapStartBacking.SetActive(true);
        _mapParent.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) print("Menus: " + Utils.MenusOpen);

        if (Input.GetKeyDown(KeyCode.Escape) && Cursor.visible) {
            print("pause menu opened - GM: visible: " + Cursor.visible);
            if (_pauseMenu.gameObject.activeInHierarchy) _pauseMenu.gameObject.SetActive(false);
            else if (_invParent.activeInHierarchy) _invParent.SetActive(false);
            else if (_mapParent.activeInHierarchy) _mapParent.SetActive(false);
            else _pauseMenu.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.M) && !_invParent.activeInHierarchy) {
            if (_mapParent.activeInHierarchy) _mapParent.SetActive(false);
            else _ = ChangeArea(AreaName.MAP);
        }

        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.E)) {
            if (!_mapParent.activeInHierarchy) _ = ChangeArea(AreaName.MAP);

            if (!_invParent.activeInHierarchy) _invParent.SetActive(true);
            else _invParent.SetActive(false);
        }
    }

    public void UnlockItem(ItemData item)
    {
        for (int i = 0; i < _runtimeItemData.Count; i++) {
            if (_runtimeItemData[i].Item == item) _runtimeItemData[i].Unlocked = true;
        }
        //print("Unlocked: " + item);
    }

    public List<ItemData> GetAllItems(bool unlockedOnly)
    {
        if (!unlockedOnly) return _allItems;
        else return _runtimeItemData.Where(x => x.Unlocked).Select(x => x.Item).ToList();
    }

    public bool IsUnlocked(ItemData item)
    {
        return _runtimeItemData.Where(x => x.Item == item).First().Unlocked;
    }

    public bool IsAlreadyOwned(ItemData item)
    {
        return _runtimeItemData.Where(x => x.Item == item).First().AlreadyOwned;
    }

    public int GetNumberOwned(ItemData item)
    {
        if (!_inventory.ContainsKey(item)) return 0;
        return _inventory[item];
    }

    /// <summary>
    /// Returns a list of all items that the player has at least 1 of
    /// </summary>
    public List<ItemData> GetInventoryItems()
    {
        var items = _allItems;
        return items.Where(x => this._inventory.ContainsKey(x) && this._inventory[x] > 0).ToList();
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
        _mapStartBacking.SetActive(false);

        if (targetArea == AreaName.NONE) {
            foreach (var a in _areas) a.SetActiveState(false);
            return;
        }

        if (targetArea == AreaName.MAP) {
            CharacterManager.i.LoadCurrentQuests();
            foreach (var a in _areas) if (a.Type == AreaName.MAP) a.SetActiveState(true);
            return;
        }

        if (targetArea == AreaName.RECORDS) {
            foreach (var a in _areas) if (a.Type == AreaName.RECORDS) a.SetActiveState(true);
            return;
        }

        if (Application.isPlaying) await FadeScreen(true);
        foreach (var a in _areas) a.SetActiveState(a.Type == targetArea);
        if (Application.isPlaying) await FadeScreen(false);        
    }

    public void BuyItem(ItemData item)
    {
        if (item.Cost > _currency) return;
        ChangeCurrency(-item.Cost);
        AddInventory(item);
    }

    public void LoadCharacterCreator()
    {
        SceneManager.LoadScene(_characterCreatorSceneIndex);
    }

    public async Task FadeScreen(bool fadeIn)
    {
        if (fadeIn) 
        {
            _fadeScreen.SetActive(true);

            var opacity = _fadeScreen.GetComponent<Image>();
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

            var opacity = _fadeScreen.GetComponent<Image>();
            float step = 1;
            while (opacity.color.a > 0)
            {
                var tempOpacity = opacity.color;
                tempOpacity.a = step;
                opacity.color = tempOpacity;

                step -= 50f * Time.deltaTime;

                await Task.Delay(Mathf.FloorToInt(10000 * Time.deltaTime));

            }

            _fadeScreen.SetActive(false);

        }
    }    

    public void ChangeCurrency(float curChange)
    {
        _currency += curChange;
        PlayerPrefs.SetFloat("PlayerCurrency", _currency);
    }

    public void AddInventory(ItemData newItem)
    {
        if (_inventory.ContainsKey(newItem))
        {
            _inventory[newItem] += 1;
        }
        else{
            _inventory.Add(newItem, 1);
        }

        for (int i = 0; i < _runtimeItemData.Count; i++) {
            if (_runtimeItemData[i].Item == newItem) _runtimeItemData[i].AlreadyOwned = true;
        }

        SaveCurrentInventory();
    }

    public void SubtractInventory(ItemData newItem)
    {
        if (_inventory.ContainsKey(newItem))
        {
            _inventory[newItem] -= 1;
        }
        SaveCurrentInventory();
    }

    private void SaveCurrentInventory()
    {
        string inventory= "";
        foreach (var i in _inventory.Keys)
        {
            inventory += i.Name + "," + _inventory[i] + ":";
        }

        PlayerPrefs.SetString("Inventory", inventory);
        //Debug.Log(inventory);
    }

    private void LoadInventory()
    {
        if (!PlayerPrefs.HasKey("Inventory"))return;

        string inventory = PlayerPrefs.GetString("Inventory");

        var inventoryList = inventory.Split(':');
        _inventory.Clear();

        foreach (var itemString in inventoryList) { 
            var parts = itemString.Split(',');
            if (parts.Length != 2) continue;
            
            var parsedItem = GetItemFromName(parts[0]);
            if (parsedItem == null) continue;

            ItemData newItem = parsedItem;
            AddInventory(newItem);
            _inventory[newItem] = int.Parse(parts[1]);
        }

        SaveCurrentInventory();
    }


    private ItemData GetItemFromName(string coolItem)
    {
        foreach (var item in _allItems)
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

        foreach (ItemData i in _allItems)
        {
            if (i.Type == type)
            {
                //if held
                if (_inventory.ContainsKey(i) && _inventory[i] != 0 && i.StartUnlocked)
                {
                    rManager.CreateHeldItem(i, _inventory[i], i.Cost);
                }
                //if previously held, but count = 0
                else if (i.StartUnlocked)
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
