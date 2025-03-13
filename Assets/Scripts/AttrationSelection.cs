using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttrationSelection : MonoBehaviour
{
    [SerializeField] private CheckBox _male;
    [SerializeField] private CheckBox _female;
    [SerializeField] private CheckBox _nonBinary;

    private DataPanelController _controller;
    private void OnChange(bool selected, Attraction gender) => _controller.UpdateAttraction(gender, selected);

    private void Start()
    {
        _controller = GetComponentInParent<DataPanelController>();
        
        _male.OnChange.AddListener((bool on) => OnChange(on, Attraction.MALE));
        _female.OnChange.AddListener((bool on) => OnChange(on, Attraction.FEMALE));
        _nonBinary.OnChange.AddListener((bool on) => OnChange(on, Attraction.NONBINARY));
    }
}
