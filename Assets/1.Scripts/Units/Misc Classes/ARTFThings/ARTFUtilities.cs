using UnityEngine;
using System.Collections;

public static class ARTFUtilities {
	// Look at all this buttsex
	public static bool isBehind(Transform pos, Vector3 tarFacing, Vector3 tarPosition) {
		return isBehind(pos.position, tarFacing, tarPosition);
	}

	public static bool isBehind(Vector3 pos, Vector3 tarFacing, Vector3 tarPosition) {
		float angle = Vector2.Angle(new Vector2(tarPosition.x - pos.x, tarPosition.z - pos.z), new Vector2(tarFacing.x, tarFacing.z));

		if (angle < 45.0f) {
			return true;
		} else {
			return false;
		}
	}

	// Side Functions
	public static bool isOnSide(Transform pos, Vector3 tarFacing, Vector3 tarPosition) {
		return isOnSide (pos.position, tarFacing, tarPosition);
	}

	public static bool isOnSide(Vector3 pos, Vector3 tarFacing, Vector3 tarPosition) {
		float angle = Vector2.Angle(new Vector2(tarPosition.x - pos.x, tarPosition.z - pos.z), new Vector2(tarFacing.x, tarFacing.z));

		if (angle < 135.0f) {
			return true;
		} else {
			return false;
		}
	}
}