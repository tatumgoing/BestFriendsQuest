using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProblemRewardsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;

    public void Show(ProblemData probelm)
    {
        _coinsText.text = "+" + probelm.RewardCurrency;
        gameObject.SetActive(true);
    }
}
