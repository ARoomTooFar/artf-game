﻿using UnityEngine;
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


	
	public void modifyItemList(string s, Vector3 pos, Vector3 rot){

		this.removeItemFromList(s);
		
		ItemStruct its = new ItemStruct();
		its.item = s;
		its.x = pos.x;
		its.y = pos.y;
		its.z = pos.z;

		its.xrot = rot.x;
		its.yrot = rot.y;
		its.zrot = rot.z;

		this.addStructToItemList(its);
	}


	public void addToItemList(string s, Vector3 pos, Vector3 rot){

		ItemStruct its = new ItemStruct();
		its.item = s;

		its.x = pos.x;
		its.y = pos.y;
		its.z = pos.z;

		its.xrot = rot.x;
		its.yrot = rot.y;
		its.zrot = rot.z;

		addStructToItemList(its);
	}

	public void addStructToItemList (ItemStruct h)
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

	public void resetNameCounter(){
		nameCounter = 0;
	}

	public void clearItemList(){
		itemList.Clear();
	}


}
