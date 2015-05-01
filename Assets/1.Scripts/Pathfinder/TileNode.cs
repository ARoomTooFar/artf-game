using System;
using System.Collections.Generic;
using UnityEngine;

public class TileNode : AbstractNode, IEquatable<TileNode> {
	public TileNode(Vector3 pos){
		this.Position = pos;
		CostSoFar = 0;
	}

	public Vector3 Position {
		get;
		private set;
	}

	public bool Equals(TileNode tln){
		return this.Position.Equals(tln.Position);
	}
	
	public override bool Equals(AbstractNode node){
		if(node is TileNode) {
			return this.Equals(node as TileNode);
		}
		return false;
	}
	
	public override List<KeyValuePair<AbstractNode, float>> getConnections() {
		List<KeyValuePair<AbstractNode, float>> retVal = new List<KeyValuePair<AbstractNode, float>>();
		ARTFRoom rm = MapData.TheFarRooms.find (Position);
		List<Vector3> checkVals = new List<Vector3> ();
		checkVals.Add (Position.moveinDir (DIRECTION.North));
		checkVals.Add (Position.moveinDir (DIRECTION.South));
		checkVals.Add (Position.moveinDir (DIRECTION.East));
		checkVals.Add (Position.moveinDir (DIRECTION.West));

		foreach (Vector3 vec in checkVals) {
			if(!rm.inRoom(vec)){
				continue;
			}
			if(rm.isEdge(vec)){
				//continue;
			}
			if(!rm.isPathable(vec)){
				continue;
			}
			retVal.Add (new KeyValuePair<AbstractNode, float> (new TileNode (vec), Vector3.Distance (vec, Position)));
		}

		return retVal;
	}

	public override string ToString(){
		return Position.toCSV();
	}

	public override Vector3 position(){
		return Position;
	}
}

