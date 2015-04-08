using UnityEngine;
using System.Collections;

public class CameraRaycast : MonoBehaviour {

	public LayerMask draggingLayerMask;
	static Camera UICamera;
	TileMapController tilemapcont;
	float mouseDeadZone = 10f;
	Shader focusedShader;
	Shader nonFocusedShader;
	Vector3 newp;


	// Use this for initialization
	void Start() {
		draggingLayerMask = LayerMask.GetMask("Walls");
		UICamera = this.gameObject.GetComponent<Camera>();
		tilemapcont = GameObject.Find("TileMap").GetComponent("TileMapController") as TileMapController;
		
		focusedShader = Shader.Find("Transparent/Bumped Diffuse");
		nonFocusedShader = Shader.Find("Bumped Diffuse");
	}
	
	// Update is called once per frame
	void Update() {
		if(!Input.GetMouseButtonDown(0) || UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == true) 
			return;
		
		Ray ray = UICamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit; 
		
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, ~draggingLayerMask)) {
			ClickEvent drag = hit.collider.transform.root.GetComponentInChildren<ClickEvent>();
			if(drag == null || !drag.enabled) {
				return;
			}
			StartCoroutine(drag.onClick(Input.mousePosition));	
		}
	}
}
