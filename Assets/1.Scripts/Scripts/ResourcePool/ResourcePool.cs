using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourcePool<T> where T:IPoolableResource {

	private T original;
	private int growth;
	private Stack<T> pool = new Stack<T>();

	ResourcePool(T original, int initialPool = 10, int growth = 5){
		this.original = original;
		this.growth = growth;
		restock(initialPool);
	}

	public T getResource(){
		if(pool.Count == 0) {
			restock(growth);
		}
		return pool.Pop();
	}

	public void returnResource(T resource){
		resource.reset();
		pool.Push(resource);
	}

	private void restock(int num){
		for(int i = 0; i < num; ++i) {
			pool.Push((T)original.clone());
		}
	}
}
