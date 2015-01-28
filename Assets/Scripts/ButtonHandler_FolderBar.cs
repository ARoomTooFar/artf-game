using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

//This class attaches to every button in the folder bar.
//It controls all all activity that takes place in the
//folder bar due to user interaction.
public class ButtonHandler_FolderBar : MonoBehaviour, IPointerClickHandler
{
	//setup for the scrollviews for folders
	public GameObject[] folders;
	public GameObject folderObject;
	int numberOfFolders;

	//setup for the folder buttons
	public GameObject[] buttons;
	public GameObject buttonObject;
	int numberOfbuttons;

	//for toggling the color of the buttons
	public bool buttonIsSelected = false;
	
	void Start ()
	{
		//get number of folder scrollviews and make an array for them
		numberOfFolders = folderObject.gameObject.transform.GetChildCount ();
		folders = new GameObject[numberOfFolders];

		//add all the folder scrollviews to their array
		for (int i = 0; i < numberOfFolders; i++) {
			folders [i] = folderObject.gameObject.transform.GetChild (i).gameObject;
		}

		//get number of folder buttons and setup the array for them
		numberOfbuttons = buttonObject.gameObject.transform.GetChildCount ();
		buttons = new GameObject[numberOfbuttons];

		//shove all of the folder button into the array we have for them
		for (int i = 0; i < numberOfbuttons; i++) {
			buttons [i] = buttonObject.gameObject.transform.GetChild (i).gameObject;
		}
	}

	//Pre: User clicked the button this script is attached to
	//Post: Opens the right folder, and highlights the right button
	//
	//This function is part of the Unity GUI event system.
	public void OnPointerClick (PointerEventData data)
	{
		buttonIsSelected = !buttonIsSelected; //boolean for changing button color
		//setButtonStates ();
		setFolderStates (this.name);
	}
	
	//This functions makes it so the correct folder is opened up, and all the
	//other folders are closed. It opens a folder by setting the gameobject for it
	//to active, and closes a folder by making it inactive.
	void setFolderStates (string name)
	{
		//because of the naming convention we're using for objects, we
		//can find out what type of button the user has clicked by
		//getting all the text after the underscore, e.g. if it's Folder_Tile,
		//we can get the Tile part. This string gets that stubstring.
		string typeOfFolder = name.Substring (name.IndexOf ('_') + 1, (name.Length - name.IndexOf ('_') - 1));

		//loop through the folder array, comparing every folder to the button the user
		//has pressed. if the folder is a match, we set it active. Otherwise, we set it
		//inactive
		for (int i = 0; i < numberOfFolders; i++) {
			//this string gets the type of folder
			string typeOfButton = folders [i].name.Substring (folders [i].name.IndexOf ('_') + 1, 
			                                                 (folders [i].name.Length - folders [i].name.IndexOf ('_') - 1));
			//compare the button and folder names, and do the appropriate action
			if (String.Equals (typeOfButton, typeOfFolder)) {
				//reverse the boolean that controls if the folder object is active
				folders [i].SetActive (!folders [i].activeSelf);
			} else {
				//set the folder object active
				folders [i].SetActive (false);
			}
		}
	}

	//This function is for setting the state of the folder buttons.
	//Currently it only changes their color according to which is
	//selected.
	void setButtonStates ()
	{
		//loop through all buttons. if the button is the one we've just selected,
		//change its color. otherwise, set it to black 
		for (int i = 0; i < numberOfbuttons; i++) {
			if (String.Equals (this.name, buttons [i].name)) {
				Image img = this.GetComponent<Image> ();
				if (buttonIsSelected == true) {
					img.color = Color.grey;
				} else {
					img.color = Color.black;
				}
			} else {
				Image img = buttons [i].GetComponent<Image> ();
				img.color = Color.black;

			}
		}
	}
}