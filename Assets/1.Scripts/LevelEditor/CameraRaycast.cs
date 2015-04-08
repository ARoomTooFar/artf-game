using UnityEngine;
using System.Collections;

public class CameraRaycast : MonoBehaviour {

	public LayerMask draggingLayerMask;
	static Camera UICamera;
	Ray ray;
	RaycastHit hit; 

	// Use this for initialization
	void Start() {
		draggingLayerMask = LayerMask.GetMask("Walls");
		UICamera = this.gameObject.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update() {
		if(!Input.GetMouseButtonDown(0) || UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == true) {
			return;
		}
		
		ray = UICamera.ScreenPointToRay(Input.mousePosition);
		
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, ~draggingLayerMask)) {
			ClickEvent drag = hit.collider.transform.root.GetComponentInChildren<ClickEvent>();
			if(drag == null || !drag.enabled) {
				return;
			}
			StartCoroutine(drag.onClick(Input.mousePosition));	
		}
	}
}
