using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UpgradeUI : MonoBehaviour
{
	public GameObject parentObject;

	int tier = 1;
	Text Text_Tier;
	Text Text_AttackPatterns;
	
	public Camera UICamera;
	public Camera FollowCamera;
	public Canvas UpgradeUICanvas;
	
	GameObject buttons;
	GameObject text;
	GameObject tierStats;

	Button Button_TierDown;
	Button Button_TierUp;
	Button Button_Rotate;
	
	Button Button_X;
	
	Vector3 mouseStartPos;
	
	bool rayHit = false;

//	RectTransform TierStats;
	
	void Start ()
	{
//		TierStats = GameObject.Find("TierStats").GetComponent<RectTransform>();

		parentObject = transform.parent.gameObject;
		Text_Tier = transform.Find("Text/Text_Tier").GetComponent("Text") as Text;
//		Text_AttackPatterns = transform.Find("Text/Text_AttackPatterns").GetComponent("Text") as Text;
		updateMonsterStatText ();
		
		UpgradeUICanvas = this.gameObject.GetComponent("Canvas") as Canvas;
		UICamera = GameObject.Find("UpgradeUICamera").GetComponent<Camera>();
		FollowCamera = GameObject.Find("UICamera").GetComponent<Camera>();
		UpgradeUICanvas.worldCamera = UICamera;
		
		//ui elements we need to toggle on click
		buttons = transform.Find("Buttons").gameObject;
		text = transform.Find("Text").gameObject;
		tierStats = transform.Find("TierStats").gameObject;
		
		//initialize UI to be invisible
		buttons.SetActive(false);
		text.SetActive(false);
		tierStats.SetActive(false);

		mouseStartPos = new Vector3(0,0,0);

		Button_TierUp = transform.Find("Buttons/Button_TierUp").GetComponent("Button") as Button;
		Button_TierDown = transform.Find("Buttons/Button_TierDown").GetComponent("Button") as Button;
		Button_TierUp.onClick.AddListener (() => {
			increaseTier (); });
		Button_TierDown.onClick.AddListener (() => {
			decreaseTier (); }); 


		Button_Rotate = transform.Find("Buttons/Button_Rotate").GetComponent("Button") as Button;
		
		Button_X = transform.Find("Buttons/Button_X").GetComponent("Button") as Button;

	
		//rotate object button
		Button_Rotate.onClick.AddListener (() => {
			MapData.rotateObject(this.transform.parent.gameObject, this.transform.parent.gameObject.transform.position);
		});
		
		//X out
		Button_X.onClick.AddListener (() => {
			toggleUpgradeUI(); 
		});
		
	}

//	void buildTierStatsWindow(){
//		TierStats.anchoredPosition = new Vector2(82f, 72f);
//		GameObject titleTextObject = Instantiate (Resources.Load ("ObjectUI/StatTitle")) as GameObject;
//		RectTransform r = titleTextObject.GetComponent<RectTransform>();
//		r.anchoredPosition = new Vector2(82f, 72f);
//	}
	
	//Update causes itemObjectUI flickering
	//LateUpdate prevents it
	void LateUpdate(){
//		faceUIToCamera();
	}

	void Update(){
		RaycastHit hit;

		//keeps canvas stuck on world object
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(FollowCamera, parentObject.transform.position);
		UpgradeUICanvas.GetComponent<RectTransform>().anchoredPosition = screenPoint;
		Debug.Log(screenPoint + " : " + UpgradeUICanvas.GetComponent<RectTransform>().anchoredPosition);

		//this checks if the object this script applies to was clicked
		if (Input.GetMouseButtonDown (0)) {
			Physics.Raycast(UICamera.ScreenPointToRay(Input.mousePosition), out hit);
			if (hit.collider && (hit.collider.gameObject.GetInstanceID() == parentObject.gameObject.GetInstanceID())){
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
				toggleUpgradeUI();
			}
			rayHit = false;
		}
	}
	
	void faceUIToCamera(){
		Vector3 p = new Vector3();

		p = parentObject.transform.position;
//		p = itemob.getPosition();
		UpgradeUICanvas.transform.position = p; 
		
		p = UICamera.transform.rotation.eulerAngles;
		UpgradeUICanvas.transform.rotation = Quaternion.Euler(p);
		
	}
	


	public void increaseTier ()
	{
		tier += 1;
		updateMonsterStatText ();
	}
	
	public void decreaseTier ()
	{
		tier -= 1;
		if (tier <= 1) {
			tier = 1;
		}
		updateMonsterStatText ();
	}
	

	
	//Update the monster stat text with the newly changed value
	void updateMonsterStatText ()
	{
		Text_Tier.text = "Tier: " + tier.ToString ();
//		Text_AttackPatterns.text = "Tier:\n get ";
	}
	
	public bool toggleUpgradeUI(){
		buttons.SetActive(!buttons.activeSelf);
		text.SetActive(!text.activeSelf);
		tierStats.SetActive(!tierStats.activeSelf);
		
		//if already active return false, otherwise return true (used for select/deselect all)
		if (!buttons.activeSelf)
			return false;
		else 
			return true;
	}
	
}
