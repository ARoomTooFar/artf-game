using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProtectBox
{
	public float range = 1.5f;

	private Vector3 targetV;

	public ProtectBox(Vector3 target)
	{
		targetV = target;

	}

	public Vector3 getProtectV()
	{
		//Generates a Vector3 around the target, but not too close to it

		//First generate 2 random +s or -s
		float firstR = Random.value;
		float secondR = Random.value;

		if(firstR < 0.5f){
			firstR = -1.0f;
		}else {
			firstR = 1.0f;
		}

		if (secondR < 0.5f){
			secondR = -1.0f;
		}else {
			secondR = 1.0f;
		}

		//First random is horizontal
		//Second random is vertical

		float xmin = targetV.x + ((firstR) * (range/2));
		float xmax = targetV.x + ((firstR) * (range));

		float zmin = targetV.z + ((secondR) * (range/2));
		float zmax = targetV.z + ((secondR) * (range));

		float x = Random.Range (xmin, xmax);
		float z = Random.Range (zmin, zmax);

		Vector3 targetPos = new Vector3 (x, 1.605f, z);
		return targetPos;
	}

}
