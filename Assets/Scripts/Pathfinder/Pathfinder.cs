
using System;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinder {

	private static List<AbstractNode> getPath(AbstractNode start, AbstractNode end){
		List<AbstractNode> open = new List<AbstractNode>();
		List<AbstractNode> closed = new List<AbstractNode>();
		open.Add(start);
		
		AbstractNode current = null;
		
		while(open.Count > 0) {
			open.Sort((first, second) => first.CostSoFar.CompareTo(second.CostSoFar));
			
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
		retVal.Add(current);;
		retVal.Reverse();
		return retVal;
	} 

	public static List<ARTFRoom> getRoomPath(ARTFRoom start, ARTFRoom end){
		List<AbstractNode> path = getPath(new RoomNode(start), new RoomNode(end));
		List<ARTFRoom> retVal = new List<ARTFRoom>();
		foreach(RoomNode node in path) {
			retVal.Add(node.Room);
		}
		return retVal;
	}

	public static List<Vector3> getSingleRoomPath(Vector3 start, Vector3 end){
		if(MapData.TheFarRooms.find(start) != MapData.TheFarRooms.find(end)) {
			return null;
		}

		List<AbstractNode> path = getPath(new TileNode(MapData.TerrainBlocks.find(start)),
		                                  new TileNode(MapData.TerrainBlocks.find(end)));
		List<Vector3> retVal = new List<Vector3>();
		foreach(TileNode node in path) {
			retVal.Add(node.terBlock.Position);
		}
		return retVal;
	}

	private abstract class AbstractNode : IEquatable<AbstractNode> {
		public float CostSoFar{ get; set; }

		public AbstractNode FromNode { get; set; }

		public abstract List<KeyValuePair<AbstractNode, float>> getConnections();

		public abstract bool Equals(AbstractNode node);
	}

	private class RoomNode : AbstractNode, IEquatable<RoomNode> {
		public RoomNode (ARTFRoom rm){
			this.Room = rm;
			CostSoFar = 0;
		}
		public ARTFRoom Room {
			get;
			private set;
		}

		public bool Equals(RoomNode rmn){
			return this.Room.Equals(rmn.Room);
		}

		public override List<KeyValuePair<AbstractNode, float>> getConnections() {
			List<KeyValuePair<AbstractNode, float>> retVal = new List<KeyValuePair<AbstractNode, float>>();
			foreach(ARTFRoom rm in this.Room.LinkedRooms.Values ) {
				retVal.Add(new KeyValuePair<AbstractNode, float>(new RoomNode(rm), Vector3.Distance(rm.Center, Room.Center)));
			}
			return retVal;
		}

		public override bool Equals(AbstractNode node){
			if(node is RoomNode) {
				return this.Equals(node as RoomNode);
			}
			return false;
		}
	}

	private class TileNode : AbstractNode, IEquatable<TileNode> {
		public TileNode(TerrainBlock pos){
			this.terBlock = pos;
			CostSoFar = 0;
		}

		public TerrainBlock terBlock {
			get;
			private set;
		}

		public bool Equals(TileNode tln){
			return this.terBlock.Position.Equals(tln.terBlock.Position);
		}

		public override bool Equals(AbstractNode node){
			if(node is TileNode) {
				return this.Equals(node as TileNode);
			}
			return false;
		}

		public override List<KeyValuePair<AbstractNode, float>> getConnections() {
			List<KeyValuePair<AbstractNode, float>> retVal = new List<KeyValuePair<AbstractNode, float>>();
			foreach(TerrainBlock blk in this.terBlock.Neighbors.Values) {
				if(!blk.Walkable){
					continue;
				}
				retVal.Add(new KeyValuePair<AbstractNode, float>(new TileNode(blk), Vector3.Distance(blk.Position, terBlock.Position)));
			}
			return retVal;
		}
	}
}