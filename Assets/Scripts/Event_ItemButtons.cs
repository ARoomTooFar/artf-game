using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class Event_ItemButtons : MonoBehaviour, IBeginDragHandler, IEndDragHandler {
	TileMapController tilemapcont;
	
	Image thisImage;
	GameObject draggedImageAnchor;
	string itemToPlace;
	Material matToMakeInvisible;
	
	string connectedPrefab = "";

	
	void Start () {
		tilemapcont = GameObject.Find ("TileMap").GetComponent("TileMapController") as TileMapController;
		thisImage = this.GetComponent("Image") as Image;
		draggedImageAnchor = GameObject.Find ("DraggedImageAnchor");
		Image p = draggedImageAnchor.GetComponent("Image") as Image;
		matToMakeInvisible = Resources.Load("Textures/basecolor") as Material;
		p.material = matToMakeInvisible;
	}

	public void setConnectedPrefab(string s){
		connectedPrefab = s;
	}

	public void setButtonImage(string icon){
		Image im = this.GetComponent("Image") as Image;
		Sprite sp = Resources.Load <Sprite>("IconsUI/" + icon);
		im.sprite = sp;
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


		string prefabLocation = "Prefabs/" + connectedPrefab;
		tilemapcont.setSelectedItem(prefabLocation);

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
