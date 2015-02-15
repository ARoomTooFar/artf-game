using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This class renders the highlighting for selected tiles
public class Renderer_TileMapGraphics : MonoBehaviour
{
	Input_TileMap input_tileMap;
	public Material selectionMat; //material for selected tiles
	HashSet<Vector3> selectedTiles = new HashSet<Vector3> ();
	
	void Awake ()
	{
		input_tileMap = GameObject.Find ("TileMap").GetComponent ("Input_TileMap") as Input_TileMap;
	}

	/* update function */
	void OnPostRender ()
	{
		selectTiles ();
//		drawGrid ();
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
//	void drawGrid(){
//		GL.Begin (GL.LINES);
//		gridMat.SetPass (0);
//
//		/* get size of tile map */
//		int size_x = 0; int size_z = 0;
//		if (tiles != null) {
//			size_x = tiles.GetComponent<TileMap>().grid_x;
//			size_z = tiles.GetComponent<TileMap>().grid_z;
//		}
//
//		/* draw grid */
//		int count = 0;
//		for (int z = 0; z < size_z; z++) {
//			if(count == 5){
//				
//			}
//			GL.Vertex (new Vector3 (0f, 0f, z));
//			GL.Vertex (new Vector3 (size_x, 0f, z));
//		}
//		
//		for (int x = 0; x < size_x; x++) {
//			GL.Vertex (new Vector3 (x, 0f, 0f));
//			GL.Vertex (new Vector3 (x, 0f, size_z - 0f));
//		}
//		
//		GL.End ();
//	}
}
