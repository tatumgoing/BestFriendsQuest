using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class TownGameManager : MonoBehaviour
{
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

    [Header ("UI Lists")]

    public List<GameObject> sceneList = new List<GameObject>();
    public List<GameObject> sceneUIList = new List<GameObject>();
    public List<TMP_Text> currencyDisplays = new List<TMP_Text>();

    public GameObject neighborhoodUI;
    public GameObject neighborhood;



    // Start is called before the first frame update
    void Start()
    {
        currency = PlayerPrefs.GetFloat("PlayerCurrency", 100);
        ChangeCurrency(0);

        LoadInventory();

        //MakeCharacterHouses();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene(GameObject newScene, GameObject newSceneUI)
    {
        //Debug.Log("Change Scene Going To: " + newScene + newSceneUI);

        //iterates over all environments and UIs, disabling. then, enables selected environment and UI
        foreach (GameObject i in sceneList)
        {
            i.SetActive(false);
            UpdateRecords();
            

        }
        foreach (GameObject j in sceneUIList)
        {
            j.SetActive(false);
        }

        newScene.SetActive(true);
        newSceneUI.SetActive(true);

        //delete later
        MakeCharacterHouses();


    }

    public void ChangeCurrency(float curChange)
    {
        currency += curChange;
        foreach (TMP_Text i in currencyDisplays)
        {
            i.text =  "$" + currency.ToString("F2");
        }

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

    public void UpdateRecords()
    {
        foreach (RecordsManager rManager in recordsManagers)
        {
            rManager.ClearRecords();

            foreach (Item i in allItems)
            {
                //if held
                if (items.ContainsKey(i) && items[i] != 0 && i.unlocked)
                {
                    rManager.CreateHeldItem(i.Name, items[i]);
                }
                //if previously held, but count = 0
                else if (i.unlocked)
                {
                    rManager.CreateUnheldItem(i.Name, 0);
                }
                else
                {
                    rManager.CreateLockedItem(i.Name);
                }

                // if never held

            }
        }

        
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
            
            
            newHouseButton.GetComponent<Button>().onClick.AddListener(() => OpenHouse(character));


            // make dictionary for houses and buttons maybe



            GameObject newHouse = Instantiate(houseMenuPrefab, houseMenuUI.transform);
            newHouse.SetActive(false);

            CharacterHouse newHouseScript = newHouse.GetComponent<CharacterHouse>();

            //now they reference each other yay

            newHouseScript.SetHouseCharacter(character);
            character.house = newHouse;

            //sets back button
            newHouse.GetComponentInChildren<NavigationButton>().newScene = neighborhood;
            newHouse.GetComponentInChildren<NavigationButton>().newSceneUI = neighborhoodUI;
            newHouse.GetComponentInChildren<NavigationButton>().gameManager = this;



        }

    }

    private void OpenHouse(CharacterData character)
    {
        Debug.Log(character.characterName);

        character.house.gameObject.SetActive(true);
    }



}
