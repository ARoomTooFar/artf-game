using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FaceWorldUIToCamera : MonoBehaviour
{
	public GameObject parentObject;
	public GameObject ScreenSaceOverlayPanel;

	void Start ()
	{
		parentObject = transform.parent.gameObject;
		this.GetComponent<Canvas> ().worldCamera = Camera.main;
		ScreenSaceOverlayPanel = transform.Find ("OverlayPanel").gameObject;
	}

	void Update ()
	{
//		stickScreenSpaceOverlayCameraToObject();
	}

	void LateUpdate ()
	{
		faceUIToCamera ();
		//		scaleWorldSpaceCanvas();
	}
	
	void faceUIToCamera ()
	{
		this.GetComponent<Canvas> ().transform.rotation = Quaternion.Euler (Camera.main.transform.rotation.eulerAngles);
	}

	void stickScreenSpaceOverlayCameraToObject ()
	{
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint (Camera.main, parentObject.transform.position);
		ScreenSaceOverlayPanel.transform.position = screenPoint;
	}

	void scaleWorldSpaceCanvas ()
	{
		float uiScaleFactor = Camera.main.transform.position.y / 17f;
		
		this.GetComponent<Canvas> ().GetComponent<RectTransform> ().localScale = 
			new Vector3 (uiScaleFactor, uiScaleFactor, 1f);
	}
}
