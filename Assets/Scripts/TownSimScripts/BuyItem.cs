using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyItem : MonoBehaviour
{
    public TownGameManager gameManager;

    public Item item;

    private void Start()
    {
        gameManager = TownGameManager.i;
    }
    public void Puchased()
    {
        if(Mathf.Abs(item.Cost) <= gameManager.currency)
        {
            gameManager.ChangeCurrency(-item.Cost);
            gameManager.AddInventory(item);
        }
        
    }
}
