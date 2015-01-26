using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TerrainBlockTests ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Assert(bool boolean, string message){
		if(!boolean){
			Debug.LogError("Assert Error: " + message);
		}
	}

	void TerrainBlockTests(){

		TerrainBlock testBlock = new TerrainBlock ("block1", new Vector3 (), DIRECTION.North);

		//Blocks in each cardinal and ordinal direction to testBlock
		TerrainBlock testBlockNorth = new TerrainBlock ("block1", new Vector3 (.0f, .0f, 1.0f), DIRECTION.North);
		TerrainBlock testBlockSouth = new TerrainBlock ("block1", new Vector3 (0f, .0f, -1.0f), DIRECTION.North);
		TerrainBlock testBlockEast = new TerrainBlock ("block1", new Vector3 (1f, .0f, .0f), DIRECTION.North);
		TerrainBlock testBlockWest = new TerrainBlock ("block1", new Vector3 (-1f, .0f, .0f), DIRECTION.North);
		TerrainBlock testBlockNorthEast = new TerrainBlock ("block1", new Vector3 (1f, .0f, 1.0f), DIRECTION.North);
		TerrainBlock testBlockSouthEast = new TerrainBlock ("block1", new Vector3 (1f, .0f, -1.0f), DIRECTION.North);
		TerrainBlock testBlockNorthWest = new TerrainBlock ("block1", new Vector3 (-1f, .0f, 1.0f), DIRECTION.North);
		TerrainBlock testBlockSouthWest = new TerrainBlock ("block1", new Vector3 (-1f, .0f, -1.0f), DIRECTION.North);
		
		//Check adding a block.
		testBlock.addNeighbor (testBlockNorth, DIRECTION.North);
		Assert (testBlock.getNeighbor(DIRECTION.North).Equals(testBlockNorth), "problem in adding or getting TerrainBlockNeighbor");
		Assert (testBlock.getNeighbor(DIRECTION.South.Opposite()).Equals(testBlockNorth), "problem in adding or getting TerrainBlockNeighbor");
		Assert (testBlock.getNeighbor (DIRECTION.South) == null, "block thinks there is neighbor where there isn't");

		//Make sure cannot add block where block exists
		Assert (!testBlock.addNeighbor (testBlockEast, DIRECTION.North), "block does not block adding where a block already is");
		
		//Make sure you cannot add a block in DIRECTION.NotNeighbor
		try{
			testBlock.addNeighbor(testBlockNorth, DIRECTION.NotNeighbor);
			Debug.LogError("Assigning neighbor in NotNeighbor direction not blocked");
		}catch(Exception){
		}

		//checj clearNeighbors
		testBlock.addNeighbor (testBlockNorth, DIRECTION.North);
		testBlock.addNeighbor (testBlockSouth, DIRECTION.South);
		testBlock.addNeighbor (testBlockEast, DIRECTION.East);
		Assert (testBlock.Neighbors.Count == 3, "why are there not three blocks here");
		testBlock.clearNeighbors ();
		Assert (testBlock.Neighbors.Count == 0, "Why neighbors not removed?");

		//Check removing neighbor
		testBlock.removeNeighbor (DIRECTION.North);
		Assert (testBlock.getNeighbor (DIRECTION.North) == null, "problem in removing neighbor");

		//Make sure isNeighbor is working
		Assert (testBlock.isNeighbor (testBlockNorth).Equals (DIRECTION.North), "error in isNeighbor");
		Assert (testBlock.isNeighbor (testBlockSouth).Equals (DIRECTION.South), "error in isNeighbor");
		Assert (testBlock.isNeighbor (testBlockEast).Equals (DIRECTION.East), "error in isNeighbor");
		Assert (testBlock.isNeighbor (testBlockWest).Equals (DIRECTION.West), "error in isNeighbor");
		Assert (testBlock.isNeighbor (testBlockNorthEast).Equals (DIRECTION.NorthEast), "error in isNeighbor");
		Assert (testBlock.isNeighbor (testBlockSouthEast).Equals (DIRECTION.SouthEast), "error in isNeighbor");
		Assert (testBlock.isNeighbor (testBlockNorthWest).Equals (DIRECTION.NorthWest), "error in isNeighbor");
		Assert (testBlock.isNeighbor (testBlockSouthWest).Equals (DIRECTION.SouthWest), "error in isNeighbor");
		
		//Check DIRECTION.Opposite is working
		Assert (DIRECTION.North == DIRECTION.South.Opposite (), "error with DIRECTION.NS");
		Assert (DIRECTION.South == DIRECTION.North.Opposite (), "error with DIRECTION.NS");
		Assert (DIRECTION.East == DIRECTION.West.Opposite (), "error with DIRECTION.EW");
		Assert (DIRECTION.West == DIRECTION.East.Opposite (), "error with DIRECTION.EW");
		Assert (DIRECTION.NorthEast == DIRECTION.SouthWest.Opposite (), "error with DIRECTION.NESW");
		Assert (DIRECTION.SouthWest == DIRECTION.NorthEast.Opposite (), "error with DIRECTION.NESW");
		Assert (DIRECTION.NorthWest == DIRECTION.SouthEast.Opposite (), "error with DIRECTION.NWSE");
		Assert (DIRECTION.SouthEast == DIRECTION.NorthWest.Opposite (), "error with DIRECTION.NWSE");

		testBlock = new TerrainBlock("block1", new Vector3(0,1,2), DIRECTION.North);
		Assert (testBlock.SaveString.Equals("0,1,2,North"), "save string error");


		print ("TerrainBlockTests: Tests Done");
	}

	void MapDataTests(){
		TerrainBlock testBlock = new TerrainBlock ("block1", new Vector3 (), DIRECTION.North);
		
		//Blocks in each cardinal and ordinal direction to testBlock
		TerrainBlock testBlockNorth = new TerrainBlock ("block1", new Vector3 (.0f, .0f, 1.0f), DIRECTION.North);
		TerrainBlock testBlockSouth = new TerrainBlock ("block1", new Vector3 (0f, .0f, -1.0f), DIRECTION.North);
		TerrainBlock testBlockEast = new TerrainBlock ("block1", new Vector3 (1f, .0f, .0f), DIRECTION.North);
		TerrainBlock testBlockWest = new TerrainBlock ("block1", new Vector3 (-1f, .0f, .0f), DIRECTION.North);
		TerrainBlock testBlockNorthEast = new TerrainBlock ("block1", new Vector3 (1f, .0f, 1.0f), DIRECTION.North);
		TerrainBlock testBlockSouthEast = new TerrainBlock ("block1", new Vector3 (1f, .0f, -1.0f), DIRECTION.North);
		TerrainBlock testBlockNorthWest = new TerrainBlock ("block1", new Vector3 (-1f, .0f, 1.0f), DIRECTION.North);
		TerrainBlock testBlockSouthWest = new TerrainBlock ("block1", new Vector3 (-1f, .0f, -1.0f), DIRECTION.North);
	
	}
}
