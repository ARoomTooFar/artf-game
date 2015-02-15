using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary; 
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Linq;

public class ItemClass  
{
	Transform itemObjects = GameObject.Find ("ItemObjects").GetComponent ("Transform") as Transform;

	//names with aspects
	//e.g. what's the position of walltile_45?
	public static Dictionary<string, itemAspects> nameDic = new Dictionary<string, itemAspects> ();

	//positions with types
	//e.g. does tile (4, 15, 12) have a wall tile on it?
	public static Dictionary<Vector3, List<string>> positionTypeDic = new Dictionary<Vector3, List<string>> ();

	//positions with names
	//e.g. what particular instances of objects are at this position
	public static Dictionary<Vector3, List<string>> positionNameDic = new Dictionary<Vector3, List<string>> ();

	//types with aspects
	//e.g. where a
//	public static Dictionary<string, itemAspects> typeDic = new Dictionary<string, itemAspects>();

	public struct itemAspects
	{
		public string type;
		public string name;
		public Vector3 position;
		public Vector3 rotation;
	}
	

	public void addItem (string name, Vector3 posVec, Vector3 rotVec)
	{
		itemAspects its = new itemAspects ();

		its.type = name.Substring (0, name.IndexOf ('_'));
		its.name = name;
		its.position = posVec;
		its.rotation = rotVec;

		nameDic.Add (its.name, its);

		//if positionTypeDic doesn't have the vector, add it
		if (!positionTypeDic.ContainsKey (posVec)) {
			List<string> list = new List<string> ();
			list.Add (its.type);
			positionTypeDic.Add (posVec, list);
			//if positionDic has the vector, but not the type, add it
		} else if (positionTypeDic.ContainsKey (posVec) && !positionTypeDic [posVec].Contains (its.type)) {
			positionTypeDic [posVec].Add (its.type);
			//if list exists, and it doesn't contain the type, add it
		}

		//if positionNameDic doesn't have the vector, add it
		if (!positionNameDic.ContainsKey (posVec)) {
			List<string> list = new List<string> ();
			list.Add (its.name);
			positionNameDic.Add (posVec, list);
			//if positionDic has the vector, but not the name, add it
		} else if (positionNameDic.ContainsKey (posVec) && !positionNameDic [posVec].Contains (its.name)) {
			positionNameDic [posVec].Add (its.name);
			//if list exists, and it doesn't contain the name, add it
		}

	}

//	public void removeItemByName(string name){
//		positionTypeDic.Remove(nameDic[name].position);
//		positionNameDic.Remove(nameDic[name].position);
//
//		//must be done last
//		nameDic.Remove(name);
//	}

	//remove all items of a particular type from a location
	public void removeItemTypesAtLocation (string type, Vector3 posVec)
	{

		//find list in itemObject list and remove it (the prefab for it)
//		foreach (Transform child in itemObjects) {
//			if (String.Equals (child.transform.name, nameDic.)) {
//				GameObject.Destroy (child.gameObject);
//			}
//		}

		//remove from nameDic
//		foreach(string s in nameDic.Keys){
//			string nameToType = s.Substring (0, s.IndexOf ('_'));
//			if(String.Equals(nameToType, type)){
//				nameDic.Remove(s);
//
//			}
//		}

		//remove from positionTypeDic
		if (positionTypeDic.ContainsKey (posVec) && positionTypeDic [posVec].Contains (type)) {
			positionTypeDic [posVec].Remove (type);
		}

//		foreach (Vector3 vec in positionNameDic.Keys) {
//			foreach (string s in positionNameDic[vec]) {
//				string nameToType = s.Substring (0, s.IndexOf ('_'));
//				if (String.Equals (nameToType, type)) {
//					positionNameDic[vec].Remove (s);
//					nameDic.Remove(s);
//
//				}
//			}
//		}


	}

	public void changeItemPosition (string name, Vector3 posVec)
	{
		//if no entry in type position dictionary, create one
		if (!positionTypeDic.ContainsKey (posVec)) {
			List<string> list = new List<string> ();
			list.Add (nameDic [name].type);
			positionTypeDic.Add (posVec, list);
			//if list exists, but doesn't contain type, add the type
			//can be used later for checking for placing multiple items on one tile
		} else if (positionTypeDic.ContainsKey (posVec) && !positionTypeDic [posVec].Contains (nameDic [name].type)) {
			positionTypeDic [posVec].Add (nameDic [name].type);
		}

		//if no entry in name position dictionary, create one
		if (!positionNameDic.ContainsKey (posVec)) {
			List<string> list = new List<string> ();
			list.Add (name);
			positionNameDic.Add (posVec, list);
			//if list exists, but doesn't contain type, add the type
			//can be used later for checking for placing multiple items on one tile
		} else if (positionNameDic.ContainsKey (posVec) && !positionNameDic [posVec].Contains (name)) {
			positionNameDic [posVec].Add (name);
		}

		//update nameDic
		itemAspects its = nameDic [name];
		its.position = posVec;
		nameDic [name] = its; 
	}

	public void changeItemRotation (string name, Vector3 rotVec)
	{
		itemAspects its = nameDic [name];
		its.rotation = rotVec;
		nameDic [name] = its; 
	}

	public bool positionContainsType (Vector3 posVec, string type)
	{
		if (positionTypeDic.ContainsKey (posVec)){
			if(positionTypeDic [posVec].Contains (type)) {
				return true;
			} else {
				return false;
			}
		}else{
			return false;
		}
	}

















	public static List<ItemStruct>
		itemList;

	[System.Serializable]
	public struct ItemStruct
	{
		public string item;
		public float x;
		public float y;
		public float z;
		public float xrot;
		public float yrot;
		public float zrot;

//		[System.Serializable]
//		public struct PosVecStruct{
//			public float x;
//			public float y;
//			public float z;
//		}
//		
//		public List<PosVecStruct> PosVecLists;
//
//		public void makeNewPosVecStruct(){
//			PosVecLists = new List<PosVecStruct> ();
//		}

	}

	static int nameCounter = 0;
	
	public ItemClass ()
	{
		itemList = new List<ItemStruct> ();
	}

	public List<ItemStruct> getItemList ()
	{
		return itemList;
	}
	
	public void modifyItemList (string s, Vector3 pos, Vector3 rot)
	{

		this.removeItemFromList (s);
		
		ItemStruct its = new ItemStruct ();
		its.item = s;
		its.x = pos.x;
		its.y = pos.y;
		its.z = pos.z;

		its.xrot = rot.x;
		its.yrot = rot.y;
		its.zrot = rot.z;

		this.addStructToItemList (its);
	}

	public void addToItemList (string s, Vector3 pos, Vector3 rot)
	{

		ItemStruct its = new ItemStruct ();
		its.item = s;

		its.x = pos.x;
		its.y = pos.y;
		its.z = pos.z;

		//$
//		its.makeNewPosVecStruct ();

		its.xrot = rot.x;
		its.yrot = rot.y;
		its.zrot = rot.z;

		addStructToItemList (its);
	}

	public void addStructToItemList (ItemStruct h)
	{
		itemList.Add (h);
	}

	public void removeItemFromList (string s)
	{

		for (int i = 0; i < itemList.Count; i++) {
			if (String.Equals (itemList [i].item, s)) {
				itemList.Remove (itemList [i]);
			}
		}
	}

	public int getItemListLength ()
	{
		return itemList.Count;
	}

	//check if an item of a certain type is on a certain tile position
	public bool itemOnPlace (string name, Vector3 pos)
	{

		//if the itemlist is empty, no need to search
		if (getItemListLength () == 0) {
			return false;
		}

		string prefabType;

		for (int i = 0; i < itemList.Count; i++) {
			prefabType = itemList [i].item.Substring (0, itemList [i].item.IndexOf ('_'));

			//if itemlist entry is type we're looking for, and if
			//it occupies the position we're checking against
//			Debug.Log ("comaring " + prefabType + " with " + name );
			if (String.Equals (prefabType, name)
				&& itemList [i].x == pos.x
				&& itemList [i].y == pos.y
				&& itemList [i].z == pos.z) {
				return true;
			}
		}

		//if no match was found, the tile doesn't have the thing on it
		return false;
	}

	public string makeName (string s)
	{
		return s + "_" + nameCounter++;
	}

	public void resetNameCounter ()
	{
		nameCounter = 0;
	}

	public void clearItemList ()
	{
		itemList.Clear ();
	}


}
