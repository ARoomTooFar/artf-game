using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Input_ItemObjectUI : MonoBehaviour {
	public Output_ItemObjectUI output_itemObjectUI;
	public Output_ItemObject output_itemObject;
	public GameObject buttons;

	Button Button_UpArrow_Attack;
	Button Button_DownArrow_Attack;

	Button Button_UpArrow_Health;
	Button Button_DownArrow_Health;

	Button Button_UpArrow_Armor;
	Button Button_DownArrow_Armor;

	Button Button_Rotate;

	Camera UICamera;

	Vector3 mouseStartPos = new Vector3(0,0,0);

	void Start () {
//		Button_UpArrow_Attack = transform.Find("ToggledItems/MonsterUpgrade/Attack/Button_UpArrow_Attack").GetComponent("Button") as Button;
		Button_UpArrow_Attack = transform.Find("Buttons/Button_UpArrow_Attack").GetComponent("Button") as Button;
		Button_DownArrow_Attack = transform.Find("Buttons/Button_DownArrow_Attack").GetComponent("Button") as Button;
		Button_UpArrow_Health = transform.Find("Buttons/Button_UpArrow_Health").GetComponent("Button") as Button;
		Button_DownArrow_Health = transform.Find("Buttons/Button_DownArrow_Health").GetComponent("Button") as Button;
		Button_UpArrow_Armor = transform.Find("Buttons/Button_UpArrow_Armor").GetComponent("Button") as Button;
		Button_DownArrow_Armor = transform.Find("Buttons/Button_UpArrow_Attack").GetComponent("Button") as Button;

		Button_Rotate = transform.Find("Buttons/Button_Rotate").GetComponent("Button") as Button;


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

		UICamera = GameObject.Find("UICamera").camera;
	}


	void Update(){
		RaycastHit hit;
		bool rayHit = false;
		
		//this checks if the object this script applies to was clicked
		if (Input.GetMouseButtonDown (0)) {
			Physics.Raycast(UICamera.ScreenPointToRay(Input.mousePosition), out hit);
			if (hit.collider && (hit.collider.gameObject.GetInstanceID() == output_itemObject.getGameObject().GetInstanceID())){
				rayHit = true;
				mouseStartPos = Input.mousePosition;
			}
		}
		
		//this is to check if a drag has been performed.
		//if a drag has been performed, the game will register it
		//as a click. we have to prevent this by checking if the mouse
		//has moved since the last mouseButtonDown. if it has, it means
		//we're dragging, and so we choose not to toggle the object UI.
		if (Input.GetMouseButtonUp(0) && rayHit == true){ 
			Vector3 offset = Input.mousePosition - mouseStartPos;
			
			//if the offset is not zero, then a drag happened
			if (Math.Abs(offset.x) == 0){
//				toggle.SetActive (!toggle.activeSelf);
				toggleItemObjectUI();
			}
			rayHit = false;
		}
	}

	public void toggleItemObjectUI(){
//		GameObject buttons = transform.Find("Buttons") as GameObject;
		buttons.SetActive(!buttons.activeSelf);
	}


}
