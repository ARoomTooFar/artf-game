using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.IO;

//This class listens to UI elements that are on canvases attached to objects
//in the world space
public class UIHandler_ObjectUI : MonoBehaviour
{
	//all the dang buttons
//	public Slider slider = null;
//	public InputField inputField = null;

	public TransformHandler_Object transformHandler_object;

	public Text attackText = null;
	public Button Button_UpArrow_Attack = null;
	public Button Button_DownArrow_Attack = null;

	public Text healthText = null;
	public Button Button_UpArrow_Health = null;
	public Button Button_DownArrow_Health = null;

	public Text armorText = null;
	public Button Button_UpArrow_Armor = null;
	public Button Button_DownArrow_Armor = null;

	//these should eventually be drawn from Monster/Entity class
	private int attack = 1;
	private int health = 1;
	private int armor = 1;
	
	public Button Button_Rotate = null; //holds the rotate button arrow


	void Start ()
	{
		updateMonsterStatText ();

		//Slider listener
		//when the slider is moved, call sliderAction()
//		slider.onValueChanged.AddListener (this.sliderAction); 


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



		//setup the listener for when the rotation button is clicked
		Button_Rotate.onClick.AddListener (() => {
			transformHandler_object.rotateObject(90f); 
		});

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
		attackText.text = "Attack: " + attack.ToString ();
		healthText.text = "Health: " + health.ToString ();
		armorText.text = "Armor: " + armor.ToString ();
	}
}
