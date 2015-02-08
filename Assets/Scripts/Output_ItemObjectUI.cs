﻿using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Output_ItemObjectUI : MonoBehaviour
{
	public Output_ItemObject output_itemObject;

	int attack = 1;
	int health = 1;
	int armor = 1;
	Text Text_Attack;
	Text Text_Health;
	Text Text_Armor;

	Camera UICamera;
	public Canvas itemObjectUICanvas;

	void Start ()
	{
		Text_Attack = transform.Find("Text/Text_Attack").GetComponent("Text") as Text;
		Text_Health = transform.Find("Text/Text_Health").GetComponent("Text") as Text;
		Text_Armor = transform.Find("Text/Text_Armor").GetComponent("Text") as Text;
		updateMonsterStatText ();

		itemObjectUICanvas = this.GetComponent("Canvas") as Canvas;
		UICamera = GameObject.Find("UICamera").camera;
		itemObjectUICanvas.worldCamera = UICamera;
	}

	void Update(){
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

}
