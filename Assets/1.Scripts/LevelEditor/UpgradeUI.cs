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

	static UpgradeUI currentActiveObjectUI = null;

	int tier = 0;
	int maxTier = 6;
	Text Text_Tier;
	Text Text_AttackPatterns;
	
	public Camera UICamera;
	public Camera FollowCamera;
	public Canvas UpgradeUICanvas;
	
	GameObject lowerHalf;
	GameObject tierStats;

	Button Button_TierDown;
	Button Button_TierUp;
	Button Button_Rotate;
	
	Button Button_X;
	
	Vector3 mouseStartPos;
	
	bool rayHit = false;

	void Start ()
	{
		Button_TierUp = transform.Find("LowerHalf/Button_TierUp").GetComponent<Button>();
		Button_TierDown = transform.Find("LowerHalf/Button_TierDown").GetComponent<Button>();
		Button_TierUp.onClick.AddListener (() => {
			increaseTier (); });
		Button_TierDown.onClick.AddListener (() => {
			decreaseTier (); }); 
		Button_Rotate = transform.Find("LowerHalf/Button_Rotate").GetComponent<Button>();
		Button_X = transform.Find("LowerHalf/Button_X").GetComponent<Button>();
		//rotate object button
		Button_Rotate.onClick.AddListener (() => {
			MapData.rotateObject(this.transform.parent.gameObject, this.transform.parent.gameObject.transform.position);
		});
		//X out
		Button_X.onClick.AddListener (() => {
			toggleUpgradeUI(); 
		});

		parentObject = transform.parent.gameObject;

		Text_Tier = transform.Find("LowerHalf/Text_Tier").GetComponent<Text>();
		updateMonsterStatText ();
		
		UpgradeUICanvas = this.gameObject.GetComponent<Canvas>();

		UICamera = GameObject.Find("UpgradeUICamera").GetComponent<Camera>();
		FollowCamera = GameObject.Find("UICamera").GetComponent<Camera>();

		//sets Render Camera if in Screen Space - Camera mode
		//sets World Camera if in World Space mode
		UpgradeUICanvas.worldCamera = UICamera;
		
		//ui elements we need to toggle on click
		lowerHalf = transform.Find("LowerHalf").gameObject;
		tierStats = transform.Find("TierStats").gameObject;
		
		//initialize UI to be invisible
		lowerHalf.SetActive(false);
		tierStats.SetActive(false);

		mouseStartPos = new Vector3(0,0,0);
	}
	
	//Update causes itemObjectUI flickering. LateUpdate prevents it
	void LateUpdate(){
		faceUIToCamera();

		if(FollowCamera.orthographic == true){
			print("ASdfadF");
			lowerHalf.SetActive(false);
			tierStats.SetActive(false);
			currentActiveObjectUI = null;
		}
	}

	//turn tiers gray that aren't applied. turn tiers green that are applied
	void updateTiers(){
		for (int i = tier; i <= maxTier; i++) {
			foreach (Transform thing in tierStats.transform) {
				if (thing.GetComponent<Text> () != null && thing.name != "Title" && thing.name.Contains ("Tier" + i.ToString ())) {
					thing.GetComponent<Text> ().color = Color.gray;
				}
			}
		}

		for (int i = tier; i >= 0; i--) {

			foreach (Transform thing in tierStats.transform) {
				if (thing.name.Contains ("Tier" + i.ToString ())) {
					thing.GetComponent<Text> ().color = Color.green;
				} 
			}
		}
	}

	void scaleWorldSpaceCanvas(){
		float uiScaleFactor = FollowCamera.transform.position.y / 17f;

		UpgradeUICanvas.GetComponent<RectTransform>().localScale = 
			new Vector3(uiScaleFactor, uiScaleFactor, 1f);
	}

	void stickScreenSpaceOverlayCameraToObject(){
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(FollowCamera, parentObject.transform.position);
		UpgradeUICanvas.GetComponent<RectTransform>().anchoredPosition = screenPoint;

//		Vector3 screenPoint = UICamera.WorldToViewportPoint(parentObject.transform.position);
//		UpgradeUICanvas.GetComponent<RectTransform>().anchoredPosition = screenPoint;
//		lowerHalf.transform.position = screenPoint;
		//Debug.Log(UpgradeUICanvas.GetComponent<RectTransform>().anchoredPosition + ", " + screenPoint);
	}

	void Update(){
		RaycastHit hit;

		updateTiers();

		//keeps canvas stuck on world object. for when canvas is in Screen Space - Overlay mode
		stickScreenSpaceOverlayCameraToObject();

		//resizes canvas according to camera zoom, for when canvas is in World Space mode
//		scaleWorldSpaceCanvas();

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
		UpgradeUICanvas.transform.position = p; 
		
		p = UICamera.transform.rotation.eulerAngles;
		UpgradeUICanvas.transform.rotation = Quaternion.Euler(p);
	}

	public void increaseTier ()
	{
		tier += 1;
		if (tier >= maxTier) {
			tier = maxTier;
		}
		updateMonsterStatText ();
		parentObject.GetComponent<MonsterData>().tier = tier;
	}
	
	public void decreaseTier ()
	{
		tier -= 1;
		if (tier <= 0) {
			tier = 0;
		}
		updateMonsterStatText ();
		parentObject.GetComponent<MonsterData>().tier = tier;
	}
	

	
	//Update the monster stat text with the newly changed value
	void updateMonsterStatText ()
	{
		Text_Tier.text = "Tier: " + tier.ToString ();
	}
	
	//makes sure only one ObjectUI is active in the level editor
	public bool toggleUpgradeUI(){



		if(currentActiveObjectUI == null){
			lowerHalf.SetActive(true);
			tierStats.SetActive(true);
			currentActiveObjectUI = this.gameObject.GetComponent<UpgradeUI>();
		}else if(currentActiveObjectUI == this.gameObject.GetComponent<UpgradeUI>()){
			lowerHalf.SetActive(false);
			tierStats.SetActive(false);
			currentActiveObjectUI = null;
		}else{
			currentActiveObjectUI.toggleUpgradeUI();
			lowerHalf.SetActive(true);
			tierStats.SetActive(true);
			currentActiveObjectUI = this.gameObject.GetComponent<UpgradeUI>();
		}

		//if already active return false, otherwise return true (used for select/deselect all)
		if (!lowerHalf.activeSelf)
			return false;
		else 
			return true;
	}
	
}
