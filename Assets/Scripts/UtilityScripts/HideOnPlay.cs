using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnPlay : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(GameManager.i || TownGameManager.i);
    }
}
