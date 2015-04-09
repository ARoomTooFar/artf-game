using UnityEngine;
using System.Collections;

public static class ARTFUtilities {
	// Checks if the the given position/vector3 is behind something at position with facing
	//     Based off of 45degree angles
	public static bool IsBehind(Transform pos, Vector3 tarFacing, Vector3 tarPosition) {
		return IsBehind(pos.position, tarFacing, tarPosition);
	}

	public static bool IsBehind(Vector3 pos, Vector3 tarFacing, Vector3 tarPosition) {
		float angle = Vector2.Angle(new Vector2(tarPosition.x - pos.x, tarPosition.z - pos.z), new Vector2(tarFacing.x, tarFacing.z));

		if (angle < 45.0f) {
			return true;
		} else {
			return false;
		}
	}


	// Like is behind, but for the side
	public static bool IsOnSide(Transform pos, Vector3 tarFacing, Vector3 tarPosition) {
		return IsOnSide (pos.position, tarFacing, tarPosition);
	}

	public static bool IsOnSide(Vector3 pos, Vector3 tarFacing, Vector3 tarPosition) {
		float angle = Vector2.Angle(new Vector2(tarPosition.x - pos.x, tarPosition.z - pos.z), new Vector2(tarFacing.x, tarFacing.z));

		if (angle < 135.0f) {
			return true;
		} else {
			return false;
		}
	}
	
	
	// Calculates a trajectory for a parabolic motion given start and end location and an angle
	//     Angle is from y direction to forward direction not the starting angle from the ground
	//     Around 70 degrees it starts to land away fom the center due the height difference not being in the calculation
	//     It will break around values > 80.0f and values < 1.0f degrees
	public static Vector3 AngledArcTrajectory (Transform pos, Vector3 destination, float angle) {
		return AngledArcTrajectory(pos.position, destination, angle);
	}
	
	public static Vector3 AngledArcTrajectory (Vector3 start, Vector3 destination, float angle) {
		if (angle < 1.0f) Debug.LogError ("Angle given to AngledArcTrajectory is less than 1.0f");
		if (angle > 89.0f) Debug.LogError ("Angle given to AngledArcTrajectory is greater than 89.0f");
		angle = angle * Mathf.Deg2Rad;
	
		// Calculates distance from source to target
		float dist = Vector3.Distance (start, destination);
		
		// Get dir of z due to angle not accounting for this direction
		float zDir = 0.0f;
		if (destination.z - start.z != 0.0f) {
			zDir = (destination.z - start.z) / Mathf.Abs (destination.z - start.z);
		} 
		
		float speed = Mathf.Sqrt((9.81f * dist)/Mathf.Sin(angle * 2.0f));
		
		// Calculates angle between x and z
		Vector3 direction = destination - start;
		float newAngle = Vector3.Angle (Vector3.right, direction) * Mathf.Deg2Rad;
		
		// What speed to disburse between x and z after calculating speed for y
		float speedOverLand = Mathf.Sin (angle) * speed;
		return new Vector3(speedOverLand * Mathf.Cos (newAngle), speed * Mathf.Cos(angle), zDir * speedOverLand * Mathf.Sin (newAngle));
	}
	
	
	
	// Calculates a trajectory for a parabolic motion given start and end location and a speed
	//     Take note that there is a physical limit where it will be impossible for the object to hit something if the speed is too low or if the ojbect is really far
	//     If so, it will throw an error displaying how much you fucked up
	public static Vector3 SpeedArcTrajectory (Transform pos, Vector3 destination, float speed) {
		return SpeedArcTrajectory(pos.position, destination, speed);
	}
	
	public static Vector3 SpeedArcTrajectory (Vector3 start, Vector3 destination, float speed) {
		// Calculates distance from source to target
		float dist = Vector3.Distance (start, destination);
		
		// Get dir of z due to angle not accounting for this direction
		float zDir = 0.0f;
		if (destination.z - start.z != 0.0f) {
			zDir = (destination.z - start.z) / Mathf.Abs (destination.z - start.z);
		} 
		
		// Angle of elevation
		float angle = Mathf.Asin ((9.81f * dist) / (speed * speed)) / 2.0f;
		
		// Calculates angle between x and z
		Vector3 direction = destination - start;
		float newAngle = Vector3.Angle (Vector3.right, direction) * Mathf.Deg2Rad;
		
		// What speed to disburse between x and z after calculating speed for y
		float speedOverLand = Mathf.Sin (angle) * speed;
		return new Vector3(speedOverLand * Mathf.Cos (newAngle), speed * Mathf.Cos(angle), zDir * speedOverLand * Mathf.Sin (newAngle));
	
	}
}