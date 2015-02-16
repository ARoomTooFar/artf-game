using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

//This class attaches to every button in the folder bar.
//It controls all all activity that takes place in the
//folder bar due to user interaction.
public class UIHandler_FolderBar : MonoBehaviour, IPointerClickHandler
{
	//default color for the folder buttons
	Color buttonColor;

	Color selectedButtonColor = Color.cyan;

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

	//holds the entire folder bar. for use with sliding it off and on screen
	public RectTransform folderBar;

	public GameObject blankFolder;

	bool lerpingFolderClosed = false;
	bool lerpingFolderOpen = false;
	float currentPosX = 0f;
	float openPosX = -107.5f;
	float closePosX = 40f;
	Vector3 currentPos;

	string currentButton;
	
	void Start ()
	{
		buttonColor = new Color32(0, 147, 176, 255);

		//get number of folder scrollviews and make an array for them
		numberOfFolders = folderObject.gameObject.transform.childCount;
		folders = new GameObject[numberOfFolders];

		//add all the folder scrollviews to their array
		for (int i = 0; i < numberOfFolders; i++) {
			folders [i] = folderObject.gameObject.transform.GetChild (i).gameObject;
		}

		//get number of folder buttons and setup the array for them
		numberOfbuttons = buttonObject.gameObject.transform.childCount;
		buttons = new GameObject[numberOfbuttons];

		//shove all of the folder button into the array we have for them
		for (int i = 0; i < numberOfbuttons; i++) {
			buttons [i] = buttonObject.gameObject.transform.GetChild (i).gameObject;
			Image img = buttons [i].GetComponent<Image> ();
			img.color = buttonColor;
		}

//		folderBar = GameObject.Find("FolderBar").GetComponent ("RectTransform") as RectTransform;
		folderBar = GameObject.Find("FolderBar").GetComponent<RectTransform>();

		blankFolder = GameObject.Find("BlankFolder");

	
	}

	//Pre: User clicked the button this script is attached to
	//Post: Opens the right folder, and highlights the right button
	//
	//This function is part of the Unity GUI event system.
	public void OnPointerClick (PointerEventData data)
	{
		buttonIsSelected = !buttonIsSelected; //boolean for changing button color

		setFolderStates (this.name);

		slideFolderBar();

		setButtonStates ();
	}

	void Update(){
		if(lerpingFolderClosed == true){
			currentPosX = Mathf.MoveTowards (folderBar.anchoredPosition.x, closePosX, Time.deltaTime * 600f);
			currentPos = new Vector3(currentPosX, -245.5f, 0f);
			folderBar.anchoredPosition = currentPos;

			if(folderBar.anchoredPosition.x == closePosX){
				lerpingFolderClosed = false;
			}
		}

		if(lerpingFolderOpen == true){
			currentPosX = Mathf.MoveTowards (folderBar.anchoredPosition.x, openPosX, Time.deltaTime * 600f);
			currentPos = new Vector3(currentPosX, -245.5f, 0f);
			folderBar.anchoredPosition = currentPos;
			
			if(folderBar.anchoredPosition.x == openPosX){
				lerpingFolderOpen = false;
			}
		}
	}

	void slideFolderBar(){
		bool hideFolderBar = true;
//		Vector3 openPos = new Vector3(openPosX, -245.5f, 0f);
//		Vector3 closePos = new Vector3(closePosX, -245.5f, 0f);

		for (int i = 0; i < numberOfFolders; i++) {
			if(folders [i].activeSelf){
				hideFolderBar = false;
			}

			if(hideFolderBar){
				lerpingFolderClosed = true;
				lerpingFolderOpen = false;

//				folderBar.anchoredPosition = closePos;

//				blankFolder.SetActive(true);
			}else{
				lerpingFolderClosed = false;
				lerpingFolderOpen = true;

//				folderBar.anchoredPosition = openPos;

//				blankFolder.SetActive(false);
			}
		}
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
				currentButton = "Button_" + typeOfButton;
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
		//change its color to grey. otherwise, set it to black 
		for (int i = 0; i < numberOfbuttons; i++) {
			if (String.Equals (buttons [i].name, currentButton)) {
				Image img = buttons [i].GetComponent<Image> ();
				img.color = selectedButtonColor;

				//if the folder bar is going into its closed state, 
				//no button should be highlighted
				if(lerpingFolderClosed == true){
					img.color = buttonColor;
				}
			} else {
				Image img = buttons [i].GetComponent<Image> ();
				img.color = buttonColor;
			}
		}
	}
}