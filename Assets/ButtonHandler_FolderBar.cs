using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class ButtonHandler_FolderBar : MonoBehaviour, IPointerClickHandler
{
	public GameObject[] folders;
	public GameObject folderObject;
	int numberOfFolders;
	
	void Start ()
	{
		numberOfFolders = folderObject.gameObject.transform.GetChildCount ();
		Debug.Log (numberOfFolders);
		folders = new GameObject[numberOfFolders];
		
		for (int i = 0; i < numberOfFolders; i++) {
			folders [i] = folderObject.gameObject.transform.GetChild (i).gameObject;
		}
		
	}
	
	void Update ()
	{
		
	}
	
	public void OnPointerClick (PointerEventData data)
	{

		setFolderState (this.name);
		
		
	}
	
	void setFolderState (string name)
	{
		//Debug.Log (name.Substring(name.IndexOf('_') + 1,( name.Length - name.IndexOf('_') - 1)));
		
		for (int i = 0; i < numberOfFolders; i++) {
			string typeOfFolder = name.Substring (name.IndexOf ('_') + 1, (name.Length - name.IndexOf ('_') - 1));
			string typeOfButton = folders[i].name.Substring (folders[i].name.IndexOf ('_') + 1, (folders[i].name.Length - folders[i].name.IndexOf ('_') - 1));
			if (String.Equals (typeOfButton, typeOfFolder)){
				folders [i].SetActive (!folders [i].activeSelf);
			}else{
				folders [i].SetActive (false);
			}
		}
	}
}