public interface IListIItem
{
    public abstract void SetSelected();
    public abstract void Destroy();
    public abstract void Initialize(ItemData item, IItemListController controller);
    public abstract ItemData Item { get; }
    public abstract bool Active { get; }
    public abstract void Deselect();
    public abstract void Hide();
    public abstract void Show();
}
