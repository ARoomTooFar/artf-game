using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Output_TileMap : MonoBehaviour {
	Input_TileMap input_tileMap;
	
	HashSet<Vector3> selectedTiles = new HashSet<Vector3> ();
//	public Material selectionMat; //material for selected tiles
	Transform itemObjects;
	TileMap tileMap;

	static ItemClass itemClass = new ItemClass();

	GameObject groundGrid;

	//start
	void Awake ()
	{
		tileMap = this.gameObject.GetComponent("TileMap") as TileMap;
		itemObjects = GameObject.Find ("ItemObjects").GetComponent ("Transform") as Transform;
		input_tileMap = this.gameObject.GetComponent("Input_TileMap") as Input_TileMap;
		groundGrid = GameObject.Find ("Ground");
	}

	void Update(){
		adjustGroundGrid();
	}

	void adjustGroundGrid(){
		float gridz = tileMap.grid_z * 1.0f;
		float gridx = tileMap.grid_x * 1.0f;
		groundGrid.transform.localScale = new Vector3(gridx, 0.03f, gridz);
		groundGrid.transform.position = new Vector3(gridx / 2f, 0f, gridz / 2f);
		
		//set the tiling of the grid texture to match the amount of tiles.
		//set by the tile map
		groundGrid.renderer.material.mainTextureScale = new Vector2(tileMap.grid_x,tileMap.grid_z);
	}

	public void instantiateItemObject(string name, Vector3 position, Vector3 rotation){
		position.x = Mathf.RoundToInt( position.x / tileMap.tileSize );
		position.z = Mathf.RoundToInt( position.z / tileMap.tileSize );
		
		GameObject temp = Instantiate (Resources.Load(name), position, Quaternion.Euler(rotation)) as GameObject;
		temp.transform.parent = itemObjects;
		temp.name = itemClass.makeName(name);
		
		(temp.GetComponent("Output_ItemObject") as Output_ItemObject).changePosition(position);
		(temp.GetComponent("Output_ItemObject") as Output_ItemObject).changeOrientation(rotation);

		itemClass.addToItemList(temp.name, position, rotation);
	}

	//update
	//still being done in renderer_tilemapgraphics, on UICamera
	void OnPostRender ()
	{
//		highlightSelectedTiles ();
	}

//	void highlightSelectedTiles ()
//	{
//		HashSet<Vector3> selTile = input_tileMap.getSelectedTiles();
//		GL.Begin (GL.QUADS);
//		selectionMat.SetPass (0);
//		foreach (Vector3 origin in selTile) {
//			GL.Vertex (new Vector3 (origin.x - .5f, 0, origin.z - .5f));
//			GL.Vertex (new Vector3 (origin.x - .5f, 0, origin.z + .5f));
//			
//			GL.Vertex (new Vector3 (origin.x + .5f, 0, origin.z + .5f));
//			GL.Vertex (new Vector3 (origin.x + .5f, 0, origin.z - .5f));
//		}
//		GL.End ();
//	}




}
