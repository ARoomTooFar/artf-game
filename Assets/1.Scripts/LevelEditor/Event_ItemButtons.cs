using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class Event_ItemButtons : MonoBehaviour,/* IBeginDragHandler, IEndDragHandler,*/ IPointerClickHandler {
	TileMapController tilemapcont;
	static Camera UICamera;
	string connectedPrefab = "";
	Vector3 newp;
	public LayerMask draggingLayerMask;
	static GameObject buttonBG;
	static int selectedButtonID;
	static GameObject itemObjectCopy = null;
	Text amountText;
	public int price;
	Text priceText;
	public string itemType;
	
	void Start() {

		amountText = this.transform.Find("AmountText").gameObject.GetComponent("Text") as Text;
		priceText = this.transform.Find("PriceText").gameObject.GetComponent("Text") as Text;
		UICamera = Camera.main.GetComponent<Camera>();
		tilemapcont = Camera.main.GetComponent<TileMapController>();
	}

	void Update() {

		amountText.text = "x" + (Money.money / price).ToString();
		priceText.text = "$" + price;

		if(selectedButtonID == this.gameObject.GetInstanceID()) {		
			if(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject () == false) {
				selectedButtonID = -1;
				StartCoroutine(folderGhostDragging());
			}
		}
	}

	//when this button is clicked
	public void OnPointerClick(PointerEventData data) {
		if(selectedButtonID != this.gameObject.GetInstanceID()) {
			selectedButtonID = this.gameObject.GetInstanceID();
			selectButton(this.gameObject);
		}
	}

	public void selectButton(GameObject butt) {
		Destroy(buttonBG);
		
		//get bgButton from resources and child it to the itemList we're in
		buttonBG = Instantiate(Resources.Load("bgButton")) as GameObject;
		buttonBG.transform.SetParent(butt.transform.parent);
		RectTransform bgRect = buttonBG.GetComponent("RectTransform") as RectTransform;
		
		//set its position and scale to be slightly bigger than the button
		RectTransform thisRect = butt.GetComponent("RectTransform") as RectTransform;
		bgRect.anchoredPosition = new Vector2(thisRect.anchoredPosition.x, thisRect.anchoredPosition.y);
		bgRect.sizeDelta = new Vector2(thisRect.sizeDelta.x, thisRect.sizeDelta.y);
		
		
		//set its color
		Button buttonOfBG = buttonBG.GetComponent("Button") as Button;
		buttonOfBG.image.color = Color.yellow;
		
		//make it so it's just an outline
		buttonOfBG.image.fillCenter = false;
	}

	IEnumerator folderGhostDragging() { 
		string prefabLocation = "LevelEditor/" + connectedPrefab;


		//for the ghost-duplicate
		itemObjectCopy = null;
		
		bool cancellingMove = false;
		bool copyCreated = false;
		newp = new Vector3(0f, 0f, 0f);
		bool doorRotated = false;
		Vector3 doorWallRot = new Vector3(0f, 0f, 0f);
		
		while(!Input.GetMouseButton(0)) { 

			//if we haven't made a copy of the object yet
			if(!copyCreated) {
				//create copy of item object
				itemObjectCopy = Instantiate(Resources.Load(prefabLocation)) as GameObject;
				itemObjectCopy.name = "Copy";
				itemObjectCopy.GetComponent<ClickEvent>().enabled = false;
				//update the item object things
				//shader has to be set in this loop, or transparency won't work
				Color trans;
				//update the item object things
				//shader has to be set in this loop, or transparency won't work
				//itemObjectCopy.gameObject.GetComponentInChildren<Renderer>().material.shader = focusedShader;
				if(itemObjectCopy.GetComponentsInChildren<Renderer>() != null){
					foreach(Renderer rend in itemObjectCopy.GetComponentsInChildren<Renderer>()) {
						foreach(Material mat in rend.materials) {
							if(mat.HasProperty("_Color")){
								trans = mat.color;
								trans.a = .5f;
								mat.color = trans;
							}
						}
					}
				}

				//copy = itemObjectCopy.GetComponent ("ItemObject") as ItemObject;
				
				//so this code only happens once
				copyCreated = true;
			}
			
			//if user wants to cancel the drag
			if(Input.GetKeyDown(KeyCode.Escape)) {
				Destroy(itemObjectCopy);
				cancellingMove = true;
				
				//break out of while loop
				break;
			}
			
			Ray ray = UICamera.ScreenPointToRay(Input.mousePosition);
			float distance;
			Global.ground.Raycast(ray, out distance);

			int x = Mathf.RoundToInt(ray.GetPoint(distance).x);
			int z = Mathf.RoundToInt(ray.GetPoint(distance).z);

			//postion holder for inside this loop
			Vector3 movePos = new Vector3(x, 0f, z);

			//for final placement of object, after we break from this loop
			newp = movePos;

			//if copy exists
			if(copyCreated) {

				//if we got a door and it's on the edge of a room
				if(itemObjectCopy.GetComponent<SceneryData>() != null
					&& itemObjectCopy.GetComponent<SceneryData>().isDoor
					&& MapData.TheFarRooms.find(movePos) != null
						   ) {
					//snap door to an edge if it's near it
					if((MapData.TheFarRooms.find(movePos).isCloseToEdge(movePos, 3f))) {
						movePos = MapData.TheFarRooms.find(movePos).getNearestEdgePosition(movePos);
					}

					//set its new rotation
					if(MapData.TheFarRooms.find(movePos).isEdge(movePos)) {
						doorRotated = true;
						doorWallRot = MapData.TheFarRooms.find(movePos).getWallSide(movePos).toRotationVector();
					}

				}

				//if it's a door, set it to last wall rotation
				if(doorRotated == true) {
					itemObjectCopy.transform.eulerAngles = doorWallRot;
				}

				itemObjectCopy.transform.position = movePos;
			}

			yield return null; 
		}
		

		
		//if move was cancelled, we don't perform an update on the item object's position
		if(cancellingMove == true) {
			Destroy(buttonBG);
//			destroyButtonBG();
			selectedButtonID = -1;
		} else {

			Vector3 pos = newp;
			Vector3 rot = new Vector3(0f, 0f, 0f);

			//don't place item if we've click a button
			if(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false) {
				SceneryData sdat = itemObjectCopy.GetComponent<SceneryData>();
				if(sdat != null && sdat.isDoor) {
					ARTFRoom rm = MapData.TheFarRooms.find(pos);
					if(rm != null && rm.isCloseToEdge(pos, 3f)) {
						pos = MapData.TheFarRooms.find(pos).getNearestEdgePosition(pos);
					}
				}
				if(Money.buy(itemType, price)){
					MapData.addObject(prefabLocation, pos, rot.toDirection());
				}
			}

			//destroy the copy
			Destroy(itemObjectCopy);
			itemObjectCopy = null;

			Destroy(buttonBG);
//			destroyButtonBG();
			selectedButtonID = -1;
		
		}

		tilemapcont.suppressDragSelecting = false;
	}

	public void setConnectedPrefab(string s) {
		connectedPrefab = s;
	}

	public void setButtonImage(string icon) {
		Image im = this.GetComponent("Image") as Image;
		Sprite sp = Resources.Load <Sprite>("IconsUI/" + icon);
		im.sprite = sp;
	}



	//below: bunch of stuff from when buttons were dragged from folders

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

//	IEnumerator dragIt ()
//	{ 
//		while (Input.GetMouseButton(0)) {
//			draggedImageAnchor.transform.position = Input.mousePosition;
//
//			yield return null; 
//		}
//
//		
//	}
}
