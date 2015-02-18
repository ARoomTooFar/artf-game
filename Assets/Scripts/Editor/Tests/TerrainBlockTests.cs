using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

[TestFixture]
public class TerrainBlockTests : MasterTest{
	[Test]
	public void addNeighborTest(){
		TerrainBlock b1 = new TerrainBlock ("block1", new Vector3 (), DIRECTION.North);
		TerrainBlock b2 = new TerrainBlock ("block1", new Vector3 (), DIRECTION.North);
		TerrainBlock b3 = new TerrainBlock ("block1", new Vector3 (), DIRECTION.North);
		
		Assert.Throws<Exception>(() => b1.addNeighbor(b2, DIRECTION.NonDirectional));
		b1.addNeighbor(b2, DIRECTION.North);
		Assert.IsFalse(b1.addNeighbor(b3, DIRECTION.North));
		Assert.AreEqual(b2, b1.getNeighbor(DIRECTION.North));
	}

	[Test]
	public void isNeighborTest(){
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

		Assert.AreEqual(DIRECTION.NonDirectional, testBlockNorth.isNeighbor(testBlockSouth));

		Assert.AreEqual(DIRECTION.North, testBlock.isNeighbor(testBlockNorth));
		Assert.AreEqual(DIRECTION.South, testBlock.isNeighbor(testBlockSouth));
		Assert.AreEqual(DIRECTION.East, testBlock.isNeighbor(testBlockEast));
		Assert.AreEqual(DIRECTION.West, testBlock.isNeighbor(testBlockWest));

		Assert.AreEqual(DIRECTION.NorthEast, testBlock.isNeighbor(testBlockNorthEast));
		Assert.AreEqual(DIRECTION.NorthWest, testBlock.isNeighbor(testBlockNorthWest));
		Assert.AreEqual(DIRECTION.SouthEast, testBlock.isNeighbor(testBlockSouthEast));
		Assert.AreEqual(DIRECTION.SouthWest, testBlock.isNeighbor(testBlockSouthWest));
	}
}
