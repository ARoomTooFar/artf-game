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

		if(firstR < 0.5){
			firstR = -1.0;
		}else {
			firstR = 1.0;
		}

		if (secondR < 0.5){
			secondR = -1.0;
		}else {
			secondR = 1.0;
		}

		//First random is horizontal
		//Second random is vertical

		float xmin = targetV.x + ((firstR) * (5.0));
		float xmax = targetV.x + ((firstR) * (10.0));

		float zmin = targetV.z + ((secondR) * (5.0));
		float zmax = targetV.z + ((secondR) * (10.0));

		float x = Random.Range (xmin, xmax);
		float z = Random.Range (zmin, zmax);

		Vector3 targetPos = new Vector3 (x, targetV.y, z);
		return targetPos;
	}

}
