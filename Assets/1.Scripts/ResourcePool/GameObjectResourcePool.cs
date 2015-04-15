using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class GameObjectResourcePool
{
	public static int Growth{ get; set; }

	private static Dictionary<string, Stack<GameObject>> pools = new Dictionary<string, Stack<GameObject>> ();

	static GameObjectResourcePool ()
	{
		Growth = 5;
	}

	public static GameObject getResource (string type, Vector3 pos, Vector3 dir)
	{
		Stack<GameObject> stk = getStack (type);
		if (stk.Count == 0) {
			restock (type);
		}
		GameObject retVal = stk.Pop ();
		retVal.transform.position = pos;
		retVal.transform.eulerAngles = dir;
		retVal.SetActive (true);
		return retVal;
	}

	public static void returnResource (string type, GameObject obj)
	{
		obj.SetActive (false);
		getStack (type).Push (obj);
	}

	private static void restock (string type)
	{
		Stack<GameObject> stk = getStack (type);
		GameObject newThing;
		for (int i = 0; i < Growth; ++i) {
			newThing = getNewInstance (String.Format(type, Global.inLevelEditor?"LevelEditor":"Gameplay"));
			stk.Push (newThing);
		}
	}

	private static Stack<GameObject> getStack (string type)
	{
		Stack<GameObject> stk;
		try {
			stk = pools [type];
		} catch (Exception) {
			stk = new Stack<GameObject> ();
			pools.Add (type, stk);
		}
		return stk;
	}

	private static GameObject getNewInstance (string type)
	{
		//Debug.Log(type);
		GameObject temp = GameObject.Instantiate (Resources.Load (type), new Vector3 (1, 0, 1), new Quaternion ()) as GameObject;
		temp.SetActive (false);
		return temp;
	}
}
