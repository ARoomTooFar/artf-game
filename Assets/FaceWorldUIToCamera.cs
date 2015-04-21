using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FaceWorldUIToCamera : MonoBehaviour {
	public GameObject parentObject;
	public GameObject low;

	void Update(){
		stickScreenSpaceOverlayCameraToObject();
	}

	void Start(){
		parentObject = transform.parent.gameObject;
		this.GetComponent<Canvas>().worldCamera = Camera.main;
		low = transform.Find("OverlayPanel").gameObject;
	}

	void LateUpdate(){
//		faceUIToCamera();

//		scaleWorldSpaceCanvas();
	}
	
	void faceUIToCamera(){
		this.GetComponent<Canvas>().transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles);
	}

	void stickScreenSpaceOverlayCameraToObject(){
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, parentObject.transform.position);
//		this.GetComponent<Canvas>().GetComponent<RectTransform>().anchoredPosition = screenPoint;
		
//		Vector3 screenPoint = Camera.main.WorldToViewportPoint(parentObject.transform.position);
//		this.GetComponent<Canvas>().GetComponent<RectTransform>().anchoredPosition = screenPoint;
				low.transform.position = screenPoint;
//		low.GetComponent<Canvas>().GetComponent<RectTransform>().anchoredPosition = screenPoint;
		//Debug.Log(UpgradeUICanvas.GetComponent<RectTransform>().anchoredPosition + ", " + screenPoint);
	}

	void scaleWorldSpaceCanvas(){
		float uiScaleFactor = Camera.main.transform.position.y / 17f;
		
		this.GetComponent<Canvas>().GetComponent<RectTransform>().localScale = 
			new Vector3(uiScaleFactor, uiScaleFactor, 1f);
	}
}
