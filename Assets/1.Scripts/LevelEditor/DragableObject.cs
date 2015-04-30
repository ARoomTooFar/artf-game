using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class DragableObject : ClickEvent {

	public LayerMask draggingLayerMask;
	Camera UICamera;
	TileMapController tilemapcont;

	GameObject tt;

	void OnMouseEnter(){
		if(this.gameObject.name != "Copy"){
			if(tt == null && gameObject.name == "CackleBranch(Clone)"){
				instantiateToolTip("Cackle Branch");
			} else if(gameObject.name == "FoliantFodder(Clone)"){
				instantiateToolTip("Foliant Fodder");
			} else if(gameObject.name == "Artilitree(Clone)"){
				instantiateToolTip("Artilitree");
			}
		}
	}

	void OnMouseExit(){
		Destroy(tt);
	}

	void Update () {
		makeToolTipFollowMouse();
	}

	void makeToolTipFollowMouse(){
		if (tt != null){
			Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			tt.transform.position = screenPos;
		}
	}
	
	void instantiateToolTip(string s){
		tt = Instantiate (Resources.Load ("ScreenUI/ToolTip")) as GameObject;
		tt.transform.SetParent(GameObject.Find("ScreenUI").transform);
		Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		tt.transform.position = screenPos;
		Text t = tt.transform.Find("Text").GetComponent<Text>() as Text;
		t.text = s;
	}
	
	void Start() {
		draggingLayerMask = LayerMask.GetMask("Walls");
		UICamera = Camera.main;
		tilemapcont = Camera.main.GetComponent<TileMapController>();
	}
		
	public override IEnumerator onClick(Vector3 initPosition) {
		if(!Mode.isTileMode()) {
			return false;
		}

		//for the ghost-duplicate
		GameObject itemObjectCopy = null;
		Vector3 position = this.gameObject.transform.position;
		tilemapcont.suppressDragSelecting = true;
		Ray ray;
		Vector3 mouseChange; 
		while(Input.GetMouseButton(0)) { 
			//if user wants to cancel the drag
			if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButton(1)) {
				Destroy(itemObjectCopy);
				return false;
			}
			
			ray = UICamera.ScreenPointToRay(Input.mousePosition);
			float distance;
			Global.ground.Raycast(ray, out distance);
			
			mouseChange = initPosition - Input.mousePosition;

			position = ray.GetPoint(distance).Round();

			//if mouse left deadzone
			if(Math.Abs(mouseChange.x) > Global.mouseDeadZone 
				|| Math.Abs(mouseChange.y) > Global.mouseDeadZone 
				|| Math.Abs(mouseChange.z) > Global.mouseDeadZone) {
						
				if(itemObjectCopy == null) {
					//create copy of item object
					itemObjectCopy = Instantiate(this.gameObject, getPosition(), getRotation()) as GameObject;

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
				} else {
					itemObjectCopy.transform.position = position;
					itemObjectCopy.transform.rotation = getRotation();
				}

				//for now y-pos remains as prefab's default.

			}
			yield return null; 
		}
		
		tilemapcont.suppressDragSelecting = false;
		
		//destroy the copy
		Destroy(itemObjectCopy);
		tilemapcont.deselect(getPosition());
		MapData.dragObject(this.gameObject, getPosition(), position - getPosition());
		tilemapcont.selectTile(position);
	}

	public Vector3 getPosition() {
		return this.gameObject.transform.root.position;
	}
	
	public Quaternion getRotation() {
		return this.gameObject.transform.rotation;
	}
}
