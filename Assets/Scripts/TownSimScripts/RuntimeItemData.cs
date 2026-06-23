[System.Serializable]
public class RuntimeItemData
{
    public ItemData Item;
    public bool Unlocked;
    public bool AlreadyOwned;

    public RuntimeItemData(ItemData itemData)
    {
        Item = itemData;
        Unlocked = Item.StartUnlocked;
    }
}
