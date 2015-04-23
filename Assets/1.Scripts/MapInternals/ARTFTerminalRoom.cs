using System;
using UnityEngine;

public class ARTFTerminalRoom : ARTFRoom {
	public ARTFTerminalRoom(Vector3 pos1, Vector3 pos2) : base(pos1, pos2) {
		if(Global.inLevelEditor) {
			GameObjectResourcePool.returnResource("Prefabs/RoomCorner", URMarker);
			GameObjectResourcePool.returnResource("Prefabs/RoomCorner", LLMarker);
			GameObjectResourcePool.returnResource("Prefabs/RoomCorner", ULMarker);
			GameObjectResourcePool.returnResource("Prefabs/RoomCorner", LRMarker);
			URMarker = null;
			LLMarker = null;
			ULMarker = null;
			LRMarker = null;
		}
	}

	public override void updateMarkerPositions(){}

	public override void setMarkerActive(bool active){}
}


