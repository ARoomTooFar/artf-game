using UnityEngine;
using System.Collections;

//This class sets the ground size and position to be the same
//size as the tilemap
public class TransformHandler_Ground : MonoBehaviour {

	TileMap tileMap;
	public GameObject ground;


	// Use this for initialization
	void Start () {
		tileMap = GameObject.Find ("TileMap").GetComponent<TileMap> ();;
	}
	
	// Update is called once per frame
	void Update () {
		float gridz = tileMap.grid_z * 1.0f;
		float gridx = tileMap.grid_x * 1.0f;
		ground.transform.localScale = new Vector3(gridx, 0.03f, gridz);

		ground.transform.position = new Vector3(gridx / 2f, 0f, gridz / 2f);

		//set the tiling of the grid texture to match the amount of tiles
		//set by the tile map
		ground.renderer.material.mainTextureScale = new Vector2(tileMap.grid_x,tileMap.grid_z);
	}
}
