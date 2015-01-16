using UnityEngine;
using System.Collections;

enum cam_views {play, top}; // rot90, rot180, rot270};

public class LECamera : MonoBehaviour {

	public Transform target;
	public float smoothing = 5f;
	public GameObject tilemap;
	public Material mat;
	Transform topdown;

	Vector3 offset;
	
	void Start () {
//		Camera.main.orthographic = true;
//		camera.orthographicSize = 30;
		//target = tilemap.transform;
		topview ();
	}

	void topview(){
		transform.rotation = Quaternion.Euler (90, 0, 0);
		//transform.LookAt (target);
		transform.Translate(new Vector3(12, 12, -20));
	}


	void  OnPostRender () {
		GL.Begin (GL.LINES);
		mat.SetPass (0);
		for (int z = 0; z < 30; z++) {
			GL.Vertex (new Vector3 (0.5f, 0f, z + 0.5f));
			GL.Vertex (new Vector3 (30.5f, 0f, z + 0.5f));
		}

		for (int x = 0; x < 30; x++) {
			GL.Vertex (new Vector3 (x + 0.5f, 0f, 0.5f));
			GL.Vertex (new Vector3 (x + 0.5f, 0f, 30.5f));
		}

		GL.End ();
	}
}
