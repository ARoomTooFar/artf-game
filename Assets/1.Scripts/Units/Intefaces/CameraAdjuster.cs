using UnityEngine;
using System.Collections;

public class CameraAdjuster : MonoBehaviour {
	public GameObject p1;
	public GameObject p2;
	public GameObject p3;
	public GameObject p4;
	//Average X, Z (Pulled from Player Locations)
	//Base X,Y,Z (To be adjusted by the multiplier Value)
    //Value of Adjustment
	public float avgX,avgZ,baseY,baseX,baseZ,adjVal,supBaseY;
	public bool buut;
	// Use this for initialization
	void Start () {
		adjVal = 2.5f;
		baseY = 15f;
		supBaseY = baseY;
		baseX = baseY/2 + adjVal;
		baseZ = -(baseY/2 + adjVal);
		avgX = (p1.transform.position.x + p2.transform.position.x + p3.transform.position.x + p4.transform.position.x)/4 + baseX;
		avgZ = (p1.transform.position.z + p2.transform.position.z + p3.transform.position.z + p4.transform.position.z)/4 + baseZ;
	}
	
	// Update is called once per frame
	void Update () {
		if((Mathf.Abs(p1.transform.position.x - p2.transform.position.x) > baseY) || (Mathf.Abs(p2.transform.position.x - p3.transform.position.x) > baseY) || (Mathf.Abs(p1.transform.position.x - p3.transform.position.x) > baseY) || (Mathf.Abs(p3.transform.position.x - p1.transform.position.x) > baseY) ||
		   (Mathf.Abs(p1.transform.position.x - p4.transform.position.x) > baseY) || (Mathf.Abs(p4.transform.position.x - p1.transform.position.x) > baseY) || (Mathf.Abs(p2.transform.position.x - p3.transform.position.x) > baseY) || (Mathf.Abs(p3.transform.position.x - p2.transform.position.x) > baseY) ||
		   (Mathf.Abs(p2.transform.position.x - p4.transform.position.x) > baseY) || (Mathf.Abs(p4.transform.position.x - p2.transform.position.x) > baseY) || (Mathf.Abs(p1.transform.position.x - p2.transform.position.x) > baseY) || (Mathf.Abs(p3.transform.position.x - p4.transform.position.x) > baseY)){
			baseY += .1f;
		}
		if((Mathf.Abs(p1.transform.position.x + p2.transform.position.x) < baseY/2) && (Mathf.Abs(p2.transform.position.x + p3.transform.position.x) < baseY/2) && (Mathf.Abs(p1.transform.position.x + p3.transform.position.x) < baseY/2) && (Mathf.Abs(p3.transform.position.x + p1.transform.position.x) < baseY/2) &&
		    (Mathf.Abs(p1.transform.position.x + p4.transform.position.x) < baseY/2) && (Mathf.Abs(p4.transform.position.x + p1.transform.position.x) < baseY/2) && (Mathf.Abs(p2.transform.position.x + p3.transform.position.x) < baseY/2) && (Mathf.Abs(p3.transform.position.x + p2.transform.position.x) < baseY/2) &&
		    (Mathf.Abs(p2.transform.position.x + p4.transform.position.x) < baseY/2) && (Mathf.Abs(p4.transform.position.x + p2.transform.position.x) < baseY/2) && (Mathf.Abs(p1.transform.position.x + p2.transform.position.x) < baseY/2) && (Mathf.Abs(p3.transform.position.x + p4.transform.position.x) < baseY/2)){
			baseY -= .1f;
			if(baseY < supBaseY){
				baseY = supBaseY;
			}
		}
		baseX = baseY/2 + adjVal;
		baseZ = -(baseY/2 + adjVal);
		avgX = (p1.transform.position.x + p2.transform.position.x + p3.transform.position.x + p4.transform.position.x)/4 + baseX;
		avgZ = (p1.transform.position.z + p2.transform.position.z + p3.transform.position.z + p4.transform.position.z)/4 + baseZ;
		transform.position = new Vector3(avgX,baseY,avgZ);
		
	}
}
