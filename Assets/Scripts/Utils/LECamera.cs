using UnityEngine;
using System.Collections;
using System.Collections.Generic;

enum cam_views {play, top}; // rot90, rot180, rot270};

public class LECamera : MonoBehaviour {

	public Transform target;
	public float smoothing = 5f;
	GameObject tiles;
	public Material gridMat;
	public Material selectionMat;
	HashSet<Vector3> selectedTiles= new HashSet<Vector3> ();

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
		gridMat.SetPass (0);

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
		selectTiles ();
		drawGrid ();

	}

	/* select tiles using a list from the mouse manager */
	void selectTiles(){
//		Debug.Log (selectedTiles.Count);
		selectedTiles = tiles.GetComponent<MouseControl> ().selectedTiles;
		GL.Begin (GL.QUADS);
		selectionMat.SetPass (0);
		foreach(Vector3 origin in selectedTiles){
			GL.Vertex(new Vector3( origin.x, 0, origin.z ) );
			GL.Vertex(new Vector3( origin.x, 0, origin.z + 1 ) );
			GL.Vertex(new Vector3( origin.x + 1, 0, origin.z + 1 ) );
			GL.Vertex(new Vector3( origin.x + 1, 0, origin.z ) );
		}
		GL.End ();

	}

}
