// Collection of extension methods, may need to split up if it ever expands
//     ATM, it's just extension styles of functions in ARTFUtilities

using UnityEngine;
using System.Collections;

public static class ARTFExtensionMethods {
	public static bool IsBehind(this Transform pos, Vector3 tarFacing, Vector3 tarPosition) {
		return ARTFUtilities.IsBehind(pos.position, tarFacing, tarPosition);
	}

	public static bool IsOnSide(this Transform pos, Vector3 tarFacing, Vector3 tarPosition) {
		return ARTFUtilities.IsOnSide(pos.position, tarFacing, tarPosition);
	}

	public static bool CanSeeObject(this Character unit, GameObject target, int layerMask = ~0) {
		return ARTFUtilities.CanSeeObject(unit, target, layerMask);
	}

	public static Vector3 GetFacingTowardsObject (this Character unit, GameObject target) {
		return ARTFUtilities.GetFacingTowardsObject (unit, target);
	}
	
	public static Vector3 AngledArcTrajectory (this Transform start, Vector3 destination, float angle) {
		return ARTFUtilities.AngledArcTrajectory (start.position, destination, angle);
	}
	
	public static Vector3 SpeedArcTrajectory (this Transform start, Vector3 destination, float speed) {
		return ARTFUtilities.SpeedArcTrajectory (start.position, destination, speed);
	}
}