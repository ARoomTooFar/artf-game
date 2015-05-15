﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//opens door when players get near
public class DoorProximity : MonoBehaviour {
	public List<GameObject> playerList;
	public Vector3 doorStartPos;
	
	void Start () {
		doorStartPos = this.gameObject.transform.parent.position;

		playerList = new List<GameObject>();
		playerList.Add(GameObject.Find("Player1"));
		playerList.Add(GameObject.Find("Player2"));
		playerList.Add(GameObject.Find("Player3"));
		playerList.Add(GameObject.Find("Player4"));
	}

	void Update () {
		Vector3 doorPos =  this.gameObject.transform.parent.position;

		bool playerNear = false;
		for(int i = 0; i < playerList.Count; i++){
			if(Vector3.Distance(playerList[i].transform.position, doorStartPos) < 3f){
				playerNear = true;
			}
		}

		if(playerNear){
			doorPos.y = Mathf.MoveTowards(doorPos.y, doorStartPos.y - 5f, Time.deltaTime * 4f);
			this.gameObject.transform.parent.position = doorPos;
		}else{
			doorPos.y = Mathf.MoveTowards(doorPos.y, doorStartPos.y, Time.deltaTime * 4f);
			this.gameObject.transform.parent.position = doorPos;
		}
	}
}