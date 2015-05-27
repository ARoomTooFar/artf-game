using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Loadgear : MonoBehaviour {
	public List<GameObject> gear;
	public Player[] playerPrefabs = new Player[4];
	
	// Use this for initialization
	protected void Start () {
		// this.LoadPlayers();
	}
	
	protected void Update() {
	}
	
	public void LoadPlayers() {
		GSManager manager = GameObject.Find("GSManager").GetComponent<GSManager>();
		PlayerUI ui = GameObject.Find ("PlayerUI").GetComponent<PlayerUI>();
		
		
		LoadGSManagerForTesting(manager);
		
		for (int i = 0; i < manager.playerDataList.Length; i++) {
			if(manager.playerDataList[i] == null) continue;
			
			PlayerData tempData = manager.playerDataList[i];
			Player tempPlayer = Instantiate (playerPrefabs[i], MapDataParser.start.Coordinates[i], Quaternion.identity) as Player;
			
			tempPlayer.gear.EquipWeapon(this.gear[tempData.weapon], tempPlayer.opposition, tempData.inventory[tempData.weapon]);
			tempPlayer.gear.EquipArmor(this.gear[tempData.armor], tempData.inventory[tempData.armor]);
			tempPlayer.gear.EquipHelmet(this.gear[tempData.headgear], tempData.inventory[tempData.headgear]);
			
			tempPlayer.inventory.EquipItems(this.gear[tempData.actionslot1], tempPlayer.opposition);
			tempPlayer.inventory.EquipItems(this.gear[tempData.actionslot2], tempPlayer.opposition);
			tempPlayer.inventory.EquipItems(this.gear[tempData.actionslot3], tempPlayer.opposition);
			
			manager.players[i] = tempPlayer;
			ui.setUpPlayerUIPane("Player" + (i + 1));
		}
		
		foreach (GameObject cam in GameObject.FindGameObjectsWithTag("Door")) {
			cam.GetComponent<DoorProximity>().FindPlayers();
		}
		
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraAdjuster>().InstantiatePlayers();
	}
	
	private void LoadGSManagerForTesting(GSManager manager) {
		for (int i = 0; i < 4; i++) {
			PlayerData tempData = new PlayerData();
			
			tempData.inventory = new int[52];
			
			for (int j = 0; j < tempData.inventory.Length; j++) {
				tempData.inventory[j] = 1;
			}
			
			tempData.weapon = 12 + i * 4;
			tempData.headgear = 41;
			tempData.armor = 32;
			
			if (i == 0) {
				tempData.actionslot1 = 0;
				tempData.actionslot2 = 1;
				tempData.actionslot3 = 2;
			} else if (i == 1) {
				tempData.actionslot1 = 4;
				tempData.actionslot2 = 5;
				tempData.actionslot3 = 6;
			} else if (i == 2) {
				tempData.actionslot1 = 7;
				tempData.actionslot2 = 4;
				tempData.actionslot3 = 1;
			} else {
				tempData.actionslot1 = 6;
				tempData.actionslot2 = 2;
				tempData.actionslot3 = 7;
			}
			
			manager.playerDataList[i] = tempData;
		}
	}
}
