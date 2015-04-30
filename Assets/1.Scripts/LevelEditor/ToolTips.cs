using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class ToolTips : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	GameObject tt;
	
	void Start () {
		
	}
	
	void Update () {
		makeToolTipFollowMouse();
	}
	
	public void OnPointerEnter(PointerEventData data){
		if(tt == null && gameObject.name == "Button_Room"){
			instantiateToolTip("Place Room");
		} else if(gameObject.name == "Button_ModeToggle"){
			instantiateToolTip("Toggle Mode");
		} else if(gameObject.name == "Button_ZoomIn"){
			instantiateToolTip("Zoom In");
		} else if(gameObject.name == "Button_ZoomOut"){
			instantiateToolTip("Zoom Out");
		} else if(gameObject.name == "Button_CameraToggle"){
			instantiateToolTip("Toggle Camera");
		}
	}
	
	public void OnPointerExit(PointerEventData data){
		Destroy(tt);
	}
	
	void instantiateToolTip(string s){
		tt = Instantiate (Resources.Load ("ScreenUI/ToolTip")) as GameObject;
		tt.transform.SetParent(GameObject.Find("ScreenUI").transform);
		Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		tt.transform.position = screenPos;
		Text t = tt.transform.Find("Text").GetComponent<Text>() as Text;
		t.text = s;
	}
	
	void makeToolTipFollowMouse(){
		if (tt != null){
			Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			tt.transform.position = screenPos;
		}
	}
}

