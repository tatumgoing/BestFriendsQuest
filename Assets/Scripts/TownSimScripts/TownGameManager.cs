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
    
    public RecordsManager recordsManager;

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
        //iterates over all environments and UIs, disabling. then, enables selected environment and UI
        foreach (GameObject i in sceneList)
        {
            i.SetActive(false);
            ViewRecords();
            

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

    public void ViewRecords()
    {
        recordsManager.ClearRecords();

        foreach (var i in items)
        {
             recordsManager.CreateRecordItem(i.Key.Name, i.Value);
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
            GameObject newHouse = Instantiate(houseButtonPrefab, houseGrid.transform);
            newHouse.GetComponent<Button>().onClick.AddListener(() => OpenHouse(character));
            newHouse.GetComponentInChildren<TMP_Text>().text = character.characterName;

            newHouse.GetComponentInChildren<Image>().sprite= character.characterIcon;

            GameObject newHouseMenu = Instantiate(houseMenuPrefab, houseMenuUI.transform);
            newHouseMenu.SetActive(false);
            character.house = newHouseMenu;

            //sets back button
            newHouseMenu.GetComponentInChildren<NavigationButton>().newScene = neighborhood;
            newHouseMenu.GetComponentInChildren<NavigationButton>().newSceneUI = neighborhoodUI;
            newHouseMenu.GetComponentInChildren<NavigationButton>().gameManager = this;



        }

    }

    private void OpenHouse(CharacterData character)
    {
        Debug.Log(character.characterName);

        character.house.gameObject.SetActive(true);
    }



}
