using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterLoot : MonoBehaviour {
	public LootList lootList;
	public List<string> inspectorLootDisplay; //for inspector display of loot list
	public NewEnemy monster;

	void Start(){
		monster = this.gameObject.GetComponent<NewEnemy>();
	}

	public void lootMonster(){
		foreach(var key in lootList.thisMonster.Keys){
			float chanceToDrop = lootList.thisMonster[key];

			if(chanceToDrop > 0f){
				float rollTheDice = Random.Range(0f, 100f);

				if(rollTheDice < chanceToDrop)
					LootedItems.addItemToLoot(key);
			}
		}
	}
	
	public void initializeLoot(string monsterType, int tier){
		inspectorLootDisplay = new List<string>();

		if(LootTable.lootTable.ContainsKey(monsterType + tier.ToString())){
			lootList = LootTable.lootTable[monsterType + tier.ToString()];

			foreach(var key in lootList.thisMonster.Keys){
				if(lootList.thisMonster[key] > 0f)
					inspectorLootDisplay.Add(key + ": " + lootList.thisMonster[key] + "%");
			}
		}else{
			print ("No loot table information for " + this.gameObject.name + " of tier " + tier);
		}
	}
}


