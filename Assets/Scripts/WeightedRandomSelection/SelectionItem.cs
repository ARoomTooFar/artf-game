public class SelectionItem<T> : ISelectionItem<T> {

	private T item;

	public SelectionItem(T item) {
		this.item = item;
	}

	public ISelectionItem<T> getItem(){
		return this;
	}

	public T openItem(){
		return item;
	}
}

