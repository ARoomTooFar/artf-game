using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

[TestFixture]
public class ARTFRoomTests : MasterTest{
	[Test]
	public void CornerTests(){
		ARTFRoom test = new ARTFRoom(new Vector3(0, 0, 5), new Vector3(5, 0, 0));
		Assert.AreEqual(new Vector3(0, 0, 0), test.LLCorner);
		Assert.AreEqual(new Vector3(5, 0, 5), test.URCorner);
		Assert.AreEqual(new Vector3(0, 0, 5), test.ULCorner);
		Assert.AreEqual(new Vector3(5, 0, 0), test.LRCorner);
	}

	[Test]
	public void DimensionsTests(){
		ARTFRoom test = new ARTFRoom(new Vector3(-2, 0, -1), new Vector3(2, 0, 1));
		Assert.AreEqual(5, test.Length);
		Assert.AreEqual(3, test.Height);
		Assert.AreEqual(16, test.Perimeter);
		Assert.AreEqual(15, test.Area);
	}

	[Test]
	public void LinkTerrainTest(){
		MapData.ClearData();
		test_ARTFRoom test = new test_ARTFRoom(new Vector3(0, 0, 0), new Vector3(1, 0, 1));
		Assert.AreEqual(0, test_MapData.Terrain.numTiles());
		test.linkTerrain();
		Assert.AreEqual(4, test.Blocks.Count);
		Assert.AreEqual(4, test_MapData.Terrain.numTiles());
		test.unlinkTerrain();
		Assert.AreEqual(0, test.Blocks.Count);
	}

	[Test]
	public void MoveTest(){
		MapData.ClearData();
		test_ARTFRoom test = new test_ARTFRoom(new Vector3(1, 0, 1), new Vector3(2, 0, 2));
		test.linkTerrain();
		Assert.IsNull(test_MapData.Terrain.findBlock(new Vector3(4, 0, 3)));
		float oLength = test.Length;
		float oHeight = test.Height;
		test.Move(new Vector3(2, 0, 1));
		Assert.AreEqual(new Vector3(3, 0, 2), test.LLCorner);
		Assert.AreEqual(new Vector3(4, 0, 3), test.URCorner);
		Assert.IsNotNull(test_MapData.Terrain.findBlock(new Vector3(4, 0, 3)));
		Assert.AreEqual(oLength, test.Length);
		Assert.AreEqual(oHeight, test.Height);
	}

	[Test]
	public void ResizeTest(){
		MapData.ClearData();
		test_ARTFRoom test = new test_ARTFRoom(new Vector3(0, 0, 0), new Vector3(1, 0, 1));
		test.linkTerrain();
		Assert.IsNull(test_MapData.Terrain.findBlock(new Vector3(2, 0, 2)));
		test.Resize(new Vector3(1, 0, 1), new Vector3(2, 0, 2));
		Assert.AreEqual(new Vector3(0, 0, 0), test.LLCorner);
		Assert.AreEqual(new Vector3(2, 0, 2), test.URCorner);
		Assert.AreEqual(3, test.Length);
		Assert.AreEqual(3, test.Height);
		Assert.IsNotNull(test_MapData.Terrain.findBlock(new Vector3(0, 0, 0)));
		Assert.IsNotNull(test_MapData.Terrain.findBlock(new Vector3(2, 0, 2)));
	}

	[Test]
	public void TilePositionTests(){
		test_ARTFRoom test = new test_ARTFRoom(new Vector3(0, 0, 0), new Vector3(5, 0, 5));
		Vector3 pos1 = new Vector3(0, 0, 0);
		Vector3 pos2 = new Vector3(1, 0, 1);
		Vector3 pos3 = new Vector3(-1, 0, -1);

		Assert.IsTrue(test.isCorner(pos1));
		Assert.IsTrue(test.isEdge(pos1));
		Assert.IsTrue(test.inRoom(pos1));

		Assert.IsFalse(test.isEdge(pos2));
		Assert.IsFalse(test.isCorner(pos2));
		Assert.IsTrue(test.inRoom(pos2));

		Assert.IsFalse(test.isEdge(pos3));
		Assert.IsFalse(test.isCorner(pos3));
		Assert.IsFalse(test.inRoom(pos3));
	}
}
