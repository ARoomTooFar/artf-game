using System;
using UnityEngine;

public class ARTFTerminalRoom : ARTFRoom {
	public ARTFTerminalRoom(Vector3 pos1, Vector3 pos2) : base(pos1, pos2) {
		if(Global.inLevelEditor) {
			foreach(GameObject cor in CornerMarkers) {
				GameObjectResourcePool.returnResource(roomCornerId, cor);
			}
			Corners.Clear();
		}
	}

	public override void updateMarkerPositions(){}

	public override void setMarkerActive(bool active){}
}


