﻿using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FolderBarController : MonoBehaviour {
	//default color for the folder buttons
	Color buttonColor;
	public Color selectedButtonColor;

	
	//setup for the scrollviews for folders
	public GameObject[] folders;
	public GameObject folderObject;
	int numberOfFolders;
	
	//setup for the folder buttons
	public GameObject[] buttons;
	public GameObject buttonObject;
	int numberOfbuttons;
	
	//holds the entire folder bar. for use with sliding it off and on screen
	public RectTransform folderBar;
	bool lerpingFolderClosed = true;
	bool lerpingFolderOpen = false;
	float currentPosX = 0f;
	float openPosX = -128f;
	float closePosX = 40f;
	Vector3 currentPos;
	string currentButton;
	
	void Start() {
		Money.moneyDisplay = GameObject.Find("Shop").GetComponentInChildren<Text>() as Text;
		Money.updateMoneyDisplay();

		folderBar = GameObject.Find("FolderBar").GetComponent<RectTransform>();

		buttonColor = new Color32(0, 147, 176, 150);

		//get number of folder scrollviews and make an array for them
		numberOfFolders = folderObject.gameObject.transform.childCount;
		folders = new GameObject[numberOfFolders];

		//get number of folder buttons and setup the array for them
		numberOfbuttons = buttonObject.gameObject.transform.childCount;
		buttons = new GameObject[numberOfbuttons];
		
		//add all the folder scrollviews to their array
		for(int i = 0; i < numberOfFolders; i++) {
			folders[i] = folderObject.gameObject.transform.GetChild(i).gameObject;
		}

		//shove all of the folder button into the array we have for them
		for(int i = 0; i < numberOfbuttons; i++) {
			buttons[i] = buttonObject.gameObject.transform.GetChild(i).gameObject;
			Image img = buttons[i].transform.Find("Panel").gameObject.GetComponent<Image>();
			img.color = buttonColor;
		}

		setupButtons();

		//hack to fix needing to click on the folder bar twice when the editor first starts
		foreach(GameObject fold in folders) {
			fold.SetActive(false);
		}
	}

	void Update() {
		if(lerpingFolderClosed == true) {
			currentPosX = Mathf.MoveTowards(folderBar.anchoredPosition.x, closePosX, Time.deltaTime * 600f);
			currentPos = new Vector3(currentPosX, folderBar.anchoredPosition.y, 0f);
			folderBar.anchoredPosition = currentPos;
			
			if(folderBar.anchoredPosition.x == closePosX) {
				lerpingFolderClosed = false;
			}
		}
		
		if(lerpingFolderOpen == true) {
			currentPosX = Mathf.MoveTowards(folderBar.anchoredPosition.x, openPosX, Time.deltaTime * 600f);
			currentPos = new Vector3(currentPosX, folderBar.anchoredPosition.y, 0f);
			folderBar.anchoredPosition = currentPos;
			
			if(folderBar.anchoredPosition.x == openPosX) {
				lerpingFolderOpen = false;
			}
		}
	}
	
	public void slideFolderBar() {
		bool hideFolderBar = true;
		
		for(int i = 0; i < numberOfFolders; i++) {
			if(folders[i].activeSelf) {
				hideFolderBar = false;
			}
			
			if(hideFolderBar) {
				lerpingFolderClosed = true;
				lerpingFolderOpen = false;
			} else {
				lerpingFolderClosed = false;
				lerpingFolderOpen = true;
			}
		}
	}
	
	//This functions makes it so the correct folder is opened up, and all the
	//other folders are closed. It opens a folder by setting the gameobject for it
	//to active, and closes a folder by making it inactive.
	public void setFolderStates(string name) {
		//because of the naming convention we're using for objects, we
		//can find out what type of button the user has clicked by
		//getting all the text after the underscore, e.g. if it's Folder_Tile,
		//we can get the Tile part. This string gets that stubstring.
		string typeOfFolder = name.Substring(name.IndexOf('_') + 1, (name.Length - name.IndexOf('_') - 1));
		
		//loop through the folder array, comparing every folder to the button the user
		//has pressed. if the folder is a match, we set it active. Otherwise, we set it
		//inactive
		for(int i = 0; i < numberOfFolders; i++) {
			//this string gets the type of folder
			string typeOfButton = folders[i].name.Substring(folders[i].name.IndexOf('_') + 1, 
			                                                  (folders[i].name.Length - folders[i].name.IndexOf('_') - 1));
			//compare the button and folder names, and do the appropriate action
			if(String.Equals(typeOfButton, typeOfFolder)) {
				//reverse the boolean that controls if the folder object is active
				folders[i].SetActive(!folders[i].activeSelf);
				currentButton = "Button_" + typeOfButton;
			} else {
				//set the folder object active
				folders[i].SetActive(false);
			}
		}
	}
	
	//This function is for setting the state of the folder buttons.
	//Currently it only changes their color according to which is
	//selected.
	public void setButtonStates() {
		//loop through all buttons. if the button is the one we've just selected,
		//change its color to grey. otherwise, set it to black 
		for(int i = 0; i < numberOfbuttons; i++) {
			if(String.Equals(buttons[i].name, currentButton)) {
				Image img = buttons[i].transform.Find("Panel").gameObject.GetComponent<Image>();
				img.color = selectedButtonColor;
				
				//if the folder bar is going into its closed state, 
				//no button should be highlighted
				if(lerpingFolderClosed == true) {
					img.color = buttonColor;
				}
			} else {
				Image img = buttons[i].transform.Find("Panel").gameObject.GetComponent<Image>();
				img.color = buttonColor;
			}
		}
	}

	void setupButtons() {
		for(int i = 0; i < numberOfFolders; i++) {
			//get folder type (Puzzle, Monster, etc.)
			string folderType = folders[i].name.Substring(folders[i].name.IndexOf('_') + 1);
			
			//get prefabs we need for this folder
			UnityEngine.Object[] prefabs = Resources.LoadAll("LevelEditor/" + folderType);

			//get ItemList gameobject under this folder
			Transform itemList = folders[i].transform.Find("ScrollView/ItemList");

			//just to get dimensions
			GameObject newButtonJustForDimensions = Instantiate(Resources.Load("folderButton")) as GameObject;
			RectTransform newButtonJustForDimensionsRect = newButtonJustForDimensions.GetComponent<RectTransform>();
			float buttonY = -1 * newButtonJustForDimensionsRect.sizeDelta.y / 2 - 20;
			float buttonX = newButtonJustForDimensionsRect.sizeDelta.x / 2 - 5;
			Destroy (newButtonJustForDimensions);

			bool firstIter = true;
			for(int j = 0; j < prefabs.Length; j++) {
				GameObject newButts = Instantiate(Resources.Load("folderButton")) as GameObject;
				newButts.transform.SetParent(itemList);
				RectTransform buttRects = newButts.GetComponent<RectTransform>();

				if(firstIter) {
					firstIter = false;
				} else {
					//if wanna make new column or not
					if(!(j%5 == 0)){
						//continue on down
						buttonY -= buttRects.sizeDelta.y + 30;
					}else{
						//reset y to top
						buttonY = -1 * buttRects.sizeDelta.y / 2 - 20;

						//move buttons over to next column
						buttonX += buttRects.sizeDelta.x + 20;
					}

				}
				buttRects.anchoredPosition = new Vector2(buttonX, buttonY);
			}

			int prefabCounter = 0;
			//setup each button
			for(int h = 0; h < itemList.childCount; h++) {
				
				//if there's still prefabs to assign to buttons
				if(prefabCounter < prefabs.Length) {
					
					//add script to button
					Event_ItemButtons uih = itemList.GetChild(h).gameObject.GetComponent<Event_ItemButtons>();

					//set button's script to drop prefab
					uih.setConnectedPrefab(folderType + "/" + prefabs[prefabCounter].name);

					//add event trigger to object
					//EventTrigger ev = itemList.GetChild(h).gameObject.AddComponent<EventTrigger>() as EventTrigger;
					itemList.GetChild(h).gameObject.AddComponent<EventTrigger>();

					//set icon
					string prefabType = prefabs[prefabCounter].name.Substring(prefabs[prefabCounter].name.IndexOf('_') + 1);
					//Debug.Log (prefabType);
					uih.setButtonImage(prefabType);

					
					//set it's type name, for buying/selling
					//uih.itemType = prefabType;
					uih.price = (prefabs[prefabCounter] as GameObject).GetComponent<LevelEditorData>().baseCost;

					prefabCounter++;
				} else {	
					//make rest of buttons inactive, invisible, and unclickabble
					itemList.GetChild(h).gameObject.SetActive(false);
				}
			}
		}
	}
}
