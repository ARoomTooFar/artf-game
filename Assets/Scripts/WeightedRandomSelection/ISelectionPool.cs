public interface ISelectionPool<T> : ISelectionItem<T> {
	void addItem(ISelectionItem<T> item, float value);
	void removeItem(ISelectionItem<T> item);
}
