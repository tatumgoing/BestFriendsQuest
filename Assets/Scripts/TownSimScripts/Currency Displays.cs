using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyDisplays : MonoBehaviour
{
    TownGameManager gameManager;

    public TMP_Text currencyDisplay;

    private void Start()
    {
        gameManager = TownGameManager.i;
    }
    void Update()
    {
        if (this.isActiveAndEnabled)
        {
            currencyDisplay.text = "$" + gameManager.currency.ToString("F2");
        }
    }
}
