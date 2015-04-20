
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder {

	private static List<AbstractNode> DijkstraPathfinder(AbstractNode start, AbstractNode end){
		List<AbstractNode> open = new List<AbstractNode>();
		List<AbstractNode> closed = new List<AbstractNode>();
		open.Add(start);
		
		AbstractNode current = null;
		
		while(open.Count > 0) {
			//open.Sort((first, second) => first.CostSoFar.CompareTo(second.CostSoFar));
			open.Sort((first, second) => Vector3.Distance(first.position(), end.position()).CompareTo(Vector3.Distance(second.position(), end.position())));

			current = open[0];
			
			if(current.Equals(end)) {
				break;
			}
			
			foreach(KeyValuePair<AbstractNode, float> kvp in current.getConnections()) {
				AbstractNode endNode = kvp.Key;
				endNode.FromNode = current;
				if(closed.Contains(endNode)) {
					continue;
				}
				float endNodeCost = current.CostSoFar + kvp.Value;
				if(open.Contains(endNode)) {
					endNode = open[open.IndexOf(endNode)];
					if(endNodeCost < endNode.CostSoFar) {
						endNode.CostSoFar = endNodeCost;
						endNode.FromNode = current;
					}
					continue;
				}
				
				endNode.CostSoFar = endNodeCost;
				open.Add(endNode);
			}
			
			open.Remove(current);
			closed.Add(current);
		}
		
		if(!current.Equals(end)) {
			return null;
		}
		
		List<AbstractNode> retVal = new List<AbstractNode>();
		while(current != start) {
			retVal.Add(current);
			current = current.FromNode;
		}
		retVal.Add(current);
		retVal.Reverse();
		return retVal;
	} 

	public static List<SceneryBlock> getRoomPath(ARTFRoom start, ARTFRoom end){
		List<AbstractNode> path = DijkstraPathfinder(new RoomNode(start, null), new RoomNode(end, null));
		if(path == null || path.Count == 0) {
			return null;
		}
		List<SceneryBlock> retVal = new List<SceneryBlock>();
		for(int i = 0; i < path.Count; ++i) {
			retVal.Add(((RoomNode)path[i]).Door);
		}
		return retVal;
	}

	public static List<Vector3> getSingleRoomPath(Vector3 start, Vector3 end){
		if(MapData.TheFarRooms.find(start) != MapData.TheFarRooms.find(end)) {
			return null;
		}

		List<AbstractNode> path = DijkstraPathfinder(new TileNode(MapData.TerrainBlocks.find(start)),
		                                  new TileNode(MapData.TerrainBlocks.find(end)));
		List<Vector3> retVal = new List<Vector3>();
		foreach(TileNode node in path) {
			retVal.Add(node.terBlock.Position);
		}
		return retVal;
	}

	public static List<Vector3> getPath(Vector3 start, Vector3 end){
		ARTFRoom sRoom = MapData.TheFarRooms.find(start);
		ARTFRoom eRoom = MapData.TheFarRooms.find(end);

		if(sRoom == null || eRoom == null) {
			return null;
		}

		if(sRoom.Equals(eRoom)) {
			return getSingleRoomPath(start, end);
		}

		List<SceneryBlock> roomPath = getRoomPath(sRoom, eRoom);
		if(roomPath == null) {
			return null;
		}

		List<Vector3> retVal = new List<Vector3>();

		List<Vector3> somePath = getSingleRoomPath(start, roomPath[1].Position);
		if(somePath == null) {
			return null;
		}
		retVal.AddRange(somePath);
		ARTFRoom r1;
		ARTFRoom r2;
		for(int i = 1; i < roomPath.Count-1; ++i){
			r1 = MapData.TheFarRooms.find(roomPath[i].Position);
			r2 = MapData.TheFarRooms.find(r1.getDoorCheckPosition(roomPath[i]));
			retVal.AddRange(r2.RoomPaths[new KeyValuePair<Vector3, Vector3>
			                             (r1.getDoorCheckPosition(roomPath[i]), roomPath[i+1].Position)]);
		}

		ARTFRoom rm = MapData.TheFarRooms.find(roomPath[roomPath.Count - 1].Position);

		retVal.AddRange(getSingleRoomPath(rm.getDoorCheckPosition(roomPath[roomPath.Count-1]), end));

		return retVal;

	}
}