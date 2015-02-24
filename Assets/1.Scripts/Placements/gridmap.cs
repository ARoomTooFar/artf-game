using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class p_node {
	Vector2 position;
	bool reachable;
	float heuristic;
}

public class path{
	List<p_node> theway;
}

public class gridmap {

	List< List<p_node> > gamemap;

	void Awake() {

	}

	void Update () {
		
	}
}
