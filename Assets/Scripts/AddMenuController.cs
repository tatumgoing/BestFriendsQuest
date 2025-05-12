using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMenuController : MonoBehaviour
{
    [SerializeField] private Transform _categoryParent;

    [Header("Add")]
    [SerializeField] private GameObject _addPrefab;
    [SerializeField] private Transform _addListParent;

    private List<AddOption> _spawnedAddOptions = new List<AddOption>();

    public void BuildAddList(IFeatureController faceController)
    {
        foreach (var a in _spawnedAddOptions) Destroy(a.gameObject);
        _spawnedAddOptions.Clear();

        foreach (var feature in faceController.GetAllOptions()) AddOption(feature);
    }

    private void AddOption(FeatureData feature)
    {
        var newAddOption = Instantiate(_addPrefab, _addListParent).GetComponent<AddOption>();
        newAddOption.Initialize(feature);
        _spawnedAddOptions.Add(newAddOption);
    }

    public void DeselectCategories() => ChangeCategory(FeatureSubType.ALL);

    public void ChangeCategory(FeatureSubType type)
    {
        foreach (var button in _categoryParent.GetComponentsInChildren<AddMenuCategoryButton>()) {
            if (type != button.Type) button.GetComponent<SelectableItem>().Deselect(true, false);
        }

        foreach (var option in _spawnedAddOptions) {
            option.gameObject.SetActive(type == FeatureSubType.ALL || option.Type == type);
        }
    }
}
