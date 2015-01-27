using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum cam_views {play, top}; // rot90, rot180, rot270};

[ExecuteInEditMode]
public class LECamera : MonoBehaviour {

	public Transform target;
	public float smoothing = 5f;
	GameObject tiles;
	public Material mat;
	List<Vector3> selectedTiles;

	Vector3 offset;
	
	void Awake () {
		tiles = GameObject.Find ("TileMap");
		topview ();
	}

	void topview(){
		transform.rotation = Quaternion.Euler (45, 0, 0);
		//transform.LookAt (target);
		transform.position = new Vector3(0, 10, 0);

	}


	/* draw the grid lines */
	void drawGrid(){
		GL.Begin (GL.LINES);
		mat.SetPass (0);

		/* get size of tile map */
		int size_x = 0; int size_z = 0;
		if (tiles != null) {
			size_x = tiles.GetComponent<TileMap>().grid_x;
			size_z = tiles.GetComponent<TileMap>().grid_z;
		}

		/* draw grid */
		int count = 0;
		for (int z = 0; z < size_z; z++) {
			if(count == 5){
				
			}
			GL.Vertex (new Vector3 (0f, 0f, z));
			GL.Vertex (new Vector3 (size_x, 0f, z));
		}
		
		for (int x = 0; x < size_x; x++) {
			GL.Vertex (new Vector3 (x, 0f, 0f));
			GL.Vertex (new Vector3 (x, 0f, size_z - 0f));
		}
		
		GL.End ();
	}

	/* update function */
	void OnPostRender () {
		drawGrid ();
		selectTiles ();
	}

	/* select tiles using a list from the mouse manager */
	void selectTiles(){
//		Debug.Log (selectedTiles.Count);
		selectedTiles = tiles.GetComponent<MouseControl> ().selectedTiles;
		GL.Begin (GL.QUADS);
		for(int i = 0; i < selectedTiles.Count; i++){
			GL.Vertex(new Vector3( selectedTiles[i].x, 0, selectedTiles[i].z ) );
			GL.Vertex(new Vector3( selectedTiles[i].x, 0, selectedTiles[i].z + 1 ) );
			GL.Vertex(new Vector3( selectedTiles[i].x + 1, 0, selectedTiles[i].z + 1 ) );
			GL.Vertex(new Vector3( selectedTiles[i].x + 1, 0, selectedTiles[i].z ) );
		}
		GL.End ();

	}

}
