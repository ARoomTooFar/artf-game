using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;


/*
 * This class fills folders with items, based on what
 * prefabs exist in the Resources/prefabs directory
 * 
 */
public class UIHandler_FolderFiller : MonoBehaviour {
	public GameObject[] folders;
	int numberOfFolders;

	public GameObject[] itemButtons;
	int numberOfItemButtons;



	void Start () {
		numberOfFolders = transform.childCount;
		folders = new GameObject[numberOfFolders];



		for (int i = 0; i < numberOfFolders; i++) {
			folders [i] = transform.GetChild (i).gameObject;

			Transform itemList = folders [i].transform.Find("ScrollView/ItemList");
			numberOfItemButtons = itemList.childCount;
//			itemButtons = new GameObject[numberOfItemButtons];



			if(String.Equals(folders[i].name, "Folder_Trap")){
				UnityEngine.Object[] prefabs = Resources.LoadAll("Prefabs/Trap");

				foreach(Transform butt in itemList){
					UIHandler_ItemButtons uih = butt.gameObject.AddComponent("UIHandler_ItemButtons") as UIHandler_ItemButtons;
					EventTrigger ev = butt.gameObject.AddComponent("EventTrigger") as EventTrigger;
					uih.setConnectedPrefab(folders[i].name.Substring(folders[i].name.IndexOf ('_') + 1) + "/" + prefabs[0].name);
				}
			}




//			Debug.Log (prefabs.Length);
		}





	}

	void Update () {
	
	}
}
