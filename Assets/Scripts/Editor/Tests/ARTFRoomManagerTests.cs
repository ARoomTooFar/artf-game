using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

[TestFixture]
public class ARTFRoomManagerTests : MasterTest{
	[Test]
	public void doRoomsIntersectTest(){
		ARTFRoom r1 = new ARTFRoom(new Vector3(0, 0, 0), new Vector3(2, 0, 2));
		ARTFRoom r2 = new ARTFRoom(new Vector3(3, 0, 3), new Vector3(4, 0, 4));
		ARTFRoom r3 = new ARTFRoom(new Vector3(1, 0, 1), new Vector3(3, 0, 3));
		
		Assert.IsTrue(ARTFRoomManager.doRoomsIntersect(r1, r3));
		Assert.IsTrue(ARTFRoomManager.doRoomsIntersect(r2, r3));
		Assert.IsFalse(ARTFRoomManager.doRoomsIntersect(r1, r2));
	}
	
	[Test]
	public void addTest(){
		test_ARTFRoomManager rm = new test_ARTFRoomManager();
		rm.add(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
		Assert.AreEqual(1, rm.RoomList.Count);
		rm.add(new ARTFRoom(new Vector3(1, 0, 1), new Vector3(1, 0, 1)));
		Assert.AreEqual(2, rm.RoomList.Count);
	}
	
	[Test]
	public void doAnyRoomsIntersectTest(){
		ARTFRoomManager rm = new ARTFRoomManager();
		ARTFRoom room = new ARTFRoom(new Vector3(0, 0, 0), new Vector3(1, 0, 1));
		rm.add(room);

		ARTFRoom r1 = new ARTFRoom(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
		Assert.IsTrue(rm.doAnyRoomsIntersect(r1));
		
		ARTFRoom r2 = new ARTFRoom(new Vector3(-1, 0, -1), new Vector3(-1, 0, -1));
		Assert.IsFalse(rm.doAnyRoomsIntersect(r2));		
	}
	
	[Test]
	public void findTest(){
		ARTFRoom r1 = new ARTFRoom(new Vector3(0, 0, 0), new Vector3(2, 0, 2));
		ARTFRoom r2 = new ARTFRoom(new Vector3(3, 0, 3), new Vector3(4, 0, 4));
		test_ARTFRoomManager rm = new test_ARTFRoomManager();
		rm.add(r1);
		rm.add(r2);
		
		Assert.AreSame(r1, rm.find(new Vector3(1, 0, 1)));
		Assert.AreSame(r2, rm.find(new Vector3(3, 0, 4)));
		Assert.IsNull(rm.find(new Vector3(5, 0, 5)));
	}
	
	[Test]
	public void moveTest(){
		ARTFRoom r1 = new ARTFRoom(new Vector3(0, 0, 0), new Vector3(2, 0, 2));
		ARTFRoom r2 = new ARTFRoom(new Vector3(3, 0, 3), new Vector3(4, 0, 4));
		test_ARTFRoomManager rm = new test_ARTFRoomManager();
		rm.add(r1);
		rm.add(r2);

		rm.move(new Vector3(3, 0, 3), new Vector3(1, 0, 1));
		Assert.AreEqual(new Vector3(4, 0, 4), r2.LLCorner);
	}
	
	[Test]
	public void resizeTest(){
		ARTFRoom r1 = new ARTFRoom(new Vector3(0, 0, 0), new Vector3(2, 0, 2));
		ARTFRoom r2 = new ARTFRoom(new Vector3(3, 0, 3), new Vector3(6, 0, 6));
		test_ARTFRoomManager rm = new test_ARTFRoomManager();
		rm.add(r1);
		rm.add(r2);
		
		rm.resize(new Vector3(6, 0, 6), new Vector3(7, 0, 7));
		Assert.AreEqual(new Vector3(7, 0, 7), r2.URCorner);
	}
}
