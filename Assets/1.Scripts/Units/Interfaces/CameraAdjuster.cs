using UnityEngine;
using System.Collections;

public class CameraAdjuster : MonoBehaviour {
	public Character p1; //
	public Character p2;//
	public Character p3;//
	public Character p4;//
	//Average X, Z (Pulled from Player Locations)
	//Base X,Y,Z (To be adjusted by the multiplier Value)
    //Value of Adjustment
	public float avgX,avgZ,baseY,baseX,baseZ,adjVal,supBaseY,avgNum,avgPX,avgPZ;
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
		scrollCheck();
		//Same adjustment values for X and Z as start
		baseX = baseY/2 + adjVal;
		baseZ = -(baseY/2 + adjVal);
		//Average the x's and z's for the target location of the camera's focus
		avgMake();
		//Translate the camera after everything is done
		transform.position = new Vector3(avgX,baseY,avgZ);
	}
	void avgMake(){
		avgNum = 0;
		avgPX = 0;
		avgPZ = 0;
		if(!p1.isDead){
		  avgNum++;
		  avgPX += p1.transform.position.x;
		  avgPZ += p1.transform.position.z;
		}
		if(!p2.isDead){
		  avgNum++;
		  avgPX += p2.transform.position.x;
		  avgPZ += p2.transform.position.z;
		}
		if(!p3.isDead){
		  avgNum++;
		  avgPX += p3.transform.position.x;
		  avgPZ += p3.transform.position.z;
		}
		if(!p4.isDead){
		  avgNum++;
		  avgPX += p4.transform.position.x;
		  avgPZ += p4.transform.position.z;
		}
		//avgX = p1.transform.position.x;
		if(avgNum > 0){
			avgX = avgPX;
			avgZ = avgPZ;
			avgX = avgX/avgNum + baseX;
			avgZ = avgZ/avgNum + baseZ;
		}
	}
	void scrollCheck(){
		
		if((((Mathf.Abs(p1.transform.position.x - p2.transform.position.x) > baseY) && (!p1.isDead && !p2.isDead)) 
		   || ((Mathf.Abs(p3.transform.position.x - p2.transform.position.x) > baseY) && (!p3.isDead && p2.isDead)) 
		   || ((Mathf.Abs(p1.transform.position.x - p3.transform.position.x) > baseY) && (!p1.isDead && !p3.isDead)) 
		   || ((Mathf.Abs(p3.transform.position.x - p1.transform.position.x) > baseY) && (!p1.isDead && !p3.isDead)) 
		   || ((Mathf.Abs(p1.transform.position.x - p4.transform.position.x) > baseY) && (!p1.isDead && !p4.isDead)) 
		   || ((Mathf.Abs(p4.transform.position.x - p1.transform.position.x) > baseY) && (!p1.isDead && !p4.isDead)) 
		   || ((Mathf.Abs(p2.transform.position.x - p3.transform.position.x) > baseY) && (!p2.isDead && !p3.isDead)) 
		   || ((Mathf.Abs(p3.transform.position.x - p2.transform.position.x) > baseY) && (!p3.isDead && !p2.isDead)) 
		   || ((Mathf.Abs(p4.transform.position.x - p2.transform.position.x) > baseY) && (!p4.isDead && p2.isDead)) 
		   || ((Mathf.Abs(p4.transform.position.x - p2.transform.position.x) > baseY) && (!p4.isDead && p2.isDead)) 
		   || ((Mathf.Abs(p1.transform.position.x - p2.transform.position.x) > baseY) && (!p1.isDead && p2.isDead)) 
		   || ((Mathf.Abs(p3.transform.position.x - p4.transform.position.x) > baseY) && (!p4.isDead && p3.isDead)))
		   ||(((Mathf.Abs(p1.transform.position.z - p2.transform.position.z) > baseY) && (!p1.isDead && !p2.isDead)) 
		   || ((Mathf.Abs(p3.transform.position.z - p2.transform.position.z) > baseY) && (!p3.isDead && p2.isDead)) 
		   || ((Mathf.Abs(p1.transform.position.z - p3.transform.position.z) > baseY) && (!p1.isDead && !p3.isDead)) 
		   || ((Mathf.Abs(p3.transform.position.z - p1.transform.position.z) > baseY) && (!p1.isDead && !p3.isDead)) 
		   || ((Mathf.Abs(p1.transform.position.z - p4.transform.position.z) > baseY) && (!p1.isDead && !p4.isDead)) 
		   || ((Mathf.Abs(p4.transform.position.z - p1.transform.position.z) > baseY) && (!p1.isDead && !p4.isDead)) 
		   || ((Mathf.Abs(p2.transform.position.z - p3.transform.position.z) > baseY) && (!p2.isDead && !p3.isDead)) 
		   || ((Mathf.Abs(p3.transform.position.z - p2.transform.position.z) > baseY) && (!p3.isDead && !p2.isDead)) 
		   || ((Mathf.Abs(p4.transform.position.z - p2.transform.position.z) > baseY) && (!p4.isDead && p2.isDead)) 
		   || ((Mathf.Abs(p4.transform.position.z - p2.transform.position.z) > baseY) && (!p4.isDead && p2.isDead)) 
		   || ((Mathf.Abs(p1.transform.position.z - p2.transform.position.z) > baseY) && (!p1.isDead && p2.isDead)) 
		   || ((Mathf.Abs(p3.transform.position.z - p4.transform.position.z) > baseY) && (!p4.isDead && p3.isDead)))
		   ){
			baseY += .1f;
		}
		
		//Check if they are too close to need to shrink the distancing
		if((((Mathf.Abs(p1.transform.position.x + p2.transform.position.x) < baseY/2) && (!p1.isDead || !p2.isDead))
		   && ((Mathf.Abs(p2.transform.position.x + p3.transform.position.x) < baseY/2) && (!p3.isDead || !p2.isDead))
		   && ((Mathf.Abs(p1.transform.position.x + p3.transform.position.x) < baseY/2) && (!p1.isDead || !p3.isDead))
		   && ((Mathf.Abs(p3.transform.position.x + p1.transform.position.x) < baseY/2) && (!p1.isDead || !p3.isDead))
		   && ((Mathf.Abs(p1.transform.position.x + p4.transform.position.x) < baseY/2) && (!p1.isDead || !p4.isDead))
		   && ((Mathf.Abs(p4.transform.position.x + p1.transform.position.x) < baseY/2) && (!p1.isDead || !p4.isDead))
		   && ((Mathf.Abs(p2.transform.position.x + p3.transform.position.x) < baseY/2) && (!p3.isDead || !p2.isDead))
		   && ((Mathf.Abs(p3.transform.position.x + p2.transform.position.x) < baseY/2) && (!p3.isDead || !p2.isDead))
		   && ((Mathf.Abs(p2.transform.position.x + p4.transform.position.x) < baseY/2) && (!p4.isDead || !p2.isDead))
		   && ((Mathf.Abs(p4.transform.position.x + p2.transform.position.x) < baseY/2) && (!p4.isDead || !p2.isDead))
		   && ((Mathf.Abs(p1.transform.position.x + p2.transform.position.x) < baseY/2) && (!p1.isDead || !p2.isDead))
		   && ((Mathf.Abs(p3.transform.position.x + p4.transform.position.x) < baseY/2) && (!p1.isDead || !p2.isDead)))
		   || (((Mathf.Abs(p1.transform.position.z + p2.transform.position.z) < baseY/2) && (!p1.isDead || !p2.isDead))
		   && ((Mathf.Abs(p2.transform.position.z + p3.transform.position.z) < baseY/2) && (!p3.isDead || !p2.isDead))
		   && ((Mathf.Abs(p1.transform.position.z + p3.transform.position.z) < baseY/2) && (!p1.isDead || !p3.isDead))
		   && ((Mathf.Abs(p3.transform.position.z + p1.transform.position.z) < baseY/2) && (!p1.isDead || !p3.isDead))
		   && ((Mathf.Abs(p1.transform.position.z + p4.transform.position.z) < baseY/2) && (!p1.isDead || !p4.isDead))
		   && ((Mathf.Abs(p4.transform.position.z + p1.transform.position.z) < baseY/2) && (!p1.isDead || !p4.isDead))
		   && ((Mathf.Abs(p2.transform.position.z + p3.transform.position.z) < baseY/2) && (!p3.isDead || !p2.isDead))
		   && ((Mathf.Abs(p3.transform.position.z + p2.transform.position.z) < baseY/2) && (!p3.isDead || !p2.isDead))
		   && ((Mathf.Abs(p2.transform.position.z + p4.transform.position.z) < baseY/2) && (!p4.isDead || !p2.isDead))
		   && ((Mathf.Abs(p4.transform.position.z + p2.transform.position.z) < baseY/2) && (!p4.isDead || !p2.isDead))
		   && ((Mathf.Abs(p1.transform.position.z + p2.transform.position.z) < baseY/2) && (!p1.isDead || !p2.isDead))
		   && ((Mathf.Abs(p3.transform.position.z + p4.transform.position.z) < baseY/2) && (!p1.isDead || !p2.isDead)))
		   ){
			baseY -= .1f;
			//So it won't go too low
			if(baseY < supBaseY){
				baseY = supBaseY;
			}
		}
	}
}
