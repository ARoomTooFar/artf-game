using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Output_ItemObjectUI : MonoBehaviour
{
	Output_ItemObject output_itemObject;


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

	void Start ()
	{
		output_itemObject = transform.parent.GetComponent("Output_ItemObject") as Output_ItemObject;

		Text_Attack = transform.Find("Text/Text_Attack").GetComponent("Text") as Text;
		Text_Health = transform.Find("Text/Text_Health").GetComponent("Text") as Text;
		Text_Armor = transform.Find("Text/Text_Armor").GetComponent("Text") as Text;
		updateMonsterStatText ();

		itemObjectUICanvas = this.gameObject.GetComponent("Canvas") as Canvas;
		UICamera = GameObject.Find("UICamera").camera;
		itemObjectUICanvas.worldCamera = UICamera;

		//ui elements we need to toggle on click
		buttons = transform.Find("Buttons").gameObject;
		text = transform.Find("Text").gameObject;

		//initialize UI to be invisible
		buttons.SetActive(false);
		text.SetActive(false);

	}

	//Update causes itemObjectUI flickering
	//LateUpdate prevents it
	void LateUpdate(){
		faceUIToCamera();
	}

	void faceUIToCamera(){
		Vector3 p = new Vector3();

		p = output_itemObject.getPosition();
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

	public void toggleItemObjectUI(){
		buttons.SetActive(!buttons.activeSelf);
		text.SetActive(!text.activeSelf);

		//temporary
		output_itemObject.setToFocused();
	}

	//This will select or deselect all objects
	public void selectOrDeselectAll() {
		GameObject ObjectUI = gameObject;
		GameObject prefabd1 = ObjectUI.GetComponentInParent<Transform> ().gameObject;
		GameObject itemObjects = prefabd1.GetComponentInParent<Transform> ().gameObject;
		Transform[] allPrefabs = itemObjects.GetComponentsInChildren<Transform> ();
		foreach (Transform trans in allPrefabs) {
			GameObject obj = trans.gameObject;
			print (obj.GetType());
			Output_ItemObjectUI objui = obj.GetComponent<Output_ItemObjectUI>();
			objui.toggleItemObjectUI();
		}
		/*	GameObject obj = GameObject.Find ("ItemObjects") as GameObject;
		Transform[] children = obj.GetComponentsInChildren<Transform> ();
		//Component[] children = obj.GetComponentsInChildren ();
		//GameObject[] children = obj.GetComponentsInChildren ();
		Debug.Log ("stuff");
		foreach(Transform child in children) {
			GameObject childObj = child.gameObject;
			childObj.GetComponent<Output_ItemObjectUI>().toggleItemObjectUI();

		} 

	*/}

	/*
	 * for each child of ItemObjects
	 * 		get that objects "ObjectUI" child
	 * 		get that ObjectUI's component script
	 * 		use that script to toggle */
}
