using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class GameObjectResourcePool {

	public static int Growth{ get; set; }
	private static Dictionary<string, Stack<GameObject>> pools = new Dictionary<string, Stack<GameObject>>();

	static GameObjectResourcePool(){
		Growth = 5;
	}

	public static GameObject getResource(string type){
		Stack<GameObject> stk = getStack(type);
		if(stk.Count == 0){
			restock(type);
		}
		return stk.Pop();
	}

	public static void returnResource(string type, GameObject obj){
		obj.SetActive(false);
		getStack(type).Push(obj);
	}

	private static void restock(string type){
		Stack<GameObject> stk = getStack(type);
		for(int i = 0; i < Growth; ++i) {
			stk.Push(getNewInstance(type));
		}
	}

	private static Stack<GameObject> getStack(string type){
		Stack<GameObject> stk;
		try{
			stk = pools[type];
		}catch (Exception){
			stk = new Stack<GameObject>();
			pools.Add(type, stk);
		}
		return stk;
	}

	private static GameObject getNewInstance(string type){
		return new GameObject();
	}
}
