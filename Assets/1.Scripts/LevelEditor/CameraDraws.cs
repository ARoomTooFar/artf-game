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

	public ARTFRoom room;
	public Vector3 roomOffset = Global.nullVector3;
	public Vector3 roomResizeOrigin = Global.nullVector3;
	public Vector3 roomResize = Global.nullVector3;

	private Camera cam;
	private CameraRaycast camCast;

	public Material singleColorTransMat;
	public Color normalColor;
	public Color invalidColor;

	void Start(){
		roomOffset = Global.nullVector3;
		roomResizeOrigin = Global.nullVector3;
		roomResize = Global.nullVector3;
		cam = this.gameObject.GetComponent<Camera> ();
		camCast = cam.GetComponent<CameraRaycast>();
		tilemapcont = Camera.main.GetComponent<TileMapController>();
		normalColor = new Color(0f, .5f, 1f, .5f);
		invalidColor = new Color(1f, 0f, 0f, .4f);
	}

	void OnPostRender ()
	{
		singleColorTransMat.SetPass(0);
		drawSelectedTiles ();
		drawGrid ();
		drawMouseSquare();
		//drawBoxAroundFocusedObject();
		drawRoomMoveSquare();
		drawRoomResizeSquare();
	}	

	private void drawSquare(Vector3 origin, float x, float z, float height){
		GL.Vertex3(origin.x - x / 2, origin.y + height, origin.z - z / 2);
		GL.Vertex3(origin.x - x / 2, origin.y + height, origin.z + z / 2);
		GL.Vertex3(origin.x + x / 2, origin.y + height, origin.z + z / 2);
		GL.Vertex3(origin.x + x / 2, origin.y + height, origin.z - z / 2);
	}

	/* select tiles using a list from the mouse manager */
	void drawSelectedTiles ()
	{
		HashSet<Vector3> selTile = tilemapcont.selectedTiles;
		GL.Begin (GL.QUADS);
		Square sq = new Square(tilemapcont.shiftOrigin, tilemapcont.lastClick);
		if(MapData.TheFarRooms.isAddValid(sq)) {
			singleColorTransMat.color = normalColor;
		} else {
			singleColorTransMat.color = invalidColor;
		}
		singleColorTransMat.SetPass(0);

		foreach (Vector3 origin in selTile) {
			drawSquare(origin, 1f, 1f, -.01f);
		}
		GL.End ();
	}
	
	void drawMouseSquare(){

		Vector3 point;
		if(camCast.hitDistance < camCast.mouseDistance
		   && camCast.hit.transform != null){
			point = camCast.hit.transform.position.Round();
			point.y = 0;
		} else {
			point = camCast.mouseGroundPoint.Round();
		}

		GL.Begin (GL.QUADS);
		singleColorTransMat.color = normalColor;
		singleColorTransMat.SetPass(0);
		drawSquare(point, .8f, .8f, .002f);
		GL.End ();
	}

	void drawRoomMoveSquare(){
		if(room == null || roomOffset == Global.nullVector3) {
			return;
		}

		GL.Begin (GL.QUADS);
		if(MapData.TheFarRooms.isMoveValid(room, roomOffset)) {
			singleColorTransMat.color = normalColor;
			singleColorTransMat.SetPass(0);
		} else {
			singleColorTransMat.color = invalidColor;
			singleColorTransMat.SetPass(0);
		}
		drawSquare(room.Center + roomOffset, room.Length, room.Height, .003f);
		GL.End ();
	}

	
	void drawRoomResizeSquare(){
		
		if(room == null || roomResize == Global.nullVector3 || roomResizeOrigin == Global.nullVector3) {
			return;
		}
		Square sq = new Square(room.LLCorner, room.URCorner);
		sq.resize(roomResizeOrigin, roomResize);
		sq = new Square(sq.LLCorner, sq.URCorner);

		GL.Begin (GL.QUADS);
		if(MapData.TheFarRooms.isResizeValid(roomResizeOrigin, roomResize)){
			singleColorTransMat.color = normalColor;
			singleColorTransMat.SetPass(0);
		} else {
			singleColorTransMat.color = invalidColor;
			singleColorTransMat.SetPass(0);
		}
		drawSquare(sq.Center, sq.Length, sq.Height, .003f);
		GL.End ();
	}

	/* draw the grid lines */
	void drawGrid ()
	{
		Vector3 camFocus = Vector3.zero;
		camFocus = camCast.camFocusPoint.Round();
		GL.Begin (GL.LINES);
		singleColorTransMat.color = normalColor;
		singleColorTransMat.SetPass(0);

		//draw grid over tilemap
		for (int i = Mathf.RoundToInt(camFocus.x) - (Global.grid_x/2); i < Mathf.RoundToInt(camFocus.x) + (Global.grid_x/2); i++ ) {
			GL.Vertex3 (i-.5f , 0f, camFocus.z + (Global.grid_z/2) + 0.5f);
			GL.Vertex3 (i-.5f, 0f, camFocus.z - (Global.grid_z/2) + 0.5f);
		}
		for (int i = Mathf.RoundToInt(camFocus.z) - (Global.grid_z/2); i < Mathf.RoundToInt(camFocus.z) + (Global.grid_z/2); i++ ) {
			GL.Vertex3(camFocus.x + (Global.grid_x/2)-.5f , 0f, i + 0.5f);
			GL.Vertex3(camFocus.x - (Global.grid_x/2)-.5f, 0f, i + 0.5f);
		}
		GL.End ();
	}
	
	public void drawBoxAroundFocusedObject ()
	{
		HashSet<GameObject> focusedObjects = MapData.getObjects(cam.GetComponent<TileMapController>().selectedTiles);

		foreach (GameObject obj in focusedObjects) {
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

				Vector3 bottomBackLeft = bound.center + Vector3.Scale (bound.extents, new Vector3 (-1, -1, -1));
				Vector3 bottomFrontLeft = bound.center + Vector3.Scale (bound.extents, new Vector3 (-1, -1, 1));
				Vector3 bottomFrontRight = bound.center + Vector3.Scale (bound.extents, new Vector3 (1, -1, 1)); 
				Vector3 bottomBackRight = bound.center + Vector3.Scale (bound.extents, new Vector3 (1, -1, -1));

				Vector3 topBackLeft = bound.center + Vector3.Scale (bound.extents, new Vector3 (-1, 1, -1));
				Vector3 topFrontLeft = bound.center + Vector3.Scale (bound.extents, new Vector3 (-1, 1, 1));
				Vector3 topFrontRight = bound.center + Vector3.Scale (bound.extents, new Vector3 (1, 1, 1)); 
				Vector3 topBackRight = bound.center + Vector3.Scale (bound.extents, new Vector3 (1, 1, -1)); 

				GL.Begin (GL.QUADS);
				singleColorTransMat.color = normalColor;
				singleColorTransMat.SetPass(0);

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

