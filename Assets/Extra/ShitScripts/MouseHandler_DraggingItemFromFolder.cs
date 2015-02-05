using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseHandler_DraggingItemFromFolder : MonoBehaviour {

	bool draggingItem = false;
	public GameObject draggedImage;
	MouseHandler_TileSelection mouseControl;
	string itemToPlace;
	Image imageOnMouse;

	void Start () {
		mouseControl = GameObject.Find ("TileMap").GetComponent ("MouseHandler_TileSelection") as MouseHandler_TileSelection;

	}
	

	void Update () {
		if(draggingItem == true){
			Image p = draggedImage.GetComponent("Image") as Image;
			p.sprite = imageOnMouse.sprite;
			draggedImage.SetActive(true);
			draggedImage.transform.position = Input.mousePosition;
			StartCoroutine (dragIt ());
		}

		if (Input.GetMouseButtonUp(0)){
			mouseControl.setSelectedObject (itemToPlace);
			draggingItem = false;
		}
	}

	public void dragItem(string s, Image i){
		imageOnMouse = i;
		draggingItem = true;
		itemToPlace = s;
	}

	IEnumerator dragIt ()
	{ 

			yield return null; 

	}
}
