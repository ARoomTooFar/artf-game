using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]

public class TileMap : MonoBehaviour {
//
//	public int grid_x;
//	public int grid_z;
//
//	public float tileSize = 1.0f;
//
//	Camera UICamera;
//
//	void Start () {
//		UICamera = GameObject.Find ("UICamera").camera;
//
//		grid_x = 20;
//		grid_z = 20;
//		buildMesh();
//	}
//	
//	void Update () {
//
//		//make tileMap object move around with camera, where
//		//camera is dead center of it
//		Vector3 camPos = UICamera.transform.position;
//		camPos.y = 0f;
//		camPos.x -= (grid_x / 2) * transform.localScale.x;
//		camPos.z -= (grid_z / 2) * transform.localScale.z;
//		transform.position = camPos;
//	}
//
//	// 
//	void buildMesh(){
//
//		/* total values */
////		int tiles_total = grid_x * grid_z;
////		int tri_total = tiles_total * 2;
//
//		/* number of vertices in each x z rows and the total number of vertices */
//		int vx = grid_x - 1;
//		int vz = grid_z - 1;
////		int vert_total = vx * vz;
//
//		/* Initialization */
//		Vector3[] vertices = new Vector3[4];
//		int[] triangles = new int[2 * 3];
//		Vector3[] normals = new Vector3[4];
//
//		/* store the 4 corners of the mesh */
//		vertices [0] = new Vector3 (0, 0, 0);
//		vertices [1] = new Vector3 (tileSize * vx, 0, 0);
//		vertices [2] = new Vector3 (0, 0, tileSize * vz);
//		vertices [3] = new Vector3 (tileSize * vx, 0, tileSize * vz);
//
//		/* Arrange the vertices in counterclockwise order to produce the correct normal, used for raycasting and rendering
//		 backface culling */
//
//		triangles[0] = 0;
//		triangles[2] = 1;
//		triangles[1] = 2;
//		
//		triangles[3] = 1;
//		triangles[4] = 2;
//		triangles[5] = 3;
//
//		for (int i = 0; i < 4; ++i) {
//			normals[i] = Vector3.up;
//		}
//
//		/* create mesh */
//		Mesh mesh = new Mesh();
//
//		mesh.vertices = vertices;
//		mesh.triangles = triangles;
//		mesh.normals = normals;
//
//		MeshFilter mesh_filter = GetComponent<MeshFilter>();
////		MeshRenderer mesh_render = GetComponent<MeshRenderer>();
//		MeshCollider mesh_collider = GetComponent<MeshCollider>();
//
//		mesh_filter.mesh = mesh;
//		mesh_collider.sharedMesh = mesh;
//
//	}

}