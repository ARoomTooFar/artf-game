using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ItemObjectUI : MonoBehaviour
{
	ItemObject itemob;
	
	int attack = 1;
	int health = 1;
	int armor = 1;
	Text Text_Attack;
	Text Text_Health;
	Text Text_Armor;
	
	Camera UICamera;
	Canvas itemObjectUICanvas;
	
	GameObject buttons;
	GameObject text;
	
	Button Button_UpArrow_Attack;
	Button Button_DownArrow_Attack;
	
	Button Button_UpArrow_Health;
	Button Button_DownArrow_Health;
	
	Button Button_UpArrow_Armor;
	Button Button_DownArrow_Armor;
	
	Button Button_Rotate;
	
	Button Button_X;
	
	Vector3 mouseStartPos;
	
	bool rayHit = false;
	
	void Start ()
	{
		itemob = transform.parent.GetComponent("ItemObject") as ItemObject;
		
		Text_Attack = transform.Find("Text/Text_Attack").GetComponent("Text") as Text;
		Text_Health = transform.Find("Text/Text_Health").GetComponent("Text") as Text;
		Text_Armor = transform.Find("Text/Text_Armor").GetComponent("Text") as Text;
		updateMonsterStatText ();
		
		itemObjectUICanvas = this.gameObject.GetComponent("Canvas") as Canvas;
		UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		itemObjectUICanvas.worldCamera = UICamera;
		
		//ui elements we need to toggle on click
		buttons = transform.Find("Buttons").gameObject;
		text = transform.Find("Text").gameObject;
		
		//initialize UI to be invisible
		buttons.SetActive(false);
		text.SetActive(false);


		mouseStartPos = new Vector3(0,0,0);

		Button_UpArrow_Attack = transform.Find("Buttons/Button_UpArrow_Attack").GetComponent("Button") as Button;
		Button_DownArrow_Attack = transform.Find("Buttons/Button_DownArrow_Attack").GetComponent("Button") as Button;
		Button_UpArrow_Health = transform.Find("Buttons/Button_UpArrow_Health").GetComponent("Button") as Button;
		Button_DownArrow_Health = transform.Find("Buttons/Button_DownArrow_Health").GetComponent("Button") as Button;
		Button_UpArrow_Armor = transform.Find("Buttons/Button_UpArrow_Armor").GetComponent("Button") as Button;
		Button_DownArrow_Armor = transform.Find("Buttons/Button_UpArrow_Attack").GetComponent("Button") as Button;
		
		Button_Rotate = transform.Find("Buttons/Button_Rotate").GetComponent("Button") as Button;
		
		Button_X = transform.Find("Buttons/Button_X").GetComponent("Button") as Button;
		
		UICamera = GameObject.Find("UICamera").GetComponent<Camera>();

		//Attack upgrade/downgrade listeners
		Button_UpArrow_Attack.onClick.AddListener (() => {
			increaseAttack (); });
		Button_DownArrow_Attack.onClick.AddListener (() => {
			decreaseAttack (); }); 
		
		//Health upgrade/downgrade listeners
		Button_UpArrow_Health.onClick.AddListener (() => {
			increaseHealth (); });
		Button_DownArrow_Health.onClick.AddListener (() => {
			decreaseHealth (); });
		
		//Armor upgrade/downgrade listeners
		Button_UpArrow_Armor.onClick.AddListener (() => {
			increaseArmor (); });
		Button_DownArrow_Armor.onClick.AddListener (() => {
			decreaseArmor (); });
		
		//rotate object button
		Button_Rotate.onClick.AddListener (() => {
			MapData.rotateObject(this.transform.parent.gameObject, this.transform.parent.gameObject.transform.position);
		});
		
		//X out
		Button_X.onClick.AddListener (() => {
			toggleItemObjectUI(); 
		});
		
	}
	
	//Update causes itemObjectUI flickering
	//LateUpdate prevents it
	void LateUpdate(){
		faceUIToCamera();
	}

	void Update(){
		RaycastHit hit;
		
		
		//this checks if the object this script applies to was clicked
		if (Input.GetMouseButtonDown (0)) {
			Physics.Raycast(UICamera.ScreenPointToRay(Input.mousePosition), out hit);
			if (hit.collider && (hit.collider.gameObject.GetInstanceID() == itemob.getGameObject().GetInstanceID())){
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
				toggleItemObjectUI();
			}
			rayHit = false;
		}
	}
	
	void faceUIToCamera(){
		Vector3 p = new Vector3();
		
		p = itemob.getPosition();
		itemObjectUICanvas.transform.position = p; 
		
		p = UICamera.transform.rotation.eulerAngles;
		itemObjectUICanvas.transform.rotation = Quaternion.Euler(p);
		
	}
	
	
	//
	//Attack
	//
	public void increaseAttack ()
	{
		attack += 1;
		updateMonsterStatText ();
	}
	
	public void decreaseAttack ()
	{
		attack -= 1;
		if (attack <= 1) {
			attack = 1;
		}
		updateMonsterStatText ();
	}
	
	
	//
	//Health
	//
	public void increaseHealth ()
	{
		health += 1;
		updateMonsterStatText ();
	}
	
	public void decreaseHealth ()
	{
		health -= 1;
		if (health <= 1) {
			health = 1;
		}
		updateMonsterStatText ();
	}
	
	
	//
	//Armor
	//
	public void increaseArmor ()
	{
		armor += 1;
		updateMonsterStatText ();
	}
	
	public void decreaseArmor ()
	{
		armor -= 1;
		if (armor <= 1) {
			armor = 1;
		}
		updateMonsterStatText ();
	}
	
	
	//Update the monster stat text with the newly changed value
	void updateMonsterStatText ()
	{
		Text_Attack.text = "Attack: " + attack.ToString ();
		Text_Health.text = "Health: " + health.ToString ();
		Text_Armor.text = "Armor: " + armor.ToString ();
	}
	
	public bool toggleItemObjectUI(){
		buttons.SetActive(!buttons.activeSelf);
		text.SetActive(!text.activeSelf);
		
		//if already active return false, otherwise return true (used for select/deselect all)
		if (!buttons.activeSelf)
			return false;
		else 
			return true;
	}
	
}
