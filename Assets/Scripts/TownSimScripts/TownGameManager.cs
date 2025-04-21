using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;


public class TownGameManager : MonoBehaviour
{
    public static TownGameManager i;

    private void Awake()
    {
        i = this;
    }

    [Header("Character Manager")]

    public CharacterManager characterManager;

    public GameObject houseGrid;
    public GameObject houseButtonPrefab;

    public GameObject houseMenuUI;
    public GameObject houseMenuPrefab;

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

    void Start()
    {
        currency = PlayerPrefs.GetFloat("PlayerCurrency", 100);
        ChangeCurrency(0);

        LoadInventory();

        GenerateProblem(null);

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
    public async void ChangeScene(GameObject newSceneUI)
    {
        //fades out track and fades in load screen

        TownMusicPlayer i = TownMusicPlayer.i;
        if(i.currentTrack.TrackName != newSceneUI.GetComponent<Area>().associatedTrack.TrackName )
        {
            i.StartCoroutine(i.FadeTrackOut(i.currentTrack));
        }

        await FadeScreen(true);

        //iterates over all UIs, disabling. then, enables selected UI

        foreach (GameObject j in sceneUIList)
        {
            if(newSceneUI != j)
            {
                j.SetActive(false);
            }
            
        }

        newSceneUI.SetActive(true);

        //delete later
        MakeCharacterHouses();

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
    private void MakeCharacterHouses()
    {
        //get rid of old house list, then make new one

        foreach (Transform child in houseGrid.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in houseMenuUI.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (CharacterData character in characterManager.allCharacters)
        {
            //make their house dawg

            GameObject newHouseButton = Instantiate(houseButtonPrefab, houseGrid.transform);

            CharacterHouseButton newHouseButtonScript = newHouseButton.GetComponent<CharacterHouseButton>();
            //set parent, label, and sprite
            newHouseButtonScript.SetHouseLabel(character.characterName);
            newHouseButtonScript.SetHouseSprite(character.characterIcon);
            
            newHouseButtonScript.problemAlert.SetActive(character.hasProblem);
            
            
            newHouseButton.GetComponent<Button>().onClick.AddListener(() => OpenHouse(character));


            // make dictionary for houses and buttons maybe

            GameObject newHouse = Instantiate(houseMenuPrefab, houseMenuUI.transform);
            newHouse.SetActive(false);

            CharacterHouse newHouseScript = newHouse.GetComponent<CharacterHouse>();

            //now they reference each other yay

            newHouseScript.SetHouseCharacter(character);
            character.house = newHouse;

            //sets back button
            newHouse.GetComponentInChildren<NavigationButton>().newSceneUI = neighborhoodUI;
            newHouse.GetComponentInChildren<NavigationButton>().gameManager = this;

        }

    }

    private void OpenHouse(CharacterData character)
    {
        Debug.Log(character.characterName);

        character.house.gameObject.SetActive(true);
    }



    public void GenerateProblem(CharacterData lastCharacter)
    {
        CharacterData newProblemCharacter = characterManager.allCharacters[UnityEngine.Random.Range(0, characterManager.allCharacters.Count)];

        if(newProblemCharacter != lastCharacter)
        {

            newProblemCharacter.hasProblem = true;
            newProblemCharacter.currentProblem = allProblems[UnityEngine.Random.Range(0, allProblems.Count)];

            //Debug.Log("New Problem Character: " + newProblemCharacter.characterName + "Problem:" + newProblemCharacter.currentProblem);


        }
        else
        {
            GenerateProblem(lastCharacter);
        }
    }


}
