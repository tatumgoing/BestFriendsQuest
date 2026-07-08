using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType { Clothing, Food, Housing}
public enum ClothingType { OUTFIT, TOP, BOTTOM, HAT}

[CreateAssetMenu(fileName = "Item", menuName = "Data/Item", order = 1)]
public class ItemData : ScriptableObject
{
    [ReadOnly] public ID ID;

    public Sprite sprite;
    public string Name;
    [TextArea(3,10)] public string Description;
    public float Cost;
    public bool StartUnlocked; //INVESTIGATE

    [Space()]
    public ItemType Type;

    [ConditionalField(nameof(Type), false, false, ItemType.Clothing)] public ClothingType ClothingType;
    [ConditionalField(nameof(Type), false, false, ItemType.Clothing), SerializeField] private bool _customColor;
    [ConditionalField(nameof(Type), false, false, ItemType.Clothing), SerializeField] private Color _color;
    [ConditionalField(nameof(Type), false, false, ItemType.Clothing), SerializeField] private Texture _texture;
    
    public Texture Texture => _texture;

    private void OnValidate()
    {
        if (Name.Length < 2) Name = name;
    }

    [ButtonMethod]
    public void RegenerateID()
    {
        ID.GenerateNew();
        Debug.Log("Generating new ID for " + name + ". All saved instances of this item have been erased.");
        Utils.SetDirty(this);
    }

    public void AffectMesh(SetMaterialField mesh)
    {
        if (!mesh) return;
        if (_customColor) mesh.SetColor(_color);
    }
}
