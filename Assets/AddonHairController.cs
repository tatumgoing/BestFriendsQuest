using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AddonHairController : MonoBehaviour
{
    [SerializeField] private AddMenuController _addMenu;
    [SerializeField] private HairController _hairController;

    private List<FeatureSOData> _hairData = new List<FeatureSOData>();

    private void Start()
    {
        _hairData = Resources.LoadAll<FeatureSOData>("HairFeatures").Where(x => !x.IsMainHair).OrderByDescending(x => x.Priority).ToList();
    }

    public void OpenAddMenu()
    {
        _addMenu.BuildAddList(_hairController);
        _addMenu.ChangeCategory(FeatureSubType.ADDONS);
        _addMenu.gameObject.SetActive(true);
    }
}
