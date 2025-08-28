using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarShapeMenu : MonoBehaviour
{
    [SerializeField] private EarController _controller;
    [SerializeField] private GameObject _optionPrefab;
    [SerializeField] private Transform _listParent;
    private List<AddOption> _spawnedOptions = new List<AddOption>();

    private void Start()
    {
        BuildList();
        foreach (var o in _spawnedOptions) {
            if (o.GetData().Icon == _controller.GetCurrent().GetData().Icon) {
                o.GetComponent<SelectableItem>().Select();
            }
        }
    }

    public void SelectShape(FeatureSOData data)
    {
        _controller.UpdateCurrentEar(data);
        foreach (var o in _spawnedOptions) if (o.GetData() != data) o.GetComponent<SelectableItem>().Deselect();
    }

    private void BuildList()
    {
        foreach (var o in _spawnedOptions) Destroy(o.gameObject);
        _spawnedOptions.Clear();

        var options = _controller.GetAllOptions();
        foreach (var o in options) SpawnOption(o);
    }

    private void SpawnOption(FeatureSOData option)
    {
        var newOption = Instantiate(_optionPrefab, _listParent).GetComponent<AddOption>();
        newOption.Initialize(option);
        _spawnedOptions.Add(newOption);
    }
}
