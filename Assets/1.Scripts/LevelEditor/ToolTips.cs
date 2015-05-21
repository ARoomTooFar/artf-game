using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToolTips : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	//tool tips for everything.
	//Value is in the format title:description
	Dictionary<string, string> tipTextTable = new Dictionary<string, string>(){
		{"Button_Save", "Deploy Level:Deploys this level to your user account."},
		{"Button_ModeToggle", "Toggle Mode:Toggles between Room Mode and Tile Mode. " +
			"Room mode allows you to move and resize rooms. Tile Mode allows you to move and edit objects."},
		{"Button_ZoomOut", "Zoom Out:Zoom the camera out."},
		{"Button_ZoomIn", "Zoom In:Zoom the camera in."},
		{"Button_Delete", "Delete:Delete selected object (hotkey: delete)."},
		{"Button_CameraToggle", "Toggle Camera Mode:Toggle the camera between top-down mode and perspective mode."},
	};

	GameObject tt;
	String tipTextText;
	String tipTitleText;
	Text tipText;
	Text tipTitle;

	//are we using tooltips that follow mouse, or tooltips that show up in window
	public bool usingMouseFollowToolTip = false;
	
	void Start() {

		if(usingMouseFollowToolTip){
			tt = Instantiate(Resources.Load("ScreenUI/ToolTip")) as GameObject;
			tt.transform.SetParent(GameObject.Find("ScreenUI").transform);
			Text t = tt.transform.Find("Panel/Text").GetComponent<Text>();
			t.text = tipTextText;
			tt.SetActive(false);
		}else{
			tipText = GameObject.Find("ScreenToolTipText").GetComponent<Text>();
			tipTitle = GameObject.Find("ScreenToolTipTitle").GetComponent<Text>();

			if(tipTextTable.ContainsKey(this.gameObject.name)){
				tipTitleText =
					tipTextTable[this.gameObject.name].Substring(0, tipTextTable[this.gameObject.name].IndexOf(":"));
				tipTextText =
					tipTextTable[this.gameObject.name].Substring(tipTextTable[this.gameObject.name].IndexOf(":") + 1);
			}
		}
	}


	
	void Update() {
		if(usingMouseFollowToolTip){
			if(tt.activeSelf) {
				makeToolTipFollowMouse();
			}
		}
	}

	void OnDisable() {
		OnMouseExit();
	}

	void OnMouseEnter() {
		if(usingMouseFollowToolTip){
			if(gameObject.name == "Copy") {
				return;
			}
			tt.SetActive(true);
		}else{
			tipText.text = tipTextText;
			tipTitle.text = tipTitleText;
		}

	}

	void OnMouseExit() {
		if(usingMouseFollowToolTip){
			tt.SetActive(false);
		}else{
			tipTitle.text = "";
			tipText.text = "";
		}
	}

	public void OnPointerEnter(PointerEventData data) {
		OnMouseEnter();
	}
	
	public void OnPointerExit(PointerEventData data) {
		OnMouseExit();
	}
	
	void makeToolTipFollowMouse() {
		Vector2 screenPos = new Vector2(Input.mousePosition.x+5, Input.mousePosition.y+5);
		tt.transform.position = screenPos;
	}
}

