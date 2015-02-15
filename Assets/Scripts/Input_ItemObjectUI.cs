using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Input_ItemObjectUI : MonoBehaviour {
	Output_ItemObjectUI output_itemObjectUI;
	Output_ItemObject output_itemObject;
//	Select_Deselect_All select_Deselect_All;


	Button Button_UpArrow_Attack;
	Button Button_DownArrow_Attack;

	Button Button_UpArrow_Health;
	Button Button_DownArrow_Health;

	Button Button_UpArrow_Armor;
	Button Button_DownArrow_Armor;

	Button Button_Rotate;
	Button Screen_Button_Rotate;

//	Button Button_Select_All;

	Button Button_X;

	Camera UICamera;

	Vector3 mouseStartPos = new Vector3(0,0,0);

	bool rayHit = false;

	void Start () {
		Debug.Log("HERP");
		output_itemObject = transform.parent.GetComponent("Output_ItemObject") as Output_ItemObject;

//		Button_UpArrow_Attack = transform.Find("ToggledItems/MonsterUpgrade/Attack/Button_UpArrow_Attack").GetComponent("Button") as Button;
		Button_UpArrow_Attack = transform.Find("Buttons/Button_UpArrow_Attack").GetComponent("Button") as Button;
		Button_DownArrow_Attack = transform.Find("Buttons/Button_DownArrow_Attack").GetComponent("Button") as Button;
		Button_UpArrow_Health = transform.Find("Buttons/Button_UpArrow_Health").GetComponent("Button") as Button;
		Button_DownArrow_Health = transform.Find("Buttons/Button_DownArrow_Health").GetComponent("Button") as Button;
		Button_UpArrow_Armor = transform.Find("Buttons/Button_UpArrow_Armor").GetComponent("Button") as Button;
		Button_DownArrow_Armor = transform.Find("Buttons/Button_UpArrow_Attack").GetComponent("Button") as Button;

		Button_Rotate = transform.Find("Buttons/Button_Rotate").GetComponent("Button") as Button;
		Screen_Button_Rotate = GameObject.Find("Screen_Button_Rotate").GetComponent("Button") as Button;
	//	Button_Select_All = GameObject.Find ("Button_Select_All").GetComponent ("Button") as Button;

		Button_X = transform.Find("Buttons/Button_X").GetComponent("Button") as Button;

		UICamera = GameObject.Find("UICamera").camera;
		output_itemObjectUI = this.gameObject.GetComponent("Output_ItemObjectUI") as Output_ItemObjectUI;
//		select_Deselect_All = GameObject.Find ("ItemObjects").GetComponent ("Select_Deselect_All") as Select_Deselect_All;
		//Attack upgrade/downgrade listeners
		Button_UpArrow_Attack.onClick.AddListener (() => {
			output_itemObjectUI.increaseAttack (); });
		Button_DownArrow_Attack.onClick.AddListener (() => {
			output_itemObjectUI.decreaseAttack (); }); 
		
		//Health upgrade/downgrade listeners
		Button_UpArrow_Health.onClick.AddListener (() => {
			output_itemObjectUI.increaseHealth (); });
		Button_DownArrow_Health.onClick.AddListener (() => {
			output_itemObjectUI.decreaseHealth (); });
		
		//Armor upgrade/downgrade listeners
		Button_UpArrow_Armor.onClick.AddListener (() => {
			output_itemObjectUI.increaseArmor (); });
		Button_DownArrow_Armor.onClick.AddListener (() => {
			output_itemObjectUI.decreaseArmor (); });

		//rotate object button
		Button_Rotate.onClick.AddListener (() => {
			output_itemObject.rotate(90f); 
		});

		//X out
		Button_X.onClick.AddListener (() => {
			output_itemObjectUI.toggleItemObjectUI(); 
		});


		//Select All button
	//	Button_Select_All.onClick.AddListener (() => {
	//		select_Deselect_All.selectOrDeselectAll (); 
	//	});
		//screen button object rotate.
		//intended to rotate focused object
//		Screen_Button_Rotate.onClick.AddListener (() => {
//			output_itemObject.rotate(90f);
//		});


	}


	void Update(){
		RaycastHit hit;

		
		//this checks if the object this script applies to was clicked
		if (Input.GetMouseButtonDown (0)) {
			Physics.Raycast(UICamera.ScreenPointToRay(Input.mousePosition), out hit);
			if (hit.collider && (hit.collider.gameObject.GetInstanceID() == output_itemObject.getGameObject().GetInstanceID())){
				rayHit = true;
				mouseStartPos = Input.mousePosition;
			}
		}
		
		//checks if a drag has been performed. drags count as clicks.
		//if drag, toggle UI to turn it to its original state
		if (Input.GetMouseButtonUp(0) && rayHit == true){ 
			Vector3 offset = Input.mousePosition - mouseStartPos;
			
			//if the offset is not zero, then a drag happened
			if (Math.Abs(offset.x) == 0){
//				toggle.SetActive (!toggle.activeSelf);
				output_itemObjectUI.toggleItemObjectUI();
			}
			rayHit = false;
		}
	}
}
