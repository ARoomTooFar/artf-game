using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraHitBox : MonoBehaviour {
	public List<Character> areaUnits;
	public Player[] allPlayers;
	public float avgX,avgZ,totalX,totalZ;
	public bool same;
	// Use this for initialization
	void Start () {
		GetComponent<Transform>();
		allPlayers = FindObjectsOfType(typeof(Player)) as Player[];
		for(int x = 0; x < allPlayers.Length; x++){
			areaUnits.Add(allPlayers[x]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		avgMake();
	}
	void avgMake(){
		totalX = 0;
		totalZ = 0;
		foreach(Character unit in areaUnits) {
			totalX+=unit.transform.position.x;
			totalZ+=unit.transform.position.z;
		}
		if(areaUnits.Count > 0){
			avgX = totalX/areaUnits.Count;
			avgZ = totalZ/areaUnits.Count;
			transform.position = new Vector3(avgX,transform.position.y,avgZ);
		}
	}
	void OnTriggerEnter (Collider other) {
		Character unit = other.GetComponent<Character>();
		if(unit != null) {
			areaUnits.Add (unit);
		}
		if(other.tag == "Wall"){
			Wall wallSpot = other.transform.parent.GetComponent<Wall>();
			//Debug.Log("Here");
		//Wall wallSpot = other.GetComponent<Wall>();
			if(wallSpot != null){
				wallSpot.toggleShow();
				//Debug.Log("Here");
			}
		}
	}
	void OnTriggerStay (Collider other) {
		Character unit = other.GetComponent<Character>();
		if(unit != null) {
			foreach(Character thingie in areaUnits) {
				if(thingie == unit){
					same = true;
				}					
			}
			if(!same){
				areaUnits.Add (unit);
			}
			same = false;
		}
		if(other.tag == "Wall"){
			Wall wallSpot = other.transform.parent.GetComponent<Wall>();
			//Debug.Log("Here");
		//Wall wallSpot = other.GetComponent<Wall>();
			if(wallSpot != null){
				wallSpot.toggleShow();
				//Debug.Log("Here");
			}
		}
	}
}
