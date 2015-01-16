using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]

public class TileMap : MonoBehaviour {

	public int grid_x;
	public int grid_z;

	public float tileSize = 1.0f;
	
	// booleans for housekeeping

	// Use this for initialization
	void Start () {
		grid_x = 25;
		grid_z = 25;
		buildMesh();
	}

	// Update is called once per frame
	void Update () {

	}

	// 
	void buildMesh(){

		// total values
		int tiles_total = grid_x * grid_z;
		int tri_total = tiles_total * 2;

		// number of vertices in each x z rows and the total number of vertices
		int vx = grid_x + 1;
		int vz = grid_z + 1;
		int vert_total = vx * vz;

		// Initialization
		Vector3[] vertices = new Vector3[4];
		int[] triangles = new int[2 * 3];
		Vector3[] normals = new Vector3[4];

		vertices [0] = new Vector3 (0, 0, 0);
		vertices [1] = new Vector3 (tileSize * vx, 0, 0);
		vertices [2] = new Vector3 (0, 0, tileSize * vz);
		vertices [3] = new Vector3 (tileSize * vx, 0, tileSize * vz);

		triangles[0] = 0;
		triangles[2] = 1;
		triangles[1] = 2;
		
		triangles[3] = 1;
		triangles[4] = 2;
		triangles[5] = 3;

		for (int i = 0; i < 4; ++i) {
			normals[i] = Vector3.up;
		}

		/*
		// Store all possible vertices
		for(int z = 0; z < vz; z++){
			for(int x = 0; x < vx; x++){
				vertices[z * vx + x] = new Vector3(x * tileSize, 0, z * tileSize);
				normals[z * vx + x] = Vector3.up;
			}
		}*/
		// Debug.Log ("Vertices logged");

		/*
		// Index all the vertices in [triangles] array, draw triangles.
		for(int z = 0; z < grid_z; z++){
			for(int x = 0; x < grid_x; x++){
				int offset = (z * grid_x + x) * 6;
				// Debug.Log(offset + " " + z + " " + x + " " + vx + " " + vz);

				triangles[offset] 	  =	z * vx + x +	  0;
				triangles[offset + 2] = z * vx + x + vx + 1;
				triangles[offset + 1] = z * vx + x + vx	   ;

				triangles[offset + 3] = z * vx + x +	  0;
				triangles[offset + 5] = z * vx + x +	  1;
				triangles[offset + 4] = z * vx + x + vx + 1;
			}
		}*/

		// Debug.Log ("Triangles logged");


		Mesh mesh = new Mesh();

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;

		MeshFilter mesh_filter = GetComponent<MeshFilter>();
		MeshRenderer mesh_render = GetComponent<MeshRenderer>();
		MeshCollider mesh_collider = GetComponent<MeshCollider>();

		mesh_filter.mesh = mesh;
		mesh_collider.sharedMesh = mesh;
		// Debug.Log ("mesh built");
	}

	void drawSquare(Vector3 A, Vector3 B, Vector3 C, Vector3 D){
		Gizmos.DrawLine (A, B);
		Gizmos.DrawLine (B, C);
		Gizmos.DrawLine (C, D);
		Gizmos.DrawLine (D, A);
	}

}