public interface ISelectionPool<T> : ISelectionItem<T> {
	ISelectionPool<T> addItem(ISelectionItem<T> item, float value);
	ISelectionPool<T> removeItem(ISelectionItem<T> item);
}
