using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingShopController : MonoBehaviour
{
    [SerializeField] private SpawnedCharacter _mannequin;

    private void Start()
    {
        _mannequin.RandomMannequinPose();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) _mannequin.RandomMannequinPose();
    }

    public void DisplayItem(ItemData item)
    {
        _mannequin.ShowClothingItem(item);
    }
}
