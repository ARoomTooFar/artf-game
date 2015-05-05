using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class ToolTips : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	GameObject tt;
	public String tipText;
	
	void Start() {
		tt = Instantiate(Resources.Load("ScreenUI/ToolTip")) as GameObject;
		tt.transform.SetParent(GameObject.Find("ScreenUI").transform);
		Text t = tt.transform.Find("Text").GetComponent<Text>();
		t.text = tipText;
		tt.SetActive(false);
	}
	
	void Update() {
		if(tt.activeSelf) {
			makeToolTipFollowMouse();
		}
	}

	void OnDisable() {
		OnMouseExit();
	}

	void OnMouseEnter() {
		if(gameObject.name == "Copy") {
			return;
		}
		tt.SetActive(true);
	}

	void OnMouseExit() {
		tt.SetActive(false);
	}

	public void OnPointerEnter(PointerEventData data) {
		OnMouseEnter();
	}
	
	public void OnPointerExit(PointerEventData data) {
		OnMouseExit();
	}
	
	void makeToolTipFollowMouse() {
		Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		tt.transform.position = screenPos;
	}
}

