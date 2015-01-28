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
	 * returns a comma seperated list of a Vector3's values as a string
	 */
	public static string toCSV(this Vector3 vec){
		return string.Format ("{0},{1},{2}", vec.x, vec.y, vec.z);
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
	
}