using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyDisplays : MonoBehaviour
{
    TownGameManager gameManager;

    public TMP_Text currencyDisplay;
    [SerializeField] private TextMeshProUGUI _shadowText;
    [SerializeField] private bool _removeDollarSign;

    private void Start()
    {
        gameManager = TownGameManager.i;
    }

    void Update()
    {
        currencyDisplay.text = (_removeDollarSign ? "" : "$") + gameManager.Currency.ToString("F2");
        if (_shadowText) _shadowText.text = currencyDisplay.text;
    }
}
