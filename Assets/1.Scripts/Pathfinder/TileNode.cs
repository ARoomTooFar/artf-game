using System;
using System.Collections.Generic;
using UnityEngine;

public class TileNode : AbstractNode, IEquatable<TileNode> {
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
			retVal.Add(new KeyValuePair<AbstractNode, float>(new TileNode(blk), Vector3.Distance(blk.Position, this.terBlock.Position)));
		}
		return retVal;
	}

	public override string ToString(){
		return terBlock.Position.toCSV();
	}
}

