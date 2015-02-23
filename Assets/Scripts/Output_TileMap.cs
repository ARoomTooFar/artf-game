using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary; 
using System.Runtime.Serialization;
using System.Reflection;
using System.Text;
using System.Linq;

public class Output_TileMap : MonoBehaviour
{
//	Input_TileMap input_tileMap;
//	HashSet<Vector3> selectedTiles;
	//	public Material selectionMat; //material for selected tiles
	Transform itemObjects;
	TileMap tileMap;
	static ItemClass itemClass = new ItemClass ();
	GameObject groundGrid;

	Dictionary<Vector3, List<GameObject>> wallDic;
	
	//start
	void Awake ()
	{
//		selectedTiles = new HashSet<Vector3> ();

		wallDic = new Dictionary<Vector3, List<GameObject>>();

		tileMap = this.gameObject.GetComponent ("TileMap") as TileMap;
		itemObjects = GameObject.Find ("ItemObjects").GetComponent ("Transform") as Transform;
//		input_tileMap = this.gameObject.GetComponent ("Input_TileMap") as Input_TileMap;
		groundGrid = GameObject.Find ("Ground");
	}
	
	void Update ()
	{
		adjustGroundGrid ();
	}
	
	void adjustGroundGrid ()
	{
		float gridz = tileMap.grid_z * 1.0f;
		float gridx = tileMap.grid_x * 1.0f;
		groundGrid.transform.localScale = new Vector3 (gridx, 0.03f, gridz);
		groundGrid.transform.position = new Vector3 (gridx / 2f, 0f, gridz / 2f);
		
		//set the tiling of the grid texture to match the amount of tiles.
		//set by the tile map
		groundGrid.renderer.material.mainTextureScale = new Vector2 (tileMap.grid_x, tileMap.grid_z);
	}
	
	public GameObject instantiateItemObject (string name, Vector3 position, Vector3 rotation)
	{
		position.x = Mathf.RoundToInt (position.x / tileMap.tileSize);
		position.z = Mathf.RoundToInt (position.z / tileMap.tileSize);
		
//		Debug.Log (name + " at " + position);
		GameObject temp = Instantiate (Resources.Load (name), position, Quaternion.Euler (rotation)) as GameObject;
		
		//attempt at loading prefab with its default position. doesn't work.
		//		GameObject temps = Resources.Load(name) as GameObject;
		//		Vector3 prefabPos = temps.transform.position;
		//		prefabPos.x = position.x;
		//		prefabPos.z = position.z;
		//		GameObject temp = Instantiate (Resources.Load(name), prefabPos, Quaternion.Euler(rotation)) as GameObject;
		
		temp.transform.parent = itemObjects;
		temp.name = itemClass.makeName (name);
		
		
		
		(temp.GetComponent ("Output_ItemObject") as Output_ItemObject).changePosition (position);
		(temp.GetComponent ("Output_ItemObject") as Output_ItemObject).changeOrientation (rotation);
		
		//old way
		itemClass.addToItemList (temp.name, position, rotation);
		
		//new way
		//		itemClass.addItem (temp.name, position, rotation);

		return temp;
	}
	
	//fill in selected tiles with floor tiles
	public void fillInRoom (HashSet<Vector3> st, float firstCornerX, float firstCornerZ, float secondCornerX, float secondCornerZ)
	{
		
		
		foreach (Vector3 pos in st) {
			Vector3 rot = new Vector3 (0f, 0f, 0f);
			string floortile = "Prefabs/floortile";
			string walltile = "Prefabs/walltile";
			
			//if we're on an edge of the selected box
			if ((pos.x == firstCornerX || pos.x == secondCornerX
			     || pos.z == firstCornerZ || pos.z == secondCornerZ)) {

				//if there's no floor or wall tiles there
				if ((!itemClass.itemOnPlace(walltile, pos) && !itemClass.itemOnPlace (floortile, pos))) {
					//instantiate a wall tile

					GameObject newWallTile = instantiateItemObject (walltile, pos, rot);

					//for video demo only:
					instantiateItemObject (floortile, pos, rot);

					//if on first corner
//					if (pos.x == firstCornerX && pos.z == firstCornerZ){
//						//if wall dictionary is empty or isn't empty, but doesn't contain key
//						if(!wallDic.Any() || !wallDic.ContainsKey(pos)){
//							List<GameObject> newList = new List<GameObject>();
//							newList.Add(newWallTile);
//							wallDic.Add(pos, newList);
//							Debug.Log ("wallDic[" + pos + "]: " + wallDic[pos].Count + " entries");
//						//if dictionary not empty, and does contain key
//						}else if(wallDic.ContainsKey(pos)){
//							wallDic[pos].Add(newWallTile);
//
//						}
//					//if on wall x
//					}else if(pos.x == firstCornerX){
//
//					}

				}


				
				
			//if we're not on an edge (i.e. we are on the center area)
			//and if no floor tile there
			} else if (!itemClass.itemOnPlace (floortile, pos) && !itemClass.itemOnPlace(walltile, pos)) {
				
				//if there's a wall tile there
				if (itemClass.itemOnPlace (walltile, pos)) {
					
					//find item in itemList and remove it (the data entry for it)
					for (int i = 0; i < itemClass.getItemList().Count; i++) {
						if (itemClass.getItemList () [i].x == pos.x
						    && itemClass.getItemList () [i].z == pos.z
						    && itemClass.getItemList () [i].y == pos.y) {
							string s = itemClass.getItemList () [i].item;
							itemClass.getItemList ().Remove (itemClass.getItemList () [i]);
							
							//find list in itemObject list and remove it (the prefab for it)
							foreach (Transform child in itemObjects) {
								if (String.Equals (child.transform.name, s)) {
									GameObject.Destroy (child.gameObject);
								}
							}
						}
					}
				}
				instantiateItemObject (floortile, pos, rot);
			}
		}

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
