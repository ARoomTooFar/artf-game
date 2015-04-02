using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractNode : IEquatable<AbstractNode> {
	public float CostSoFar{ get; set; }
	
	public AbstractNode FromNode { get; set; }
	
	public abstract List<KeyValuePair<AbstractNode, float>> getConnections();
	
	public abstract bool Equals(AbstractNode node);

	public abstract string ToString();

	public abstract Vector3 position();
}

