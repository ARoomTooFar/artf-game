using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterLoot : MonoBehaviour {
	public LootList lootList;
	public List<string> inspectorLootDisplay; //for inspector display of loot list
	
	public void initializeLoot(string monsterType, int tier){
		inspectorLootDisplay = new List<string>();

		if(LootTable.lootTable.ContainsKey(monsterType + tier.ToString())){
			lootList = LootTable.lootTable[monsterType + tier.ToString()];

			foreach(var key in lootList.lootList.Keys){
				if(lootList.lootList[key] > 0f)
					inspectorLootDisplay.Add(key + ": " + lootList.lootList[key] + "%");
			}
		}else{
			print ("No loot table information for " + this.gameObject.name + " of tier " + tier);
		}

	} 
	
}
