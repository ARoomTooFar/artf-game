using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataHandler_Items : MonoBehaviour {
	
	Transform ItemObjects;
	
	Dictionary<string, Vector3> ItemDictionary;

	int nameCounter = 0;



	void Start () {
		ItemDictionary = new Dictionary<string, Vector3>();
		ItemObjects = GameObject.Find ("ItemObjects").GetComponent("Transform") as Transform;
	}
	

	void Update () {
	
	}


	//actual objects in scene

	public void wipeItemObjects(){
		foreach(Transform child in ItemObjects){
			GameObject.Destroy(child.gameObject);
		}
		nameCounter = 0;
	}

	public Transform getItemObjects(){
		return ItemObjects;
	}

	//dictionary storage of objects in scene

	public void clearItemDictionary(){
		ItemDictionary.Clear ();
	}

	public Dictionary<string, Vector3> getItemDictionary(){
		return ItemDictionary;
	}
	
	public void copyIntoItemDictionary(Dictionary<string, Vector3> dic){
		ItemDictionary = new Dictionary<string, Vector3>(dic);
	}

	public void addToItemDictionary(string s, Vector3 pos){
		ItemDictionary.Add (s, pos);
	}

	public void modifyItemDictionary(string s, Vector3 pos){
		ItemDictionary[s] = pos;
	}

	public string makeName(string s){
		return s + "_" + nameCounter++;
	}
}
