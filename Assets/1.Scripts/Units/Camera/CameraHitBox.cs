using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraHitBox : MonoBehaviour {
	public List<Character> areaUnits;
	public Player[] allPlayers;
	public float avgX,avgZ,totalX,totalZ;
	public bool same;
	public int enemyCount;
	public AudioClip Song;
	private AudioClip resumeSong;
	private AudioSource Playing;

	// Use this for initialization
	void Start () {
		GetComponent<Transform>();
		allPlayers = FindObjectsOfType(typeof(Player)) as Player[];
		for(int x = 0; x < allPlayers.Length; x++){
			areaUnits.Add(allPlayers[x]);
		}
		Playing = transform.parent.gameObject.GetComponentInChildren<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < areaUnits.Count; i++) {
			if(areaUnits[i].isDead){
				areaUnits.Remove(areaUnits[i]);
				i--;
			}
		}

		if (enemyCount > 0 && Playing != null && !(Playing.clip == Song)) {
			resumeSong = Playing.clip;
			Playing.clip = Song;
			Playing.Play ();
		}

		
		avgMake();
	}
	void avgMake(){
		totalX = 0;
		totalZ = 0;
		foreach(Character unit in areaUnits) {
			if (unit == null) continue;
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
		/*Character unit = other.GetComponent<Character>();
		if(unit != null) {
			areaUnits.Add (unit);
		}
		*/

		switch (other.tag) {
		case "Wall":
			if(other.transform.parent != null){
				Wall wallSpot = other.transform.parent.GetComponent<Wall>();
				if(wallSpot != null){
					wallSpot.toggleShow();
					//Debug.Log("Here");
				}
			}else{
				Wall wallSpot = other.gameObject.GetComponent<Wall>();
				if(wallSpot != null){
					wallSpot.toggleShow();
					//Debug.Log("Here");
				}
			}
			break;
		case "Enemy":
			enemyCount++;
			break;
		}

}
	void OnTriggerStay (Collider other) {
		/*
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
		*/
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
	void OnTriggerExit (Collider other) {
		/*Character unit = other.GetComponent<Character>();
		if(unit != null) {
			areaUnits.Add (unit);
		}
		*/
		
		switch (other.tag) {
		case "Enemy":
			enemyCount--;
			if(enemyCount < 1){
				Playing.clip = resumeSong;
				Playing.Play ();
			}
			break;
		}
		
	}
}
