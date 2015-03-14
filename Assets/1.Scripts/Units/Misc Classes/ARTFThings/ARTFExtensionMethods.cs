using UnityEngine;
using System.Collections;

public static class ARTFExtensionMethods {
	// Look at all this buttsex
	public static bool isBehind(this Transform pos, Vector3 tarFacing, Vector3 tarPosition) {
		return ARTFUtilities.isBehind(pos.position, tarFacing, tarPosition);
	}

	public static bool isOnSide(this Transform pos, Vector3 tarFacing, Vector3 tarPosition) {
		return ARTFUtilities.isOnSide(pos.position, tarFacing, tarPosition);
	}
}