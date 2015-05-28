using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//opens door when players get near
public class DoorProximity : MonoBehaviour {
	public List<GameObject> playerList;
	public Vector3 doorStartPos;
	public int numPlayers = 0;

	public SceneryBlock door;
	public SceneryBlock otherDoor;

	public DoorProximity otherProx;
	
	void Start () {
		doorStartPos = this.gameObject.transform.parent.position;
		door = MapData.SceneryBlocks.find(new Vector3(doorStartPos.x, 0, doorStartPos.z));
		otherDoor = MapData.SceneryBlocks.find(door.doorCheckPosition);
		if(otherDoor != null) {
			otherProx = otherDoor.GameObj.GetComponentInChildren<DoorProximity>();
		}
	}

	public void FindPlayers() {
		playerList = new List<GameObject>();
		playerList.Add(GameObject.FindGameObjectWithTag("Player1"));
		playerList.Add(GameObject.FindGameObjectWithTag("Player2"));
		playerList.Add(GameObject.FindGameObjectWithTag("Player3"));
		playerList.Add(GameObject.FindGameObjectWithTag("Player4"));
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag.Substring(0, other.gameObject.tag.Length - 1) == "Player") {
			numPlayers++;
		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.tag.Substring(0, other.gameObject.tag.Length - 1) == "Player") {
			numPlayers--;
		}
	}

	void Update () {
		//If there is no door on the other side, return and do nothing;
		if(otherDoor == null) {
			Debug.Log("No Partner" + this.gameObject.transform.parent.position);	
			return;
		}

		Vector3 doorPos =  door.GameObj.transform.position;
		if(numPlayers + otherProx.numPlayers > 0){
			doorPos.y = Mathf.MoveTowards(doorPos.y, doorStartPos.y - 6f, Time.deltaTime * 4f);
			door.GameObj.transform.position = doorPos;
		}else{
			doorPos.y = Mathf.MoveTowards(doorPos.y, doorStartPos.y, Time.deltaTime * 4f);
			door.GameObj.transform.position = doorPos;
		}
	}
}
