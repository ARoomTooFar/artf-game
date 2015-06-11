using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class NonMonsterUI : MonoBehaviour
{
	public GameObject parentObject;
	
	static NonMonsterUI currentActiveObjectUI = null;
	public Camera UICamera;
	public Camera FollowCamera;
	public Canvas NonMonsterUICanvas;
	
	public CameraRaycast camCast;
	public CameraDraws camDraw;
	
	GameObject lowerHalf;
	
	Vector3 mouseStartPos;
	
	bool rayHit = false;
	
	
	void Start ()
	{
		Button btn;
		btn = transform.Find("LowerHalf/Button_Rotate").GetComponent<Button>();
		btn.onClick.AddListener (() => {
			MapData.rotateObject(this.transform.parent.gameObject, this.transform.parent.gameObject.transform.position);
		});
		
		btn = transform.Find("LowerHalf/Button_X").GetComponent<Button>();
		btn.onClick.AddListener (() => {
			toggleNonMonsterUI(); 
		});
		
		parentObject = transform.parent.gameObject;

		NonMonsterUICanvas = this.gameObject.GetComponent<Canvas>();
		
		UICamera = GameObject.Find("UpgradeUICamera").GetComponent<Camera>();
		FollowCamera = GameObject.Find("UICamera").GetComponent<Camera>();
		
		camCast = FollowCamera.GetComponent<CameraRaycast>();
		camDraw = FollowCamera.GetComponent<CameraDraws>();
		
		//sets Render Camera if in Screen Space - Camera mode
		//sets World Camera if in World Space mode
		NonMonsterUICanvas.worldCamera = UICamera;
		
		//ui elements we need to toggle on click
		lowerHalf = transform.Find("LowerHalf").gameObject;
		
		//initialize UI to be invisible
		lowerHalf.SetActive(false);
		
		mouseStartPos = new Vector3(0,0,0);
	}
	
	//Update causes itemObjectUI flickering. LateUpdate prevents it
	void LateUpdate(){
		faceUIToCamera();
	}
	
	void scaleWorldSpaceCanvas(){
		float uiScaleFactor = FollowCamera.transform.position.y / 17f;
		
		NonMonsterUICanvas.GetComponent<RectTransform>().localScale = 
			new Vector3(uiScaleFactor, uiScaleFactor, 1f);
	}
	
	void stickScreenSpaceOverlayCameraToObject(){
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(FollowCamera, parentObject.transform.position);
		NonMonsterUICanvas.GetComponent<RectTransform>().anchoredPosition = screenPoint;
	}
	
	void Update(){
		
		//keeps canvas stuck on world object. for when canvas is in Screen Space - Overlay mode
		//		stickScreenSpaceOverlayCameraToObject();
		
		//resizes canvas according to camera zoom, for when canvas is in World Space mode
		scaleWorldSpaceCanvas();
		
		//this checks if the object this script applies to was clicked
		if (Input.GetMouseButtonDown (0)) {
			if(camCast.hitDistance != Mathf.Infinity
			   && camCast.hit.collider.gameObject.GetInstanceID() == parentObject.gameObject.GetInstanceID()){
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
				toggleNonMonsterUI();
			}
			rayHit = false;
		}
	}
	
	void faceUIToCamera(){
		NonMonsterUICanvas.transform.position = parentObject.transform.position;
		NonMonsterUICanvas.transform.rotation = UICamera.transform.rotation;
	}

	//makes sure only one ObjectUI is active in the level editor
	public bool toggleNonMonsterUI(){
		if(currentActiveObjectUI == null){
			lowerHalf.SetActive(true);
			currentActiveObjectUI = this.gameObject.GetComponent<NonMonsterUI>();
		}else if(currentActiveObjectUI == this.gameObject.GetComponent<NonMonsterUI>()){
			lowerHalf.SetActive(false);
			currentActiveObjectUI = null;
		}else{
			currentActiveObjectUI.toggleNonMonsterUI();
			lowerHalf.SetActive(true);
			currentActiveObjectUI = this.gameObject.GetComponent<NonMonsterUI>();
		}
		
		//if already active return false, otherwise return true (used for select/deselect all)
		if (!lowerHalf.activeSelf)
			return false;
		else 
			return true;
	}
}
