using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class Event_ItemButtons : MonoBehaviour, IPointerClickHandler {
	TileMapController tilemapcont;
	static Camera UICamera;
	string connectedPrefab = "";
	Vector3 newp;
	static GameObject buttonBG;
	static int selectedButtonID;
	static GameObject itemObjectCopy = null;
	Text amountText;
	public int price;
	Text priceText;
	public string itemType;
	
	void Start() {
		amountText = this.transform.Find("AmountText").gameObject.GetComponent<Text>();
		priceText = this.transform.Find("PriceText").gameObject.GetComponent<Text>();
		UICamera = Camera.main.GetComponent<Camera>();
		tilemapcont = Camera.main.GetComponent<TileMapController>();
	}

	void Update() {
		amountText.text = "x" + (Money.money / price).ToString();
		priceText.text = "$" + price;

		if(selectedButtonID == this.gameObject.GetInstanceID()) {		
			if(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false) {
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
		if(buttonBG == null) {
			buttonBG = Instantiate(Resources.Load("LevelEditor/Other/buttonHighlight")) as GameObject;
		}

		buttonBG.SetActive(true);
		buttonBG.transform.SetParent(butt.transform.parent);
		RectTransform thisRect = butt.GetComponent<RectTransform>();
		RectTransform bgRect = buttonBG.GetComponent<RectTransform>();
		//set its position to be slightly bigger than the button
		bgRect.anchoredPosition = thisRect.anchoredPosition;
	}

	IEnumerator folderGhostDragging() { 
		string prefabLocation = "LevelEditor/" + connectedPrefab;


		//for the ghost-duplicate
		itemObjectCopy = null;
		
		bool cancellingMove = false;
		bool copyCreated = false;
		newp = Vector3.zero;
		bool doorRotated = false;
		Vector3 doorWallRot = Vector3.zero;
		
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
				if(itemObjectCopy.GetComponentsInChildren<Renderer>() != null) {
					foreach(Renderer rend in itemObjectCopy.GetComponentsInChildren<Renderer>()) {
						foreach(Material mat in rend.materials) {
							if(mat.HasProperty("_Color")) {
								trans = mat.color;
								trans.a = .5f;
								mat.color = trans;
							}
						}
					}
				}
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

			//postion holder for inside this loop
			Vector3 movePos = UICamera.GetComponent<CameraRaycast>().mouseGroundPoint.Round();
			movePos.y = 0;

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
					if((MapData.TheFarRooms.find(movePos).isNearEdge(movePos, 3f))) {
						movePos = MapData.TheFarRooms.find(movePos).nearEdgePosition(movePos);
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
			//Destroy(buttonBG);
			buttonBG.SetActive(false);
			selectedButtonID = -1;
		} else {

			Vector3 pos = newp;
			Vector3 rot = new Vector3(0f, 0f, 0f);

			//don't place item if we've click a button
			if(UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false) {
				LevelEditorData dat = itemObjectCopy.GetComponent<LevelEditorData>();
				if(dat is RoomData) {
					if(MapData.addRoom(pos - new Vector3(3, 0, 3), pos + new Vector3(3, 0, 3))) {
						Money.buy(MapData.TheFarRooms.find(pos).Cost);
					}
				} else if(dat is LevelEntityData) {
					if(dat is SceneryData) {
						if(dat != null && (dat as SceneryData).isDoor) {
							ARTFRoom rm = MapData.TheFarRooms.find(pos);
							if(rm != null && rm.isNearEdge(pos, 3f)) {
								pos = MapData.TheFarRooms.find(pos).nearEdgePosition(pos);
							}
						}
					}
					if(MapData.addObject(prefabLocation, pos, rot.toDirection())) {
						Money.buy(price);
					}
				} 
			}

			//destroy the copy
			Destroy(itemObjectCopy);
			itemObjectCopy = null;

			//Destroy(buttonBG);
			buttonBG.SetActive(false);
			selectedButtonID = -1;
		
		}

		tilemapcont.suppressDragSelecting = false;
	}

	public void setConnectedPrefab(string s) {
		connectedPrefab = s;
	}

	public void setButtonImage(string icon) {
		Image im = this.GetComponent<Image>();
		Sprite sp = Resources.Load <Sprite>("LevelEditorIcons/" + icon);
		im.sprite = sp;
	}
}
