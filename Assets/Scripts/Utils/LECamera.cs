using UnityEngine;
using System.Collections;

enum cam_views {play, top}; // rot90, rot180, rot270};

[ExecuteInEditMode]
public class LECamera : MonoBehaviour {

	public Transform target;
	public float smoothing = 5f;
	GameObject tiles;
	public Material mat;
	Transform topdown;

	Vector3 offset;
	
	void Awake () {
//		Camera.main.orthographic = true;
//		camera.orthographicSize = 30;
		//target = tilemap.transform;
		tiles = GameObject.Find ("TileMap");
		topview ();
	}

	void topview(){
		transform.rotation = Quaternion.Euler (90, 0, 0);
		//transform.LookAt (target);
		transform.position = new Vector3(12, 10, 0);

	}


	void  OnPostRender () {
		GL.Begin (GL.LINES);
		mat.SetPass (0);

		int size_x = 0; int size_z = 0;
		if (tiles != null) {
			size_x = tiles.GetComponent<TileMap>().grid_x;
			size_z = tiles.GetComponent<TileMap>().grid_z;
		}

		for (int z = 0; z < size_z; z++) {
			GL.Vertex (new Vector3 (-0.5f, 0f, z - 0.5f));
			GL.Vertex (new Vector3 (size_x - 0.5f, 0f, z - 0.5f));
		}

		for (int x = 0; x < size_x; x++) {
			GL.Vertex (new Vector3 (x - 0.5f, 0f, -0.5f));
			GL.Vertex (new Vector3 (x - 0.5f, 0f, size_z - 0.5f));
		}

		GL.End ();
	}
}
