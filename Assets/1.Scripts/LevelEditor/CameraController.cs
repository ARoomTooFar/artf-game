using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Text;

public class CameraController : MonoBehaviour {
	TileMapController tilemapcont;
	
	Button Button_TopDown;
	Button Button_Perspective;
	Button Button_Orthographic;
	Button Button_ZoomOut;
	Button Button_ZoomIn;
	Button Button_Hand; 
	Button Button_Pointer; 
	
	
	static Camera currentCamera;
	
	static float baseX = 43f;
	static float baseY = 15f;
	static float baseZ = 2.5f;

	static float minY = 5f;
	static float maxY = 25f;
	static float zoomSpeed = .5f;
	
	static float orthoZoomSpeed = .5f;
	static float maxOrthoSize = 20;
	static float minOrthoSize = 2;
	static float currOrthoSize = 10;
	
	static Vector3 cameraRotation = new Vector3 (45, -45, 0);
	
	public Material selectionMat; //material for selected tiles
	public Material gridMat;
	public Material objectFocusMat;
	
	GameObject tileMapGameObj;
	
	private Plane groundPlane = new Plane(Vector3.up, new Vector3());
	private Vector3 prevMouse = new Vector3();
	private bool prevMouseBool = false;
	private Ray ray;


	bool drawTallBox = false;

	public GameObject focusedObject;
	
	
	void Awake ()
	{
		tilemapcont = GameObject.Find ("TileMap").GetComponent("TileMapController") as TileMapController;
		tileMapGameObj = GameObject.Find ("TileMap");
	}
	
	void Start () {

		// This should only ever be in the Level Editor, so I'm sticking a thing here to tell
		// the resource pool thing to strip colliders from game objects.
		GameObjectResourcePool.inLevelEditor = true;
		
		Button_TopDown = GameObject.Find ("Button_TopDown").GetComponent("Button") as Button;
		Button_Perspective = GameObject.Find ("Button_Perspective").GetComponent("Button") as Button;
		Button_Orthographic = GameObject.Find ("Button_Orthographic").GetComponent("Button") as Button;
		Button_ZoomOut = GameObject.Find ("Button_ZoomOut").GetComponent("Button") as Button;
		Button_ZoomIn = GameObject.Find ("Button_ZoomIn").GetComponent("Button") as Button;
		Button_Hand = GameObject.Find ("Button_Hand").GetComponent("Button") as Button;
		Button_Pointer = GameObject.Find ("Button_Pointer").GetComponent("Button") as Button;
		
		Button_ZoomIn.onClick.AddListener (() => {
			zoomCamIn ();});
		Button_ZoomOut.onClick.AddListener (() => {
			zoomCamOut ();});
		Button_TopDown.onClick.AddListener (() => {
			changeToTopDown (); });
		Button_Perspective.onClick.AddListener (() => {
			changeToPerspective ();});
		Button_Orthographic.onClick.AddListener (() => {
			changetoOrthographic ();});
		Button_Hand.onClick.AddListener (() => {
			cursorToHand (); });
		Button_Pointer.onClick.AddListener (() => {
			cursorToPointer ();});
		
		currentCamera = this.gameObject.GetComponent<Camera> ();
		
		setCameraRotation (new Vector3 (45, -45, 0));
		setCameraPosition (new Vector3 (baseX, baseY, baseZ));
		
		changeToPerspective ();
	}
	
	void OnPostRender ()
	{
		selectTiles ();
		drawGrid ();
		drawMouseSquare();
		drawBoxAroundFocusedObject();
	}
	
	void Update () {
		checkForMouseScrolling();
		
		checkForMouseClicks();
		
		checkForKeyPresses();
		
		setCameraPosition (new Vector3 (baseX, baseY, baseZ));


	}
	
	void checkForMouseScrolling(){
		if (Input.GetAxis ("Mouse ScrollWheel") < 0 && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false) {
			zoomCamIn ();
		}
		if (Input.GetAxis ("Mouse ScrollWheel") > 0 && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false) {
			zoomCamOut ();
		}
	}
	
	void checkForMouseClicks(){
		if(Input.GetMouseButton(1)) {
			dragCamera();
		} else {
			prevMouseBool = false;
		}
	}
	
	void checkForKeyPresses(){
		if (Input.GetKey (KeyCode.UpArrow)) {
			moveForward ();
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			moveBackward ();
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			moveLeft ();
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			moveRight ();
		}
	}
	
	public void cursorToHand(){
		
	}
	
	public void cursorToPointer(){
		
	}
	
	void setCameraRotation (Vector3 rot)
	{
		currentCamera.transform.root.rotation = Quaternion.Euler (rot);
	}

	void setCameraPosition (Vector3 pos)
	{
		currentCamera.transform.root.position = pos;
	}
	
	public void dragCamera ()
	{
		
		Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
		float distance = 0;
		groundPlane.Raycast(ray, out distance);
		Vector3 point = ray.GetPoint(distance).Round(1);
		
		if(!prevMouseBool) {
			prevMouse = point;
			prevMouseBool = true;
		}
		
		Vector3 offset = (prevMouse - point);
		baseX += offset.x;
		baseZ += offset.z;
	}
	
	Vector3 getCameraForward(){
		return currentCamera.transform.root.forward;
	}
	
	public void moveForward ()
	{
		baseX += Mathf.Sin(currentCamera.transform.root.eulerAngles.y*Mathf.Deg2Rad);
		baseZ += Mathf.Cos(currentCamera.transform.root.eulerAngles.y*Mathf.Deg2Rad);
	}
	public void moveBackward ()
	{
		baseX -= Mathf.Sin(currentCamera.transform.root.eulerAngles.y*Mathf.Deg2Rad);
		baseZ -= Mathf.Cos(currentCamera.transform.root.eulerAngles.y*Mathf.Deg2Rad);
	}
	public void moveLeft ()
	{
		baseX -= Mathf.Cos(currentCamera.transform.root.eulerAngles.y*Mathf.Deg2Rad);
		baseZ += Mathf.Sin(currentCamera.transform.root.eulerAngles.y*Mathf.Deg2Rad);
	}
	public void moveRight ()
	{
		baseX += Mathf.Cos(currentCamera.transform.root.eulerAngles.y*Mathf.Deg2Rad);
		baseZ -= Mathf.Sin(currentCamera.transform.root.eulerAngles.y*Mathf.Deg2Rad);
	}
	public void zoomCamIn ()
	{
		if(currentCamera.orthographic) {
			currOrthoSize = Mathf.Min(maxOrthoSize, currOrthoSize + orthoZoomSpeed);
			foreach (Camera cam in 	Camera.allCameras) {
				cam.orthographicSize = currOrthoSize;
			}
		} else {
			baseY = Mathf.Min (maxY, baseY + zoomSpeed);
		}
	}
	public void zoomCamOut ()
	{
		currOrthoSize = Mathf.Max(minOrthoSize, currOrthoSize - orthoZoomSpeed);
		if(currentCamera.orthographic) {
			foreach (Camera cam in 	Camera.allCameras) {
				cam.orthographicSize = currOrthoSize;
			}
		} else {
			baseY = Mathf.Max (minY, baseY - zoomSpeed);
		}
	}
	public void changeToTopDown ()
	{
		setCameraRotation (new Vector3 (90, 0, 0));
		foreach (Camera cam in 	Camera.allCameras) {
			cam.orthographic = true;
		}
		//currentCamera.orthographic = true;
	}
	public void changeToPerspective ()
	{
		foreach (Camera cam in 	Camera.allCameras) {
			cam.orthographic = false;
		}
		setCameraRotation (cameraRotation);
	}
	public void changetoOrthographic ()
	{
		foreach (Camera cam in 	Camera.allCameras) {
			cam.orthographic = true;
		}
		setCameraRotation (cameraRotation);
	}
	
	/* select tiles using a list from the mouse manager */
	void selectTiles ()
	{
		HashSet<Vector3> selTile = tilemapcont.getSelectedTiles ();
		GL.Begin (GL.QUADS);
		selectionMat.SetPass (0);
		foreach (Vector3 origin in selTile) {
			GL.Vertex (new Vector3 (origin.x - .5f, 0, origin.z - .5f));
			GL.Vertex (new Vector3 (origin.x - .5f, 0, origin.z + .5f));
			
			GL.Vertex (new Vector3 (origin.x + .5f, 0, origin.z + .5f));
			GL.Vertex (new Vector3 (origin.x + .5f, 0, origin.z - .5f));
		}
		GL.End ();
	}
	
	void drawMouseSquare(){

		float squareWidth = .4f;
		float cubeHeight = 6;

		Ray ray = currentCamera.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		Physics.Raycast (ray, out hitInfo, Mathf.Infinity);
		Vector3 point;

		if (hitInfo.collider == null) {
			return;
		}

		if (hitInfo.collider.gameObject.name != "TileMap") {
			point = hitInfo.transform.position.Round ();
			//Debug.Log (point.toCSV());
			point.y = 0;
		} else {
			ray = currentCamera.ScreenPointToRay(Input.mousePosition);
			float distance = 0;
			groundPlane.Raycast(ray, out distance);
			point = ray.GetPoint(distance).Round();
		}
		
		GL.Begin (GL.QUADS);
		gridMat.SetPass (0);
		selectionMat.SetPass (0);


		Vector3 pLA = new Vector3 (point.x - squareWidth, point.y, point.z - squareWidth);
		Vector3 pLB = new Vector3 (point.x - squareWidth, point.y, point.z + squareWidth);
		Vector3 pLC = new Vector3 (point.x + squareWidth, point.y, point.z + squareWidth);
		Vector3 pLD = new Vector3 (point.x + squareWidth, point.y, point.z - squareWidth);

		GL.Vertex (pLA);
		GL.Vertex (pLB);
		GL.Vertex (pLC);
		GL.Vertex (pLD);

		if (drawTallBox) {
			Vector3 pUA = new Vector3 (point.x - squareWidth, point.y + cubeHeight, point.z - squareWidth);
			Vector3 pUB = new Vector3 (point.x - squareWidth, point.y + cubeHeight, point.z + squareWidth);
			Vector3 pUC = new Vector3 (point.x + squareWidth, point.y + cubeHeight, point.z + squareWidth);
			Vector3 pUD = new Vector3 (point.x + squareWidth, point.y + cubeHeight, point.z - squareWidth);

			GL.Vertex (pUA);
			GL.Vertex (pUB);
			GL.Vertex (pUC);
			GL.Vertex (pUD);
		
			GL.Vertex (pUA);
			GL.Vertex (pUB);
			GL.Vertex (pUC);
			GL.Vertex (pUD);


			GL.Vertex (pLB);
			GL.Vertex (pUB);
			GL.Vertex (pUA);
			GL.Vertex (pLA);

			GL.Vertex (pLA);
			GL.Vertex (pUA);
			GL.Vertex (pUD);
			GL.Vertex (pLD);

			GL.Vertex (pLC);
			GL.Vertex (pUC);
			GL.Vertex (pUB);
			GL.Vertex (pLB);
		
			GL.Vertex (pLD);
			GL.Vertex (pUD);
			GL.Vertex (pUC);
			GL.Vertex (pLC);
		}


		
		GL.End ();
	}
	
	/* draw the grid lines */
	void drawGrid ()
	{
		GL.Begin (GL.LINES);
		gridMat.SetPass (0);
		selectionMat.SetPass (0);
		
		//lower edge of tilemap bounding box
		float xLowerBound = tileMapGameObj.GetComponent<Collider>().bounds.center.x - 
			((tilemapcont.grid_x / 2) * tileMapGameObj.transform.root.localScale.x);
		
		float zLowerBound = tileMapGameObj.GetComponent<Collider>().bounds.center.z - 
			((tilemapcont.grid_z / 2) * tileMapGameObj.transform.root.localScale.z);
		
		
		//upper edge of tilemap bounding box
		float xUpperBound = tileMapGameObj.GetComponent<Collider>().bounds.center.x + 
			((tilemapcont.grid_x / 2) * tileMapGameObj.transform.root.localScale.x);
		
		float zUpperBound = tileMapGameObj.GetComponent<Collider>().bounds.center.z + 
			((tilemapcont.grid_z / 2) * tileMapGameObj.transform.root.localScale.z);
		
		Color c = new Color(1f,1f,1f,0.01f) ;
		selectionMat.SetColor("Main Color", c);
		//draw grid over tilemap
		for (int z = (int)Mathf.Floor(zLowerBound); z < (int)Mathf.Floor(zUpperBound); z++) {
			GL.Color(c);
			GL.Vertex(new Vector3(Mathf.Floor(xLowerBound), 0f, z + 0.5f));
			GL.Vertex(new Vector3(Mathf.Floor(xUpperBound), 0f, z + 0.5f));
			
		}
		for (int x = (int)Mathf.Floor(xLowerBound); x < (int)Mathf.Floor(xUpperBound); x++) {
			GL.Vertex(new Vector3(x - 0.5f, 0f, Mathf.Floor(zLowerBound)));
			GL.Vertex(new Vector3(x - 0.5f, 0f, Mathf.Floor(zUpperBound)));
		}
		
		
		GL.End ();
	}

	public void drawBoxAroundFocusedObject ()
	{
		if (focusedObject != null) {

			GameObject g = focusedObject;
			Renderer rend = g.GetComponentInChildren<Renderer> ();
			Bounds bound = rend.bounds;

			Quaternion quat = g.transform.rotation;
			Vector3 bc = g.transform.position + quat * (bound.center - g.transform.position);


			Vector3 topFrontRight = bc + quat * Vector3.Scale (bound.extents, new Vector3 (1, 1, 1)); 
			Vector3 topFrontLeft = bc + quat * Vector3.Scale (bound.extents, new Vector3 (-1, 1, 1)); 
			Vector3 topBackLeft = bc + quat * Vector3.Scale (bound.extents, new Vector3 (-1, 1, -1));
			Vector3 topBackRight = bc + quat * Vector3.Scale (bound.extents, new Vector3 (1, 1, -1)); 
			Vector3 bottomFrontRight = bc + quat * Vector3.Scale (bound.extents, new Vector3 (1, -1, 1)); 
			Vector3 bottomFrontLeft = bc + quat * Vector3.Scale (bound.extents, new Vector3 (-1, -1, 1)); 
			Vector3 bottomBackLeft = bc + quat * Vector3.Scale (bound.extents, new Vector3 (-1, -1, -1));
			Vector3 bottomBackRight = bc + quat * Vector3.Scale (bound.extents, new Vector3 (1, -1, -1)); 

		
			GL.Begin (GL.QUADS);
			objectFocusMat.SetPass (0);

			GL.Vertex (topFrontLeft);
			GL.Vertex (topFrontRight);
			GL.Vertex (topBackRight);
			GL.Vertex (topBackLeft);
		
			GL.Vertex (topFrontLeft);
			GL.Vertex (topFrontRight);
			GL.Vertex (bottomFrontRight);
			GL.Vertex (bottomFrontLeft);

			GL.Vertex (topBackLeft);
			GL.Vertex (topBackRight);
			GL.Vertex (bottomBackRight);
			GL.Vertex (bottomBackLeft);

			GL.Vertex (topBackLeft);
			GL.Vertex (topFrontLeft);
			GL.Vertex (bottomFrontLeft);
			GL.Vertex (bottomBackLeft);

			GL.Vertex (topBackRight);
			GL.Vertex (topFrontRight);
			GL.Vertex (bottomFrontRight);
			GL.Vertex (bottomBackRight);


			GL.End ();
		}
	}
}

