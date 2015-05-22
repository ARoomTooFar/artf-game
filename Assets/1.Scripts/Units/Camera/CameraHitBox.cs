using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraHitBox : MonoBehaviour {
	public List<Character> areaUnits;
	public Player[] allPlayers;
	public float avgX,avgZ,totalX,totalZ;
	public bool same;
	public int enemyCount;

	private AudioSource battle;
	private AudioSource environment;

	// Use this for initialization
	void Start () {
		GetComponent<Transform>();
		allPlayers = FindObjectsOfType(typeof(Player)) as Player[];
		for(int x = 0; x < allPlayers.Length; x++){
			areaUnits.Add(allPlayers[x]);
		}
		environment = GameObject.Find ("PerspectiveAngledCamera").GetComponent<AudioSource> ();
		battle = GetComponent<AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < areaUnits.Count; i++) {
			if(areaUnits[i].isDead){
				areaUnits.Remove(areaUnits[i]);
				i--;
			}
		}

		if (enemyCount > 0 && !battle.isPlaying) {
			environment.Pause ();
			environment.volume = 0;
			battle.Play ();
		} else if (enemyCount == 0 && battle.isPlaying) {
			if (TransitionOut (battle, 0.7f, 0))
				environment.UnPause ();
		} else if (environment.volume < 1 && environment.isPlaying) {
			TransitionIn(environment, 0.3f, 1);
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
		case "Enemy":
			enemyCount++;
			battle.volume = 1;
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
			break;
		}		
	}


	bool TransitionIn(AudioSource musik, float rate, float done) {
		if(musik.volume < 0.2f) musik.volume += 0.1f * Time.deltaTime;
		else if (musik.volume < done) musik.volume += rate * Time.deltaTime;
		return musik.volume >= done;
	}

	bool TransitionOut(AudioSource musik, float rate, float done) {
		if (musik.volume > 0.2f) musik.volume -= rate * Time.deltaTime;
		else if (musik.volume > done) musik.volume -= 0.1f * Time.deltaTime;

		if(musik.volume <= done) {
			musik.Stop();
			musik.volume = 1;
			return true;
		}
		return false;
	}


}
