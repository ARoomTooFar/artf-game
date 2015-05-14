using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterLoot : MonoBehaviour {
	public LootList lootList;
	public List<string> inspectorLootDisplay; //for inspector display of loot list

	//monster dead. send loot to loot system
	public void lootMonster(){
		foreach(var key in lootList.thisMonster.Keys){
			float chanceToDrop = lootList.thisMonster[key];

			//make sure chance is above 0%
			if(chanceToDrop > 0f){

				//do a random roll to determine if item drops or not
				float rollTheDice = Random.Range(0f, 100f);
				if(rollTheDice < chanceToDrop)
					LootedItems.addItemToLoot(key);
			}
		}
	}
	
	public void initializeLoot(string monsterType, int tier){
		inspectorLootDisplay = new List<string>();

		//if loot table has data for this monster, make loot list for it
		if(LootTable.lootTable.ContainsKey(monsterType + tier.ToString())){
			lootList = LootTable.lootTable[monsterType + tier.ToString()];

			//just so we can see loot list in inspector
			foreach(var key in lootList.thisMonster.Keys){
				if(lootList.thisMonster[key] > 0f)
					inspectorLootDisplay.Add(key + ": " + lootList.thisMonster[key] + "%");
			}
		}else{
			print ("No loot table information for " + this.gameObject.name + " of tier " + tier);
		}
	}
}


