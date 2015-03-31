using UnityEngine;
//using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SelectionPoolUnweighted<T> : ISelectionPool<T> {

	private HashSet<ISelectionItem<T>> pool = new HashSet<ISelectionItem<T>>();

	public SelectionPoolUnweighted() {
	}

	public ISelectionPool<T> addItem(ISelectionItem<T> item, float value){
		pool.Add(item);
		return this;
	}

	public ISelectionPool<T> removeItem(ISelectionItem<T> item){
		pool.Remove(item);
		return this;
	}

	public ISelectionItem<T> getItem(){
		return pool.ElementAt(Mathf.FloorToInt(Random.value*pool.Count));
	}

	public T openItem(){
		return getItem().openItem();
	}
	
}

