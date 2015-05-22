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
		{"Button_Rooms", "Rooms Folder:Contains objects that assist you in building rooms, such as doors, individual wall tiles, and starter rooms."},
		{"Button_Misc", "Environment Folder:Contains scenery and decorative objects."},
		{"Button_Enemies", "Enemies Folder:Contains enemies and traps."},

		{"Shop", "Bank Account:This is the amount of money you have to spend on items."},

		{"BullyTrunk(Clone)", "Bully Trunk:The Bully Trunk is a kojrgk aerg kjefgo efgmskegt jker gkjnerkjgarlfg skefgjsekj" +
			"srkjgskejrg sdfjkgsejkrg nkjerngnsergnkejsrg ."},
		{"Bushling(Clone)", "Bushling:Bushling is a gjkds dfg js ksdfkj sdjkf jkdfg kdfg jker."},
		{"CackleBranch(Clone)", "Cackle Branch:The CackleBranch isf gjsdfk kjsfks kr ."},
		{"FoliantFodder(Clone)", "Foliant Fodder:The Foliant Fodder is fikdfgksdfgk sjkd fklsdf klasdfk askjf ."},
		{"FoliantHive(Clone)", "Foliant Hive:The Foliant Hive is fie kdwfgweksdfgk se  fklsdf klasdfk askjf ."},
		{"Mirage(Clone)", "Mirage:The Mirage is efwef wefwe err e  wewe wefwsjkd fwe kwe lwedf klasdfk askjf ."},

		{"folderButton(Clone)", "Mirage:The Mirage is efwef wefwe err e  wewe wefwsjkd fwe kwe lwedf klasdfk askjf ."},
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

			//if its a folder button, get it name from its sprite.
			//if its an object in the world, gets its name from its name.
			if(tipTextTable.ContainsKey(this.gameObject.name)){
				if(this.gameObject.name == "folderButton(Clone)"){
					string typeButtonRefersTo = this.gameObject.GetComponent<Image>().sprite.name + "(Clone)";

					if(tipTextTable.ContainsKey(typeButtonRefersTo)){
						setTipTextTextAndTipTextTitleFromTipTextTable(typeButtonRefersTo);
					}
				}else{
					setTipTextTextAndTipTextTitleFromTipTextTable(this.gameObject.name);
				}
			}

			//initialize the fields to be blank
			tipTitle.text = "";
			tipText.text = "";
		}
	}

	//sets the tip tet text and the tip text title using the tip text table
	void setTipTextTextAndTipTextTitleFromTipTextTable(string type){
		tipTitleText =
			tipTextTable[type].Substring(0, tipTextTable[type].IndexOf(":"));
		tipTextText =
			tipTextTable[type].Substring(tipTextTable[type].IndexOf(":") + 1);
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
			//resource pool objects don't get these, so they throw errors
			if(tipTitle != null && tipText != null){
				tipTitle.text = "";
				tipText.text = "";
			}
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

