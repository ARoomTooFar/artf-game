using UnityEngine;
//using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SelectionPoolUnweighted<T> : ISelectionPool<T> {

	private HashSet<ISelectionItem<T>> pool = new HashSet<ISelectionItem<T>>();

	public SelectionPoolUnweighted() {
	}

	public void addItem(ISelectionItem<T> item, float value){
		pool.Add(item);
	}

	public void removeItem(ISelectionItem<T> item){
		pool.Remove(item);
	}

	public ISelectionItem<T> getItem(){
		return pool.ElementAt(Mathf.FloorToInt(Random.value*pool.Count));
	}

	public T openItem(){
		return getItem().openItem();
	}
	
}

