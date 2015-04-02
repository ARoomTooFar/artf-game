using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class Event_ItemButtons : MonoBehaviour,/* IBeginDragHandler, IEndDragHandler,*/ IPointerClickHandler
{
	TileMapController tilemapcont;
	static Camera UICamera;
	Image thisImage;
	GameObject draggedImageAnchor;
	string itemToPlace;
	Material matToMakeInvisible;
	string connectedPrefab = "";
	Vector3 newp;
	public LayerMask draggingLayerMask;
	bool clicked = false;
	Shader translucentShader;
	static GameObject bgButt;
	static GameObject itemObjectCopy = null;
	static int selectedButtonID = -1;
	string placedItemName;
	Vector3 placedItemPos;
	Vector3 placedItemRot;
	
	void Start ()
	{

		UICamera = GameObject.Find ("UICamera").GetComponent<Camera> ();
		tilemapcont = GameObject.Find ("TileMap").GetComponent ("TileMapController") as TileMapController;
		thisImage = this.GetComponent ("Image") as Image;
		draggedImageAnchor = GameObject.Find ("DraggedImageAnchor");
		Image p = draggedImageAnchor.GetComponent ("Image") as Image;
		matToMakeInvisible = Resources.Load ("Textures/basecolor") as Material;
		p.material = matToMakeInvisible;

		translucentShader = Shader.Find ("Transparent/Bumped Diffuse");
	}

	void Update ()
	{
//		Debug.Log(clicked);
		//if this button has been clicked, enter the ghost-dragging phase
		if (clicked && selectedButtonID == this.gameObject.GetInstanceID ()) {
			Ray ray = UICamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit; 
			
			if (Physics.Raycast (ray, out hit, Mathf.Infinity) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject () == false) {

				clicked = false;
				StartCoroutine (folderGhostDragging ());

			}
		}
	}

	/*
	 * This begins the click-to-drag functionality for buttons in folders.
	 *
	 */
	public void OnPointerClick (PointerEventData data)
	{
		//deselect any other button that may be selected
		if (selectedButtonID != -1 && selectedButtonID != this.gameObject.GetInstanceID ()) {
			Destroy (bgButt);

//			MapData.removeScenery (newp);
		}
		clicked = true;


		//set this button to the currently selected button
		selectedButtonID = this.gameObject.GetInstanceID ();

//			Debug.Log ("Clicked a button, selectedbuttonid: " + selectedButtonID);

		//make new button to create a outline
		bgButt = Instantiate (Resources.Load ("bgButton")) as GameObject;
		bgButt.transform.SetParent (this.transform.parent);
		RectTransform bgRect = bgButt.GetComponent ("RectTransform") as RectTransform;

		//set its position and scale
		RectTransform thisRect = this.gameObject.GetComponent ("RectTransform") as RectTransform;
		bgRect.anchoredPosition = new Vector2 (thisRect.anchoredPosition.x, thisRect.anchoredPosition.y);
		bgRect.sizeDelta = new Vector2 (thisRect.sizeDelta.x + 10f, thisRect.sizeDelta.y + 10f);

		//set its color
		Button bgButton = bgButt.GetComponent ("Button") as Button;
		bgButton.image.color = Color.yellow;

		//make it so it's just an outline
		bgButton.image.fillCenter = false;

	}

	IEnumerator folderGhostDragging ()
	{ 
		string prefabLocation = "Prefabs/" + connectedPrefab;

		//for the ghost-duplicate
		itemObjectCopy = null;
		ItemObject copy = null;
		
		bool cancellingMove = false;
		bool copyCreated = false;
		newp = new Vector3 (0f, 0f, 0f);
		bool doorRotated = false;
		Vector3 doorWallRot = new Vector3 (0f, 0f, 0f);
		
		while (!Input.GetMouseButton(0)) { 

			//if we haven't made a copy of the object yet
			if (!copyCreated) {
				//create copy of item object
				itemObjectCopy = Instantiate (Resources.Load (prefabLocation)) as GameObject;
				copy = itemObjectCopy.GetComponent ("ItemObject") as ItemObject;
				
				//so this code only happens once
				copyCreated = true;
			}
			
			//if user wants to cancel the drag
			if (Input.GetKeyDown (KeyCode.Escape)) {
				Destroy (itemObjectCopy);
				cancellingMove = true;
				
				//break out of while loop
				break;
			}
			
			Ray ray = UICamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;

			if (Physics.Raycast (ray, out hitInfo, Mathf.Infinity, draggingLayerMask)) {
				if (hitInfo.collider.gameObject.name == "TileMap") {
					int x = Mathf.RoundToInt (hitInfo.point.x / tilemapcont.tileSize);
					int z = Mathf.RoundToInt (hitInfo.point.z / tilemapcont.tileSize);

					//postion holder for inside this loop
					Vector3 movePos = new Vector3 (x, 0f, z);

					//for final placement of object, after we break from this loop
					newp = movePos;

					//if copy exists
					if (copyCreated) {

						//if we got a door and it's on the edge of a room
						if (itemObjectCopy.GetComponent<SceneryData> () != null
							&& itemObjectCopy.GetComponent<SceneryData> ().isDoor
							&& MapData.TheFarRooms.find (movePos) != null
						   ) {
							//snap door to an edge if it's near it
							if ((MapData.TheFarRooms.find (movePos).isCloseToEdge (movePos, 3f)))
								movePos = MapData.TheFarRooms.find (movePos).getNearestEdgePosition (movePos);

							//set its new rotation
							if (MapData.TheFarRooms.find (movePos).isEdge (movePos)) {
								doorRotated = true;
								doorWallRot = MapData.TheFarRooms.find (movePos).getWallSide (movePos).toRotationVector ();
							}

						}


						//update the item object things
						//shader has to be set in this loop, or transparency won't work
						if (itemObjectCopy.gameObject.GetComponent<Renderer> () != null) {
							itemObjectCopy.gameObject.GetComponent<Renderer> ().material.shader = translucentShader;
							Color trans = itemObjectCopy.gameObject.GetComponent<Renderer> ().material.color;
							trans.a = 0.5f;
							itemObjectCopy.gameObject.GetComponent<Renderer> ().material.SetColor ("_Color", trans);
						}
					}

					//if it's a door, set it to last wall rotation
					if (doorRotated == true) {
						itemObjectCopy.transform.eulerAngles = doorWallRot;
					}

					itemObjectCopy.transform.position = movePos;
				}
			}
			yield return null; 
		}
		

		
		//if move was cancelled, we don't perform an update on the item object's position
		if (cancellingMove == true) {
			Destroy (bgButt);
			selectedButtonID = -1;
		} else {
			Vector3 pos = newp;
			Vector3 rot = new Vector3 (0f, 0f, 0f);

			//don't place item if we've click a button
			if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject () == false){
				SceneryData sdat = itemObjectCopy.GetComponent<SceneryData>();
				if (sdat != null && sdat.isDoor){
					ARTFRoom rm = MapData.TheFarRooms.find (pos);
					if(rm != null && rm.isCloseToEdge(pos, 3f)){
						pos = MapData.TheFarRooms.find (pos).getNearestEdgePosition (pos);
					}
				}
				MapData.addMonsterScenery (prefabLocation, pos, rot.toDirection ());
			}

			//destroy the copy
			Destroy (itemObjectCopy);
			itemObjectCopy = null;
//			placedItemName = prefabLocation;
//			placedItemPos = pos;
//			placedItemRot = rot;

			Destroy (bgButt); //get rid of yellow selecting box around button
			selectedButtonID = -1;
		}
	}

	public void setConnectedPrefab (string s)
	{
		connectedPrefab = s;
	}

	public void setButtonImage (string icon)
	{
		Image im = this.GetComponent ("Image") as Image;
		Sprite sp = Resources.Load <Sprite> ("IconsUI/" + icon);
		im.sprite = sp;
	}


//	public void OnBeginDrag (PointerEventData data)
//	{
//
//		tilemapcont.suppressDragSelecting = true;
//
//		Image p = draggedImageAnchor.GetComponent ("Image") as Image;
//		p.sprite = thisImage.sprite;
//		p.material = null;
//
//		StartCoroutine (dragIt ());
//
//
//	}
//
//	//for placing items by way of dragging the buttons from the folder and
//	//placing them on the map
//	public void OnEndDrag (PointerEventData data)
//	{
//		tilemapcont.suppressDragSelecting = false;
//
//		Image p = draggedImageAnchor.GetComponent ("Image") as Image;
//		p.sprite = thisImage.sprite;
//		p.material = matToMakeInvisible;
//
//
//		string prefabLocation = "Prefabs/" + connectedPrefab;
//		tilemapcont.setSelectedItem (prefabLocation);
//
//		//make sure image anchor is way off screen, so it doesn't interfere
//		//with dragging of objects
//		RectTransform anchorRect = draggedImageAnchor.GetComponent ("RectTransform") as RectTransform;
//		anchorRect.anchoredPosition = new Vector2 (-22f, -100f);
//	}

	IEnumerator dragIt ()
	{ 
		while (Input.GetMouseButton(0)) {
			draggedImageAnchor.transform.position = Input.mousePosition;

			yield return null; 
		}

		
	}
}
