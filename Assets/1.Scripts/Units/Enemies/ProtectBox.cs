using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProtectBox
{
	//Corner x pos, corner x neg, corner y pos, corner y neg
	private Vector3 targetV;

	public ProtectBox(GameObject ptarget)
	{
		targetV = ptarget.transform.position;

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

		float xmin = targetV.x + ((firstR) * (5.0f));
		float xmax = targetV.x + ((firstR) * (10.0f));

		float zmin = targetV.z + ((secondR) * (5.0f));
		float zmax = targetV.z + ((secondR) * (10.0f));

		float x = Random.Range (xmin, xmax);
		float z = Random.Range (zmin, zmax);

		Vector3 targetPos = new Vector3 (x, targetV.y, z);
		return targetPos;
	}

}
