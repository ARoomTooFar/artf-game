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


	static Camera UICamera;
	static Camera OnTopCamera;
	
	static float baseX = 43f;
	static float baseY = 15f;
	static float baseZ = 2.5f;
	static float minY = 5f;
	static float maxY = 25f;
	static Vector2 dragSpeed;
	static float zoomSpeed = 2f;
	static bool isTopDown = false;
	float dx;
	float dy;
	
	public Material selectionMat; //material for selected tiles
	public Material gridMat;
	TileMap tileMap;
	GameObject tileMapGameObj;


	void Awake ()
	{
		tilemapcont = GameObject.Find ("TileMap").GetComponent("TileMapController") as TileMapController;
		tileMap = GameObject.Find ("TileMap").GetComponent("TileMap") as TileMap;
		tileMapGameObj = GameObject.Find ("TileMap");
	}
	
	void Start () {
		
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


		dragSpeed = new Vector2 (3f, 3f);
		
		UICamera = GameObject.Find ("UICamera").GetComponent<Camera>();
		OnTopCamera = GameObject.Find ("LayersOnTopOfEverythingCamera").GetComponent<Camera>();
		
		setCameraRotation (new Vector3 (45, -45, 0));
		setCameraPosition (new Vector3 (baseX, baseY, baseZ));
		
		changeToPerspective ();
	}

	void OnPostRender ()
	{
		selectTiles ();
		drawGrid ();
	}
	
	void Update () {
		checkForMouseScrolling();
		checkForMouseClicks();
		
		//doesn't move cam in proper direction right now
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
		if (Input.GetMouseButton (1)) {
			dragCamera();
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
		UICamera.transform.rotation = Quaternion.Euler (rot);
	}
	
	void setCameraPosition (Vector3 pos)
	{
		UICamera.transform.position = pos;
	}
	
	public void dragCamera ()
	{
		dx = Input.GetAxis ("Mouse X") * dragSpeed.x;
		dy = Input.GetAxis ("Mouse Y") * dragSpeed.y;
		UICamera.transform.position -= UICamera.transform.right * dx + UICamera.transform.up * dy;
		baseX = UICamera.transform.position.x;
		baseZ = UICamera.transform.position.z;
		
		if (baseY < minY) {
			baseY = minY;
		}
		
		if (baseY > maxY) {
			baseY = maxY;
		}
		setCameraPosition (new Vector3 (baseX, baseY, baseZ));
	}
	
	Vector3 getCameraForward(){
		return UICamera.transform.forward;
	}
	
	public void moveForward ()
	{
		baseZ += .1f;
		baseX -= .1f;
	}
	public void moveBackward ()
	{
		baseZ -= .1f;
		baseX += .1f;
	}
	public void moveLeft ()
	{
		baseZ -= .1f;
		baseX -= .1f;
	}
	public void moveRight ()
	{
		baseZ += .1f;
		baseX += .1f;
	}
	public void zoomCamIn ()
	{
		baseY += zoomSpeed;
		if (baseY > maxY) {
			baseY = maxY;
		}
	}
	public void zoomCamOut ()
	{
		baseY -= zoomSpeed;
		if (baseY < minY) {
			baseY = minY;
		}
	}
	public void changeToTopDown ()
	{
		setCameraRotation (new Vector3 (90, -45, 0));
		isTopDown = true;
	}
	public void changeToPerspective ()
	{
		UICamera.orthographic = false;
		OnTopCamera.orthographic = false;
		isTopDown = false;
		setCameraRotation (new Vector3 (45, -45, 0));
	}
	public void changetoOrthographic ()
	{
		UICamera.orthographic = true;
		OnTopCamera.orthographic = true;
		isTopDown = false;
		setCameraRotation (new Vector3 (45, -45, 0));
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
	
	/* draw the grid lines */
	void drawGrid(){
		GL.Begin (GL.LINES);
		gridMat.SetPass (0);
		selectionMat.SetPass (0);
		
		/* get size of tile map */
		int size_x = 0; int size_z = 0;
		size_x = tilemapcont.grid_x;
		size_z = tilemapcont.grid_z;
		
		
		//lower edge of tilemap bounding box
		float xLowerBound = tileMapGameObj.GetComponent<Collider>().bounds.center.x - 
			((tilemapcont.grid_x / 2) * tileMapGameObj.transform.localScale.x);
		
		float zLowerBound = tileMapGameObj.GetComponent<Collider>().bounds.center.z - 
			((tilemapcont.grid_z / 2) * tileMapGameObj.transform.localScale.z);
		
		
		//upper edge of tilemap bounding box
		float xUpperBound = tileMapGameObj.GetComponent<Collider>().bounds.center.x + 
			((tilemapcont.grid_x / 2) * tileMapGameObj.transform.localScale.x);
		
		float zUpperBound = tileMapGameObj.GetComponent<Collider>().bounds.center.z + 
			((tilemapcont.grid_z / 2) * tileMapGameObj.transform.localScale.z);
		
		
		//length and width of tileMap
		float tileMapSizeX = tilemapcont.grid_x * tileMapGameObj.transform.localScale.x;
		float tileMapSizeZ = tilemapcont.grid_z * tileMapGameObj.transform.localScale.z;
		
		
		//draw grid over tilemap
		for (int z = (int)Mathf.Floor(zLowerBound); z < (int)Mathf.Floor(zUpperBound); z++) {
			GL.Vertex(new Vector3(Mathf.Floor(xLowerBound), 0f, z + 0.5f));
			GL.Vertex(new Vector3(Mathf.Floor(xUpperBound), 0f, z + 0.5f));
		}
		for (int x = (int)Mathf.Floor(xLowerBound); x < (int)Mathf.Floor(xUpperBound); x++) {
			GL.Vertex(new Vector3(x - 0.5f, 0f, Mathf.Floor(zLowerBound)));
			GL.Vertex(new Vector3(x - 0.5f, 0f, Mathf.Floor(zUpperBound)));
		}
	
		
		GL.End ();
	}
}

