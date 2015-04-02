using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode : AbstractNode, IEquatable<RoomNode> {
	public RoomNode (ARTFRoom rm, SceneryBlock door){
		this.Room = rm;
		this.Door = door;
		CostSoFar = 0;
	}
	public ARTFRoom Room {
		get;
		private set;
	}

	public SceneryBlock Door {
		get;
		private set;
	}
	
	public bool Equals(RoomNode rmn){
		return this.Room.Equals(rmn.Room);
	}
	
	public override List<KeyValuePair<AbstractNode, float>> getConnections() {
		List<KeyValuePair<AbstractNode, float>> retVal = new List<KeyValuePair<AbstractNode, float>>();
		foreach(KeyValuePair<SceneryBlock, ARTFRoom> kvp in this.Room.LinkedRooms ) {
			retVal.Add(new KeyValuePair<AbstractNode, float>(new RoomNode(kvp.Value, kvp.Key),
			                                                 Vector3.Distance(kvp.Value.Center, kvp.Value.Center)));
		}
		return retVal;
	}
	
	public override bool Equals(AbstractNode node){
		if(node is RoomNode) {
			return this.Equals(node as RoomNode);
		}
		return false;
	}

	public override string ToString(){
		return Room.ToString();
	}

	public override Vector3 position(){
		return Room.Center;
	}
}

