using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CameraRaycast : MonoBehaviour {

	private Camera UICamera;
	private Ray ray;
	private ClickEvent drag;

	public Vector3 camFocusPoint;
	public Vector3 mouseGroundPoint;
	public RaycastHit hit;
	public float hitDistance;
	public float groundDistance;
	public float mouseDistance;


	// Use this for initialization
	void Start() {
		UICamera = this.gameObject.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update() {

		hit = new RaycastHit();

		ray = new Ray(UICamera.transform.position, UICamera.transform.forward);
		Global.ground.Raycast(ray, out groundDistance);
		camFocusPoint = ray.GetPoint(groundDistance);

		ray = UICamera.ScreenPointToRay(Input.mousePosition);
		Global.ground.Raycast(ray, out mouseDistance);
		mouseGroundPoint = ray.GetPoint(mouseDistance);

		if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
			hitDistance = Vector3.Distance(hit.point, ray.origin);
		} else{
			hitDistance = Mathf.Infinity;
		}


		// If no left click or over the UI, early return
		if(!Input.GetMouseButtonDown(0) || EventSystem.current.IsPointerOverGameObject() == true) {
			return;
		}

		if(!hit.Equals(new RaycastHit())) {
			drag = hit.collider.GetComponent<ClickEvent>();
			if(drag == null || !drag.enabled){
				return;
			}
			StartCoroutine(drag.onClick(Input.mousePosition));
		}
	}
}

