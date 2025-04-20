using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    public List<ItemTabs> tabs = new List<ItemTabs>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (ItemTabs tab in tabs)
        {
            tab.GetComponent<Button>().onClick.AddListener(() => UpdateTab(tab));
        }

        UpdateTab(tabs[0]);
    }

    

    // Update is called once per frame
    void UpdateTab(ItemTabs clickedTab)
    {
        foreach(ItemTabs tab in tabs)
        {
            if (tab != clickedTab)
            {
                tab.selected = false;
            }
        }

        clickedTab.selected = true;
    }


}
