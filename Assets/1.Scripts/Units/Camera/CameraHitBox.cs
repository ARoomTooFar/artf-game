using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraHitBox : MonoBehaviour {
	public List<Character> areaUnits;
	public Player[] allPlayers;
	public float avgX,avgZ,totalX,totalZ;
	public bool same;
	public int enemyCount;

	//private AudioSource battle;
	//private AudioSource environment;

	// Use this for initialization
	void Start () {
		GetComponent<Transform>();
		allPlayers = FindObjectsOfType(typeof(Player)) as Player[];
		for(int x = 0; x < allPlayers.Length; x++){
			areaUnits.Add(allPlayers[x]);
		}
		//environment = GameObject.Find ("PerspectiveAngledCamera").GetComponent<AudioSource> ();
		//battle = GetComponent<AudioSource> ();

	}
	
	// Update is called once per frame
	void Update () {		
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
