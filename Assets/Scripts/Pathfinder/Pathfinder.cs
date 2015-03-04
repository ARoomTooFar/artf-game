
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder {

	/*
	public List<Vector3> getPath(Vector3 start, Vector3 end){

	}

	public List<Vector3> getWalkablePath(Vector3 start, Vector3 end){

	}*/

	public static  List<ARTFRoom> getRoomPath(ARTFRoom start, ARTFRoom end){
		List<RoomNode> open = new List<RoomNode>();
		List<RoomNode> closed = new List<RoomNode>();
		open.Add(new RoomNode(start));

		RoomNode current = null;

		while(open.Count > 0) {
			open.Sort((first, second) => first.CostSoFar.CompareTo(second.CostSoFar));

			current = open[0];

			if(current.Room == end) {
				break;
			}

			foreach(KeyValuePair<ARTFRoom, float> kvp in current.Connections) {
				RoomNode endNode = new RoomNode(kvp.Key);
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

		if(current.Room != end) {
			return null;
		}

		List<ARTFRoom> retVal = new List<ARTFRoom>();
		while(current.Room != start) {
			retVal.Add(current.Room);
			current = current.FromNode;
		}
		retVal.Reverse();
		return retVal;
	} 

	private class RoomNode : IEquatable<RoomNode> {
		public RoomNode (ARTFRoom rm){
			this.Room = rm;
			CostSoFar = 0;
		}
		public ARTFRoom Room {
			get;
			private set;
		}
		public float CostSoFar{ get; set; }

		public RoomNode FromNode { get; set; }

		public bool Equals(RoomNode rmn){
			return this.Room.Equals(rmn.Room);
		}
		public List<KeyValuePair<ARTFRoom, float>> Connections {
			get {
				List<KeyValuePair<ARTFRoom, float>> retVal = new List<KeyValuePair<ARTFRoom, float>>();
				foreach(ARTFRoom rm in this.Room.LinkedRooms) {
					retVal.Add(new KeyValuePair<ARTFRoom, float>(rm, Vector3.Distance(rm.Center, Room.Center)));
				}
				return retVal;
			}
		}
	}
}