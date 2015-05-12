using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterLoot : MonoBehaviour {
	public Dictionary<string, float> lootItems;
	public List<string> inspectorLootDisplay; //only for testing

	public void initializeLoot(string monsterType, int tier){
		if(LootTable.lootTable.ContainsKey(monsterType + tier.ToString())){
			lootItems = new Dictionary<string, float>(LootTable.lootTable[monsterType + tier.ToString()]);
		}else{
//			print ("No loot table information for " + this.gameObject.name + " of tier " + tier);
		}
	} 
	
}
