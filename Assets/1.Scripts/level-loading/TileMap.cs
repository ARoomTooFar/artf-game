using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]

public class TileMap : MonoBehaviour {

	public int grid_x;
	public int grid_z;

	public float tileSize = 1.0f;
	
	// booleans for housekeeping

	// Use this for initialization
	void Start () {
		grid_x = 60;
		grid_z = 60;
		buildMesh();
	}

	// Update is called once per frame
	void Update () {

	}

	// 
	void buildMesh(){

		/* total values */
		int tiles_total = grid_x * grid_z;
		int tri_total = tiles_total * 2;

		/* number of vertices in each x z rows and the total number of vertices */
		int vx = grid_x - 1;
		int vz = grid_z - 1;
		int vert_total = vx * vz;

		/* Initialization */
		Vector3[] vertices = new Vector3[4];
		int[] triangles = new int[2 * 3];
		Vector3[] normals = new Vector3[4];

		/* store the 4 corners of the mesh */
		vertices [0] = new Vector3 (0, 0, 0);
		vertices [1] = new Vector3 (tileSize * vx, 0, 0);
		vertices [2] = new Vector3 (0, 0, tileSize * vz);
		vertices [3] = new Vector3 (tileSize * vx, 0, tileSize * vz);

		/* Arrange the vertices in counterclockwise order to produce the correct normal, used for raycasting and rendering
		 backface culling */

		triangles[0] = 0;
		triangles[2] = 1;
		triangles[1] = 2;
		
		triangles[3] = 1;
		triangles[4] = 2;
		triangles[5] = 3;

		for (int i = 0; i < 4; ++i) {
			normals[i] = Vector3.up;
		}

		/* create mesh */
		Mesh mesh = new Mesh();

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;

		MeshFilter mesh_filter = GetComponent<MeshFilter>();
		MeshRenderer mesh_render = GetComponent<MeshRenderer>();
		MeshCollider mesh_collider = GetComponent<MeshCollider>();

		mesh_filter.mesh = mesh;
		mesh_collider.sharedMesh = mesh;

	}

}