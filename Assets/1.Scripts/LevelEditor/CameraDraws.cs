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

	private Camera cam;

	public Material selectionMat; //material for selected tiles
	public Material gridMat;
	public Material objectFocusMat;

	private Ray ray;
	
	bool drawTallBox = false;

	void Start(){
		cam = this.gameObject.GetComponent<Camera> ();
		tilemapcont = GameObject.Find ("TileMap").GetComponent("TileMapController") as TileMapController;
	}

	void OnPostRender ()
	{
		drawSelectedTiles ();
		drawGrid ();
		drawMouseSquare();
		drawBoxAroundFocusedObject();

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
		Ray ray = cam.ScreenPointToRay (Input.mousePosition);
		float distance;
		Global.ground.Raycast(ray, out distance);
		RaycastHit hitInfo;

		Physics.Raycast (ray, out hitInfo, distance);
		Vector3 point;
		
		if (hitInfo.collider != null) {
			point = hitInfo.transform.position.Round ();
			//Debug.Log (point.toCSV());
			point.y = 0;
		} else {
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
		Camera UICamera = GameObject.Find("UICamera").GetComponent<Camera>();
		Plane ground = new Plane(Vector3.up, Vector3.zero);
		Ray ray = new Ray();
		ray.origin = UICamera.transform.position;
		ray.direction = UICamera.transform.forward;
		float distance;
		Vector3 camFocus = Vector3.zero;
		if(ground.Raycast(ray, out distance)) {
			camFocus = ray.GetPoint(distance).Round();
		}

		GL.Begin (GL.LINES);
		gridMat.SetPass (0);
		selectionMat.SetPass (0);
		
		Color c = new Color(1f,1f,1f,0.01f) ;
		selectionMat.SetColor("Main Color", c);
		//draw grid over tilemap
		for (int i = Mathf.RoundToInt(camFocus.x) - (Global.grid_x/2); i < Mathf.RoundToInt(camFocus.x) + (Global.grid_x/2); i++ ) {
			GL.Color(c);
			GL.Vertex (new Vector3 (i-.5f , 0f, camFocus.z + (Global.grid_z/2) + 0.5f));
			GL.Vertex (new Vector3 (i-.5f, 0f, camFocus.z - (Global.grid_z/2) + 0.5f));
		}
		for (int i = Mathf.RoundToInt(camFocus.z) - (Global.grid_z/2); i < Mathf.RoundToInt(camFocus.z) + (Global.grid_z/2); i++ ) {
			GL.Color(c);
			GL.Vertex (new Vector3 (camFocus.x + (Global.grid_x/2)-.5f , 0f, i + 0.5f));
			GL.Vertex (new Vector3 (camFocus.x - (Global.grid_x/2)-.5f, 0f, i + 0.5f));
		}
		
		
		GL.End ();
	}
	
	public void drawBoxAroundFocusedObject ()
	{
		ObjectFocus.fillFocusedObjects();

		foreach (GameObject obj in ObjectFocus.focusedObjects) {
			if (obj != null) {
				Bounds bound = new Bounds();
				int i = 0;
				foreach(Renderer rend in obj.GetComponentsInChildren<Renderer>()){
					if(rend is ParticleSystemRenderer){
						continue;
					}

					if(!rend.enabled){
						continue;
					}
					bound.center = bound.center+rend.bounds.center;
					i++;
				}
				bound.center = bound.center/i;
				foreach(Renderer rend in obj.GetComponentsInChildren<Renderer>()){
					if(rend is ParticleSystemRenderer){
						continue;
					}
					if(!rend.enabled){
						continue;
					}
					bound.Encapsulate(rend.bounds);
				}
				//bound.center = obj.transform.position;

				//Renderer method
//				Renderer rend = obj.GetComponentInChildren<Renderer> ();
//				Bounds bound = rend.bounds;
//				foreach (Renderer r in obj.GetComponentsInChildren<Renderer> ())
//				{
//					bound.Encapsulate(r.bounds);
//				}
			
				//MeshFilter method
//				Mesh mes = obj.GetComponentInChildren<MeshFilter> ().mesh;
//				Bounds bound = mes.bounds;
//				foreach (MeshFilter fil in obj.GetComponentsInChildren<MeshFilter>()){
//					bound.Encapsulate(fil.mesh.bounds);
//				}
//				bound.center = obj.transform.position;

				//Quaternion quat = obj.transform.rotation;
				//Quaternion quat = Quaternion.identity;
				//Vector3 bc = obj.transform.position + quat * (bound.center - obj.transform.position);
			
				/*
				Vector3 topFrontRight = bc + quat * Vector3.Scale (bound.extents, new Vector3 (1, 1, 1)); 
				Vector3 topFrontLeft = bc + quat * Vector3.Scale (bound.extents, new Vector3 (-1, 1, 1)); 
				Vector3 topBackLeft = bc + quat * Vector3.Scale (bound.extents, new Vector3 (-1, 1, -1));
				Vector3 topBackRight = bc + quat * Vector3.Scale (bound.extents, new Vector3 (1, 1, -1)); 
				Vector3 bottomFrontRight = bc + quat * Vector3.Scale (bound.extents, new Vector3 (1, -1, 1)); 
				Vector3 bottomFrontLeft = bc + quat * Vector3.Scale (bound.extents, new Vector3 (-1, -1, 1)); 
				Vector3 bottomBackLeft = bc + quat * Vector3.Scale (bound.extents, new Vector3 (-1, -1, -1));
				Vector3 bottomBackRight = bc + quat * Vector3.Scale (bound.extents, new Vector3 (1, -1, -1));*/

				Vector3 bottomBackLeft = bound.center + Vector3.Scale (bound.extents, new Vector3 (-1, -1, -1));
				Vector3 bottomFrontLeft = bound.center + Vector3.Scale (bound.extents, new Vector3 (-1, -1, 1));
				Vector3 bottomFrontRight = bound.center + Vector3.Scale (bound.extents, new Vector3 (1, -1, 1)); 
				Vector3 bottomBackRight = bound.center + Vector3.Scale (bound.extents, new Vector3 (1, -1, -1));

				Vector3 topBackLeft = bound.center + Vector3.Scale (bound.extents, new Vector3 (-1, 1, -1));
				Vector3 topFrontLeft = bound.center + Vector3.Scale (bound.extents, new Vector3 (-1, 1, 1));
				Vector3 topFrontRight = bound.center + Vector3.Scale (bound.extents, new Vector3 (1, 1, 1)); 
				Vector3 topBackRight = bound.center + Vector3.Scale (bound.extents, new Vector3 (1, 1, -1)); 

				GL.Begin (GL.QUADS);
				objectFocusMat.SetPass (0);

				GL.Vertex(bottomBackLeft);
				GL.Vertex(bottomFrontLeft);
				GL.Vertex(bottomFrontRight);
				GL.Vertex(bottomBackRight);

				GL.Vertex (topBackLeft);
				GL.Vertex (topFrontLeft);
				GL.Vertex (topFrontRight);
				GL.Vertex (topBackRight);
				
				GL.Vertex (bottomFrontLeft);
				GL.Vertex (topFrontLeft);
				GL.Vertex (topBackLeft);
				GL.Vertex (bottomBackLeft);
				
				GL.Vertex (bottomBackLeft);
				GL.Vertex (topBackLeft);
				GL.Vertex (topBackRight);
				GL.Vertex (bottomBackRight);
				
				GL.Vertex (bottomFrontRight);
				GL.Vertex (topFrontRight);
				GL.Vertex (topFrontLeft);
				GL.Vertex (bottomFrontLeft);
				
				GL.Vertex (bottomBackRight);
				GL.Vertex (topBackRight);
				GL.Vertex (topFrontRight);
				GL.Vertex (bottomFrontRight);
			
				GL.End ();
			}
		}
	}
}

