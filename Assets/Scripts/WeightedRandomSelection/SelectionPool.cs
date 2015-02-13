using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SelectionPool<T> : ISelectionPool<T> {

	private Dictionary<ISelectionItem<T>, float> pool
		= new Dictionary<ISelectionItem<T> , float>();
	private float totalWeight = 0;

	public SelectionPool() {
	}

	public ISelectionPool<T> addItem(ISelectionItem<T> item, float value){
		if(value < 0){ throw new ArgumentException("value must be greater than 0"); }
		float oldValue;
		try{
			oldValue = pool[item];
		} catch (KeyNotFoundException){
			oldValue = 0;
		}
		pool[item] = value;
		totalWeight += value - oldValue;
		return this;
	}

	public ISelectionPool<T> removeItem(ISelectionItem<T> item){
		try{
			totalWeight -= pool[item];
		} catch (KeyNotFoundException){
			return this;
		}
		pool.Remove(item);
		return this;
	}

	public ISelectionItem<T> getItem(){
		float rand = UnityEngine.Random.value * totalWeight;

		ISelectionItem<T> retVal = null;
		foreach(KeyValuePair<ISelectionItem<T>, float> kvp in pool) {
			if(rand <= kvp.Value){
				retVal = kvp.Key;
				break;
			}
			rand -= kvp.Value;
		}

		return retVal;
	}

	public T openItem(){
		return getItem().openItem();
	}
	
}

