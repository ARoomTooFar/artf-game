using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CameraRaycast : MonoBehaviour {

	public LayerMask draggingLayerMask;
	static Camera UICamera;
	Ray ray;
	RaycastHit hit;
	ClickEvent drag;

	// Use this for initialization
	void Start() {
		draggingLayerMask = LayerMask.GetMask("Walls");
		UICamera = this.gameObject.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update() {
		// If no left click or over the UI, early return
		if(!Input.GetMouseButtonDown(0) || EventSystem.current.IsPointerOverGameObject() == true) {
			return;
		}

		// Do a ray cast from the mouse position
		ray = UICamera.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, ~draggingLayerMask)) {
			// If it hits something, get it's ClickEvent object and start it's click
			drag = hit.collider.transform.root.GetComponentInChildren<ClickEvent>();
			if(drag == null || !drag.enabled) {
				return;
			}
			StartCoroutine(drag.onClick(Input.mousePosition));	
		}
	}
}
