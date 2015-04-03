using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Text;

public class CameraDraws : MonoBehaviour {
	TileMapController tilemapcont;

	private Camera currentCamera;

	public Material selectionMat; //material for selected tiles
	public Material gridMat;
	public Material objectFocusMat;
	
	GameObject tileMapGameObj;

	private Plane groundPlane = new Plane(Vector3.up, new Vector3());
	private Ray ray;
	
	bool drawTallBox = false;
	
	public GameObject focusedObject;

	void Start(){
		currentCamera = this.gameObject.GetComponent<Camera> ();
		tilemapcont = GameObject.Find ("TileMap").GetComponent("TileMapController") as TileMapController;
		tileMapGameObj = GameObject.Find ("TileMap");
	}

	void OnPostRender ()
	{
		drawSelectedTiles ();
		drawGrid ();
		drawMouseSquare();
		//		drawBoxAroundFocusedObject();
	}	

	/* select tiles using a list from the mouse manager */
	void drawSelectedTiles ()
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
			
			
			//			Renderer rend = g.GetComponentInChildren<Renderer> ();
			//			Mesh mes = g.GetComponentInChildren<MeshFilter> ().mesh;
			Collider coll = g.GetComponentInChildren<Collider> ();
			
			//			foreach (Renderer rend in g.GetComponentsInChildren<Renderer>()){
			//				//rend.material.shader = focusedShader;
			//				Color trans = rend.material.color;
			//				trans.r = 1f;
			//				rend.material.color = trans;
			//				
			//			}
			
			Bounds bound = coll.bounds;
			
			foreach (Collider c in g.GetComponentsInChildren<Collider> ())
			{
				bound.Encapsulate(c.bounds);
			}
			//			foreach (Renderer r in g.GetComponentsInChildren<Renderer> ())
			//			{
			//				bound.Encapsulate(r.bounds);
			//			}
			//			foreach (MeshFilter fil in g.GetComponentsInChildren<MeshFilter>()){
			//				bound.Encapsulate(fil.mesh.bounds);
			//			}
			//			bound.center = g.transform.position;
			
			
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

