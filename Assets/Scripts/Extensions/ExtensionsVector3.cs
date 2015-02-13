using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class ExtensionsVector3
{
	/*
	 * public static Vector3 Round(this Vector3 vec)
	 * 
	 * Extension method for Vector3
	 * 
	 * returns a new Vector3 with x, y and z rounded to the nearest integer.
	 */
	public static Vector3 Round(this Vector3 vec){
		Vector3 retVal = new Vector3 ();
		retVal.x = Mathf.Round (vec.x);
		retVal.y = Mathf.Round (vec.y);
		retVal.z = Mathf.Round (vec.z);
		return retVal;
	}

	/*
	 * public static string toCSV(this Vector3 vec)
	 * 
	 * Extension method for Vector3
	 * 
	 * Returns a comma seperated list of a Vector3's values as a string
	 * Decimals with abs value < 1 have a leading 0
	 */
	public static string toCSV(this Vector3 vec){
		return string.Format ("{0},{1},{2}", vec.x, vec.y, vec.z);
	}

	/*
	 * public static Vector3 RotateTo(this Vector3 vec, DIRECTION dir)
	 * 
	 * Extension method for Vector3
	 * 
	 * Returns a new vector rotated around 0,0,0 to face in DIRECTION dir
	 * Assumes starting direction is North
	 * Currently only supports Cardinal Directions
	 */
	public static Vector3 RotateTo(this Vector3 vec, DIRECTION dir){
		if(!dir.isCardinal()) {
			throw new UnityException("Invalid Direction Arguement to Vector3.RotateTo. Must be Cardinal Direction.");
		}
		Vector3 retVal = vec.Copy();
		switch (dir) {
		case DIRECTION.North:
			retVal = Quaternion.Euler(0, 0, 0) * vec;
			break;
		case DIRECTION.East:
			retVal = Quaternion.Euler(0, 90, 0) * vec;
			break;
		case DIRECTION.South:
			retVal = Quaternion.Euler(0, 180, 0) * vec;
			break;
		case DIRECTION.West:
			retVal = Quaternion.Euler(0, 270, 0) * vec;
			break;
		}
		return retVal.Round();
	}

	/*
	 * public static Vector3 getMinVals(this Vector3 vec, Vector3 other)
	 * 
	 * Extension method for Vector3
	 * 
	 * Returns a new Vector3 with the smallest x, y, and z values of the input vectors
	 */
	public static Vector3 getMinVals(this Vector3 vec, Vector3 other){
		Vector3 retVal = new Vector3();
		retVal.x = Mathf.Min(vec.x, other.x);
		retVal.y = Mathf.Min(vec.y, other.y);
		retVal.z = Mathf.Min(vec.z, other.z);
		return retVal;
	}

	/*
	 * public static Vector3 getMaxVals(this Vector3 vec, Vector3 other)
	 * 
	 * Extension method for Vector3
	 * 
	 * Returns a new Vector3 with the largest x, y, and z values of the input vectors
	 */
	public static Vector3 getMaxVals(this Vector3 vec, Vector3 other){
		Vector3 retVal = new Vector3();
		retVal.x = Mathf.Max(vec.x, other.x);
		retVal.y = Mathf.Max(vec.y, other.y);
		retVal.z = Mathf.Max(vec.z, other.z);
		return retVal;
	}

	/*
	 * public static Vector3 Copy(this Vector3 vec)
	 * 
	 * Extension method for Vector3
	 * 
	 * Returns a new Vector3 with the same x/y/z as the input
	 */
	public static Vector3 Copy(this Vector3 vec){
		Vector3 retVal = new Vector3();
		retVal.Set(vec.x, vec.y, vec.z);
		return retVal;
	}

	public static DIRECTION toDirection(this Vector3 vec){
		float val = vec.y;
		val /= 360;
		val /= 45;
		int intval = Mathf.RoundToInt(val);
		switch(intval) {
		case 0:
			return DIRECTION.North;
		case 1:
			return DIRECTION.NorthEast;
		case 2:
			return DIRECTION.East;
		case 3:
			return DIRECTION.SouthEast;
		case 4:
			return DIRECTION.South;
		case 5:
			return DIRECTION.SouthWest;
		case 6:
			return DIRECTION.West;
		case 7:
			return DIRECTION.NorthWest;
		default:
			return DIRECTION.NonDirectional;
		}
	}

}


