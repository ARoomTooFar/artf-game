using UnityEngine;
using System.Collections;

//This class sets the ground size and position to be the same
//size as the tilemap
public class GroundSizeAdjuster : MonoBehaviour {

	TileMap tileMap;

	// Use this for initialization
	void Start () {
		tileMap = GameObject.Find ("TileMap").GetComponent<TileMap> ();;
	}
	
	// Update is called once per frame
	void Update () {
		float gridz = tileMap.grid_z * 1.0f;
		float gridx = tileMap.grid_x * 1.0f;
		transform.localScale = new Vector3(gridx, 0.03f, gridz);

		transform.position = new Vector3(gridx / 2f, 0f, gridz / 2f);
	}
}
