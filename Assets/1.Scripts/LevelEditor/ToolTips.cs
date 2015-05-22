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

		{"BullyTrunk(Clone)", "Bully Trunk:A beast of wood and steal. Charges delvers before pummling them into the ground."},
		{"Bushling(Clone)", "Bushling:An agile creature of vine, will use whatever means necessary to slice delvers appart. "},
		{"CackleBranch(Clone)", "Cackle Branch:A living shrub. With a gun and a bad attitude."},
		{"FoliantFodder(Clone)", "Foliant Fodder:A small, swarming bug, that craves human flesh."},
		{"FoliantHive(Clone)", "Foliant Hive:The favored nest of the Folaint Fodder, will launch endless waves of Foliant Fodder at unsuspecting delvers."},
		{"Mirage(Clone)", "Mirage:A manifestation of pure malice and cunning. Creates mirror images of itself to confuse and disrupt player. Strikes with deadly power."},
		{"Artilitree(Clone)", "Artilitree:Our most advanced weaponry, turned back against it's creators. Mostly stationary, launches barages of deadly gases."},
		{"Synth(Clone)", "Synth:A robotic husk, now a slave to the jungle. Will send delvers running under a hail of bullets."},
		{"DartPlant(Clone)", "DartPlant:A friendly flower, with a bite."},
		{"SpikeTrap(Clone)", "SpikeTrap:Stabs you."},
		{"StunTrap(Clone)", "StunTrap:Stuns you."},
		{"SlowField(Clone)", "SlowField:Slows you."},

		{"treeAndLamp(Clone)", "Tree and Lamp:A tree with a lamp."},
		{"TrashCan(Clone)", "Trash Can:A container for household refuse."},
		{"plantlight(Clone)", "Plant Light:A plant with a light."},

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

	//sets the tip text text and the tip text title using the tip text table
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

