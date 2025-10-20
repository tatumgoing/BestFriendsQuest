using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UI;

public class AddMenuController : MonoBehaviour
{
    [SerializeField] private Transform _categoryParent;
    [SerializeField] private Scrollbar _slider;

    [ReadOnly] public FeatureSubType Category;

    [Header("Add")]
    [SerializeField] private GameObject _addPrefab;
    [SerializeField] private Transform _addListParent;

    private List<AddOption> _spawnedAddOptions = new List<AddOption>();

    private void OnEnable()
    {
        _slider.value = 0;
        _slider.onValueChanged.Invoke(0);
    }

    public void BuildAddList(IFeatureController faceController)
    {
        foreach (var a in _spawnedAddOptions) Destroy(a.gameObject);
        _spawnedAddOptions.Clear();

        foreach (var feature in faceController.GetAllOptions()) AddOption(feature);

        //ChangeCategory(Category);
    }

    private void AddOption(FeatureSOData feature)
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

        //Category = type;
        //BuildAddList();
    }
}
