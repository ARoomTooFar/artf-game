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

[System.Serializable]
public class ItemClass
{
	[SerializeField]
	public static List<ItemStruct>
		itemList;

	[System.Serializable]
	public struct ItemStruct
	{
		public string item;
		public float x;
		public float y;
		public float z;
	}

	int nameCounter = 0;

	
	public ItemClass ()
	{
		itemList = new List<ItemStruct> ();
	}

	public List<ItemStruct> getItemList ()
	{
		return itemList;
	}

	public void addToItemList (ItemStruct h)
	{
		itemList.Add (h);
	}

	public void removeItemFromList (string s)
	{

		for (int i = 0; i < itemList.Count; i++) {
			if (String.Equals (itemList[i].item, s)) {
				itemList.Remove (itemList[i]);
			}
		}
	}

	public string makeName(string s){
		return s + "_" + nameCounter++;
	}

}
