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

		fillUpFolderArray();
		createButtons();

//		for (int i = 0; i < numberOfFolders; i++) {
//			folders [i] = transform.GetChild (i).gameObject;
//
//			//get ItemList gameobject under this folder
//			Transform itemList = folders [i].transform.Find("ScrollView/ItemList");
//
//			//FOR TESTING
//			if(String.Equals(folders[i].name, "Folder_Trap")){
//				//get prefabs we need for this folder
//				UnityEngine.Object[] prefabs = Resources.LoadAll("Prefabs/Trap");
//
//				//setup each button
//				for(int h = 0; h < itemList.childCount; h++){
//					UIHandler_ItemButtons uih = itemList.GetChild(h).gameObject.AddComponent("UIHandler_ItemButtons") as UIHandler_ItemButtons;
//					EventTrigger ev = itemList.GetChild(h).gameObject.AddComponent("EventTrigger") as EventTrigger;
//					uih.setConnectedPrefab(folders[i].name.Substring(folders[i].name.IndexOf ('_') + 1) + "/" + prefabs[0].name);
//
//					//must eventually send in string
//					uih.setButtonImage();
//				}
//			}
//		}

	}

	void fillUpFolderArray(){
		for (int i = 0; i < numberOfFolders; i++) {
			folders [i] = transform.GetChild (i).gameObject;
		}
	}

	void createButtons(){
		for (int i = 0; i < numberOfFolders; i++) {
			//get folder type (Puzzle, Monster, etc.)
			string folderType = folders[i].name.Substring(folders[i].name.IndexOf ('_') + 1);

			//get prefabs we need for this folder
			UnityEngine.Object[] prefabs = Resources.LoadAll("Prefabs/" + folderType);

			//get ItemList gameobject under this folder
			Transform itemList = folders [i].transform.Find("ScrollView/ItemList");


			int prefabCounter = 0;
			//setup each button
			for(int h = 0; h < itemList.childCount; h++){

				//if there's still prefabs to assign to buttons
				if(prefabCounter < prefabs.Length){

					//add script to button
					UIHandler_ItemButtons uih = itemList.GetChild(h).gameObject.AddComponent("UIHandler_ItemButtons") as UIHandler_ItemButtons;
					//set button's script to drop prefab

					uih.setConnectedPrefab(folderType + "/" + prefabs[prefabCounter].name);


					//add event trigger to object
					EventTrigger ev = itemList.GetChild(h).gameObject.AddComponent("EventTrigger") as EventTrigger;
					
					//set icon
					string prefabType = prefabs[prefabCounter].name.Substring(prefabs[prefabCounter].name.IndexOf ('_') + 1);
					uih.setButtonImage(prefabType);

					prefabCounter++;
				}else{

					//make rest of buttons inactive, invisible, and unclickabble
					itemList.GetChild(h).gameObject.SetActive(false);
				}


			}
		}
	}

}
