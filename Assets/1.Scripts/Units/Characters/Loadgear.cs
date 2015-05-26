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
		}
	}
}
