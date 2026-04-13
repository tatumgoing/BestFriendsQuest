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

    public void DisplayItem(ItemData item)
    {
        _mannequin.ShowClothingItem(item);
    }
}
