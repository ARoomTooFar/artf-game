using UnityEngine;
using System.Collections;

public class CameraAdjuster : MonoBehaviour {
	public GameObject p1; //
	public GameObject p2;//
	public GameObject p3;//
	public GameObject p4;//
	//Average X, Z (Pulled from Player Locations)
	//Base X,Y,Z (To be adjusted by the multiplier Value)
    //Value of Adjustment
	public float avgX,avgZ,baseY,baseX,baseZ,adjVal,supBaseY;
	// Use this for initialization
	void Start () {
		transform.rotation = Quaternion.Euler(45,-45,0);
		//transform.rotation = Quaternion.Euler(90,0,0);
		//arbitrarily decided point
		adjVal = 2.5f;
		//base height
		baseY = 15f;
		//Base case reminder
		supBaseY = baseY;
		//Adjusted value points
		baseX = baseY/2 + adjVal;
		baseZ = -(baseY/2 + adjVal);
		//Average the x's and z's for the target location of the camera's focus
		avgX = (p1.transform.position.x + p2.transform.position.x + p3.transform.position.x + p4.transform.position.x)/4 + baseX; // 
		avgZ = (p1.transform.position.z + p2.transform.position.z + p3.transform.position.z + p4.transform.position.z)/4 + baseZ; //
	}
	
	// Update is called once per frame
	void Update () {
		//Check if they are far enough to need to stretch the distancing
		if((Mathf.Abs(p1.transform.position.x - p2.transform.position.x) > baseY) || (Mathf.Abs(p2.transform.position.x - p3.transform.position.x) > baseY) || (Mathf.Abs(p1.transform.position.x - p3.transform.position.x) > baseY) || (Mathf.Abs(p3.transform.position.x - p1.transform.position.x) > baseY) ||
		   (Mathf.Abs(p1.transform.position.x - p4.transform.position.x) > baseY) || (Mathf.Abs(p4.transform.position.x - p1.transform.position.x) > baseY) || (Mathf.Abs(p2.transform.position.x - p3.transform.position.x) > baseY) || (Mathf.Abs(p3.transform.position.x - p2.transform.position.x) > baseY) ||
		   (Mathf.Abs(p2.transform.position.x - p4.transform.position.x) > baseY) || (Mathf.Abs(p4.transform.position.x - p2.transform.position.x) > baseY) || (Mathf.Abs(p1.transform.position.x - p2.transform.position.x) > baseY) || (Mathf.Abs(p3.transform.position.x - p4.transform.position.x) > baseY)){
			baseY += .1f;
		}
		//Check if they are too close to need to shrink the distancing
		if((Mathf.Abs(p1.transform.position.x + p2.transform.position.x) < baseY/2) && (Mathf.Abs(p2.transform.position.x + p3.transform.position.x) < baseY/2) && (Mathf.Abs(p1.transform.position.x + p3.transform.position.x) < baseY/2) && (Mathf.Abs(p3.transform.position.x + p1.transform.position.x) < baseY/2) &&
		    (Mathf.Abs(p1.transform.position.x + p4.transform.position.x) < baseY/2) && (Mathf.Abs(p4.transform.position.x + p1.transform.position.x) < baseY/2) && (Mathf.Abs(p2.transform.position.x + p3.transform.position.x) < baseY/2) && (Mathf.Abs(p3.transform.position.x + p2.transform.position.x) < baseY/2) &&
		    (Mathf.Abs(p2.transform.position.x + p4.transform.position.x) < baseY/2) && (Mathf.Abs(p4.transform.position.x + p2.transform.position.x) < baseY/2) && (Mathf.Abs(p1.transform.position.x + p2.transform.position.x) < baseY/2) && (Mathf.Abs(p3.transform.position.x + p4.transform.position.x) < baseY/2)){
			baseY -= .1f;
			//So it won't go too low
			if(baseY < supBaseY){
				baseY = supBaseY;
			}
		}
		//Same adjustment values for X and Z as start
		baseX = baseY/2 + adjVal;
		baseZ = -(baseY/2 + adjVal);
		//Average the x's and z's for the target location of the camera's focus
		avgX = (p1.transform.position.x + p2.transform.position.x + p3.transform.position.x + p4.transform.position.x)/4 + baseX;
		avgZ = (p1.transform.position.z + p2.transform.position.z + p3.transform.position.z + p4.transform.position.z)/4 + baseZ;
		//Translate the camera after everything is done
		transform.position = new Vector3(avgX,baseY,avgZ);
		
	}
}
