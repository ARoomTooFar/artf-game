using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomVertex {

	Vector3 position;
	Dictionary<DIRECTION, RoomVertex> neighbors;

	private RoomVertex(Vector3 position) {
		this.position = position.Round();
		neighbors = new Dictionary<DIRECTION, RoomVertex>();
	}

	
}
