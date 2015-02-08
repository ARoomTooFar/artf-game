using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataHandler_Items : MonoBehaviour {
	
//	Transform itemObjects;
	
//	Dictionary<string, Vector3> itemDictionary;

	int nameCounter = 0;

//	ItemClass itemClass = new ItemClass();

	void Start () {
//		itemDictionary = new Dictionary<string, Vector3>();
//		itemObjects = GameObject.Find ("ItemObjects").GetComponent("Transform") as Transform;
	}
	

	//actual objects in scene

//	public void wipeItemObjects(){
//		foreach(Transform child in itemObjects){
//			GameObject.Destroy(child.gameObject);
//		}
//		nameCounter = 0;
//	}

//	public Transform getItemObjects(){
//		return itemObjects;
//	}

	//dictionary storage of objects in scene

//	public void clearItemDictionary(){
//		itemDictionary.Clear ();
////		itemClass.clearItemList();
//	}

//	public Dictionary<string, Vector3> getItemDictionary(){
//		return itemDictionary;
//	}
//	
//	public void copyIntoItemDictionary(Dictionary<string, Vector3> dic){
//		itemDictionary = new Dictionary<string, Vector3>(dic);
//	}

//	public void addToItemList(string s, Vector3 pos){
////		itemDictionary.Add (s, pos);
//
//		ItemClass.ItemStruct its = new ItemClass.ItemStruct();
//		its.item = s;
//		its.x = pos.x;
//		its.y = pos.y;
//		its.z = pos.z;
//		itemClass.addToItemList(its);
//	}

//	public void modifyItemList(string s, Vector3 pos){
////		itemDictionary[s] = pos;
//
//		itemClass.removeItemFromList(s);
//
//		ItemClass.ItemStruct its = new ItemClass.ItemStruct();
//		its.item = s;
//		its.x = pos.x;
//		its.y = pos.y;
//		its.z = pos.z;
//		itemClass.addToItemList(its);
//	}

//	public string makeName(string s){
//		return s + "_" + nameCounter++;
//	}
}
