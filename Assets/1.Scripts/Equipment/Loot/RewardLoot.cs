using UnityEngine;
using System.Collections;

public class RewardLoot : MonoBehaviour {
	public LootSystem lootage;
	public int number;
	public Material common;
	public Material uncommon;
	public Material rare;
	public Material epic;
	public Material legendary;
	public Material artifact;
	// Use this for initialization
	void Start () {
		common = new Material(Shader.Find("Specular"));
		common.color = Color.grey;
		uncommon = new Material(Shader.Find("Specular"));
		uncommon.color = Color.green;
		rare = new Material(Shader.Find("Specular"));
		rare.color = Color.blue;
		epic = new Material(Shader.Find("Specular"));
		epic.color = Color.magenta;
		legendary = new Material(Shader.Find("Specular"));
		legendary.color = Color.yellow;
		artifact = new Material(Shader.Find("Specular"));
		artifact.color = Color.red;
		lootage = (LootSystem) FindObjectOfType(typeof(LootSystem));
		if(lootage){
			if(lootage.rarity[number] == -1){
				GetComponent<Renderer>().enabled = false;
			}else if(lootage.rarity[number] == 0){
				GetComponent<Renderer>().material = common;
			}else if(lootage.rarity[number] == 1){
				GetComponent<Renderer>().material = uncommon;
			}else if(lootage.rarity[number] == 2){
				GetComponent<Renderer>().material = rare;
			}else if(lootage.rarity[number] == 3){
				GetComponent<Renderer>().material = epic;
			}else if(lootage.rarity[number] == 4){
				GetComponent<Renderer>().material = legendary;
			}else if(lootage.rarity[number] == 5){
				GetComponent<Renderer>().material = artifact;
			}else{
				GetComponent<Renderer>().enabled = false;
			}
		}else{
			GetComponent<Renderer>().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
