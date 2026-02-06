using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Linq;

public class TownGameManager : MonoBehaviour
{
    public static TownGameManager i;

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

    public GameObject neighborhoodUI;
    public GameObject minigameUI;
    //public GameObject neighborhood;

    public GameObject fadeScreen;

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
        ChangeScene(sceneUIList[sceneUIList.Count -1], true);

        //MakeCharacterHouses();

    }

    async Task FadeScreen(bool fadeIn)
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

    public async void ChangeScene(GameObject newSceneUI, bool firstLaunch = false)
    {
        //fades out track and fades in load screen

        TownMusicPlayer i = TownMusicPlayer.i;
        if(newSceneUI && i.currentTrack != null && i.currentTrack.TrackName != newSceneUI.GetComponent<Area>().associatedTrack.TrackName )
        {
            i.StartCoroutine(i.FadeTrackOut(i.currentTrack));
        }

        if(!firstLaunch)
        {
            await FadeScreen(true);
        }

        //iterates over all UIs, disabling. then, enables selected UI

        foreach (GameObject j in sceneUIList)
        {
            if(newSceneUI != j)
            {
                if (j) j.SetActive(false);
            }
        }

        if (newSceneUI && newSceneUI != neighborhoodUI) newSceneUI.SetActive(true);

        await FadeScreen(false);

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
