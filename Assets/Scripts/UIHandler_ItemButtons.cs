using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class UIHandler_ItemButtons : MonoBehaviour, IBeginDragHandler, IEndDragHandler {

	MouseHandler_TileSelection mouseControl;
	Image thisImage;
	GameObject draggedImageAnchor;
	string itemToPlace;
	Material matToMakeInvisible;

	
	void Start () {
		mouseControl = GameObject.Find ("TileMap").GetComponent ("MouseHandler_TileSelection") as MouseHandler_TileSelection;
		thisImage = this.GetComponent("Image") as Image;
		draggedImageAnchor = GameObject.Find ("DraggedImageAnchor");
		Image p = draggedImageAnchor.GetComponent("Image") as Image;
		matToMakeInvisible = Resources.Load("Textures/basecolor") as Material;
		p.material = matToMakeInvisible;
	}

	void Update () {
	
	}

	public void OnBeginDrag (PointerEventData data)
	{
//		Debug.Log ("asfasdf");
		Image p = draggedImageAnchor.GetComponent("Image") as Image;
		p.sprite = thisImage.sprite;
		p.material = null;
//		draggedImageAnchor.SetActive(true);
		StartCoroutine (dragIt ());


	}

	public void OnEndDrag (PointerEventData data)
	{
//		Debug.Log ("ended");
//		draggedImageAnchor.SetActive(false);
		Image p = draggedImageAnchor.GetComponent("Image") as Image;
		p.sprite = thisImage.sprite;
		p.material = matToMakeInvisible;
		string g = "Prefabs/" + this.transform.gameObject.name;
		mouseControl.setSelectedObject (g);
	}

	IEnumerator dragIt ()
	{ 
		while(Input.GetMouseButton(0)){
			draggedImageAnchor.transform.position = Input.mousePosition;
//			Debug.Log ("dragging");
			yield return null; 
		}

		
	}
}
