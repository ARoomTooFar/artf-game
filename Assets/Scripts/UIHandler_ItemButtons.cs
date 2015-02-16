using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class UIHandler_ItemButtons : MonoBehaviour, IBeginDragHandler, IEndDragHandler {
	Input_TileMap input_tileMap;

//	MouseHandler_TileSelection mouseControl;
	Image thisImage;
	GameObject draggedImageAnchor;
	string itemToPlace;
	Material matToMakeInvisible;

	
	void Start () {
		input_tileMap = GameObject.Find ("TileMap").GetComponent("Input_TileMap") as Input_TileMap;
//		mouseControl = GameObject.Find ("TileMap").GetComponent ("MouseHandler_TileSelection") as MouseHandler_TileSelection;
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
		Image p = draggedImageAnchor.GetComponent("Image") as Image;
		p.sprite = thisImage.sprite;
		p.material = null;

		StartCoroutine (dragIt ());


	}

	public void OnEndDrag (PointerEventData data)
	{

		Image p = draggedImageAnchor.GetComponent("Image") as Image;
		p.sprite = thisImage.sprite;
		p.material = matToMakeInvisible;

		string g = "Prefabs/" + this.transform.gameObject.name;
//		mouseControl.setSelectedItem (g);
		input_tileMap.setSelectedItem(g);

		//make sure image anchor is way off screen, so it doesn't interfere
		//with dragging of objects
		RectTransform anchorRect = draggedImageAnchor.GetComponent("RectTransform") as RectTransform;
		anchorRect.anchoredPosition = new Vector2(-22f, -100f);
	}

	IEnumerator dragIt ()
	{ 
		while(Input.GetMouseButton(0)){
			draggedImageAnchor.transform.position = Input.mousePosition;

			yield return null; 
		}

		
	}
}
