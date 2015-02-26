using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This class renders the highlighting for selected tiles
public class Renderer_TileMapGraphics : MonoBehaviour
{
	Input_TileMap input_tileMap;
	public Material selectionMat; //material for selected tiles
	public Material gridMat;
	TileMap tileMap;
	GameObject tileMapGameObj;
	
	void Awake ()
	{
//		selectedTiles = new HashSet<Vector3> ();
		tileMap = GameObject.Find ("TileMap").GetComponent("TileMap") as TileMap;
		tileMapGameObj = GameObject.Find ("TileMap");
		input_tileMap = GameObject.Find ("TileMap").GetComponent ("Input_TileMap") as Input_TileMap;
	}

	/* update function */
	void OnPostRender ()
	{
		selectTiles ();
		drawGrid ();
	}
	
	/* select tiles using a list from the mouse manager */
	void selectTiles ()
	{
//		selectedTiles = tileMap.GetComponent<MouseHandler_TileSelection> ().selectedTiles;
		HashSet<Vector3> selTile = input_tileMap.getSelectedTiles ();
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
//		if (tiles != null) {
			size_x = tileMap.grid_x;
			size_z = tileMap.grid_z;
//		}


		//lower edge of tilemap bounding box
		float xLowerBound = tileMapGameObj.collider.bounds.center.x - 
			((tileMap.grid_x / 2) * tileMapGameObj.transform.localScale.x);

		float zLowerBound = tileMapGameObj.collider.bounds.center.z - 
			((tileMap.grid_z / 2) * tileMapGameObj.transform.localScale.z);


		//upper edge of tilemap bounding box
		float xUpperBound = tileMapGameObj.collider.bounds.center.x + 
			((tileMap.grid_x / 2) * tileMapGameObj.transform.localScale.x);
		
		float zUpperBound = tileMapGameObj.collider.bounds.center.z + 
			((tileMap.grid_z / 2) * tileMapGameObj.transform.localScale.z);


		//length and width of tileMap
		float tileMapSizeX = tileMap.grid_x * tileMapGameObj.transform.localScale.x;
		float tileMapSizeZ = tileMap.grid_z * tileMapGameObj.transform.localScale.z;


//		Debug.Log(tileMap.grid_z * tileMapGameObj.transform.localScale.z);

		for (int z = 0; z < tileMapSizeZ; z++) {
			GL.Vertex(new Vector3(Mathf.Floor()));
		}

		for (int z = 0; z < size_z; z++) {
			GL.Vertex (new Vector3 (0f - 1, 0f, z + 0.5f));
			GL.Vertex (new Vector3 (size_x - 1, 0f, z + 0.5f));
		}
		
		for (int x = 0; x < size_x; x++) {
			GL.Vertex (new Vector3 (x - 0.5f, 0f, 0f));
			GL.Vertex (new Vector3 (x - 0.5f, 0f, size_z - 0f));
		}



		/* draw grid */
//		int count = 0;
//		for (int z = 0; z < size_z; z++) {
//			GL.Vertex (new Vector3 (0f - 1, 0f, z + 0.5f));
//			GL.Vertex (new Vector3 (size_x - 1, 0f, z + 0.5f));
//		}
//		
//		for (int x = 0; x < size_x; x++) {
//			GL.Vertex (new Vector3 (x - 0.5f, 0f, 0f));
//			GL.Vertex (new Vector3 (x - 0.5f, 0f, size_z - 0f));
//		}
		
		GL.End ();
	}
}

//works
//GL.Vertex (new Vector3 (0f - 1, 0f, z + 0.5f));
//GL.Vertex (new Vector3 (size_x - 1, 0f, z + 0.5f));
//
//GL.Vertex (new Vector3 (x - 0.5f, 0f, 0f));
//GL.Vertex (new Vector3 (x - 0.5f, 0f, size_z - 0f));
