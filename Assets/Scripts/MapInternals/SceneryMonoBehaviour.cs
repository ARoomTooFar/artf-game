using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneryMonoBehaviour : BlockMonoBehaviour {

	public List<Vector3> Coordinates;

	public bool isDoor;

	public List<Vector3> LocalCoordinates(DIRECTION dir) {
		List<Vector3> retVal = new List<Vector3>();
		foreach(Vector3 vec in Coordinates) {
			retVal.Add(vec.RotateTo(dir));
		}
		return retVal;
	}
}
