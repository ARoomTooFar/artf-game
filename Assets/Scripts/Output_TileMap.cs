using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Output_TileMap : MonoBehaviour {
	Input_TileMap input_tileMap;

//	public Material selectionMat; //material for selected tiles
	HashSet<Vector3> selectedTiles = new HashSet<Vector3> ();

	Transform itemObjects;
	TileMap tileMap;

	static ItemClass itemClass = new ItemClass();

	//start
	void Awake ()
	{
		tileMap = this.gameObject.GetComponent("TileMap") as TileMap;
		itemObjects = GameObject.Find ("ItemObjects").GetComponent ("Transform") as Transform;
		input_tileMap = this.gameObject.GetComponent("Input_TileMap") as Input_TileMap;
	}

	//update
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

	public void instantiateItemObject(string name, Vector3 position){
		position.x = Mathf.RoundToInt( position.x / tileMap.tileSize );
		position.z = Mathf.RoundToInt( position.z / tileMap.tileSize );

		GameObject temp = Instantiate (Resources.Load(name), position, Quaternion.identity) as GameObject;
		temp.transform.parent = itemObjects;
		temp.name = itemClass.makeName(name);
		
		(temp.GetComponent("Output_ItemObject") as Output_ItemObject).changePosition(position);
		itemClass.addToItemList(temp.name, position);
	}


}
