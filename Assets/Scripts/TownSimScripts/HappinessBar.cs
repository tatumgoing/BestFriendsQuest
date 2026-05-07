using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HappinessBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;

    private ID _id = new ID(-1);

    public void Initialize(ID id) => _id = id;

    void Update()
    {
        if (_id != -1) _slider.value = CharacterManager.i.GetHappiness(_id)/100f;
    }
}
