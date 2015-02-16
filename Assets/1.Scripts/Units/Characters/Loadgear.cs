﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Loadgear : MonoBehaviour {

	public DataChest data;
	public string[] loadData;
	public List<Character> players = new List<Character>();
	public List<GameObject> equipment = new List<GameObject>();
	public List<GameObject> abilities = new List<GameObject>();
	public string name;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < players.Count; i++) {
			loadFromText("P" + (i + 1).ToString());
			players[i].equipTest(equipment.ToArray(), abilities.ToArray());
			equipment.Clear();
			abilities.Clear();
		}
	}

	void Update() {

		/*
		if(Input.GetKeyDown(KeyCode.Space)) {

			for (int i = 0; i < players.Count; i++) {
				loadFromText("P" + (i + 1).ToString());
				players[i].equipTest(equipment.ToArray(), abilities.ToArray());
				equipment.Clear();
				abilities.Clear();
			}
		}*/
	}

	private void loadFromText(string text) {
		TextAsset gearData = (TextAsset)Resources.Load("Testdata/" + text, typeof(TextAsset));
		loadData = gearData.text.Split('\n');

		if (loadData.Length == 0) Debug.LogWarning("Load data file " + text + " is empty.");
		else if (loadData.Length != 7) Debug.LogWarning("Load data file " + text + " does not contain exactly 7 lines.");
		else {
			name = loadData[0];	// Set the players ign

			for(int i = 1; i<loadData.Length; i++){ //Set our data
				equipPiece(loadData[i]);
			}
		}
	}

	private void equipPiece(string code){
		int index = (int)(code[1]-'0');
		if (code[0] == ('C') ) { // Chest Items
			equipment.Add (data.armory[index].gameObject);
		} else if (code[0] == ('H')) { // Helmet Items
			equipment.Add (data.hats[index].gameObject);
		} else if (code[0] == ('W')) { // Weapon Items
			equipment.Add (data.weaponry[index].gameObject);
		} else if (code[0] == ('I')) { // Ability Items
			abilities.Add (data.inventory[index].gameObject);
		} else {
			Debug.LogWarning("You fucked up, betch! Your data files are bad and you should feel bad.");
		}
	}

	/*
	// Update is called once per frame
	void Update () {

			loadFromText();
			equipPiece("W0");
			equipPiece("C0");
			equipPiece("H1");
			equipPiece("I0");
			equipPiece("I1");
			equipPiece("I2");
			System.IO.File.WriteAllText(path,System.String.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n",name,"W0","C0","H1","I0","I1","I2"));
		}
		if(Input.GetKeyDown(KeyCode.M)){
			saveToText();
		}
	}

	
	public virtual void saveToText(){
		loadData[0]=name;
		System.IO.File.WriteAllLines(path,loadData);
	}
	public virtual void loadFromText(){
		loadData = System.IO.File.ReadAllLines(path);
		for(int i = 0; i<loadData.Length; i++){
			if(i==0){//First line is name
				name = loadData[i];
			}else{//Rest of lines are things to equip
				equipPiece(loadData[i]);
			}
		}
	}
	//Item type first char, number in array, second char++
	public virtual void equipPiece(System.String code){
		int index = (int)(code[1]-'0');
		if(code[0] == ('C')){
			Quaternion butt = Quaternion.Euler(new Vector3(bodyLocation.eulerAngles.x+data.GetComponent<DataChest>().armory[index].transform.eulerAngles.x,bodyLocation.eulerAngles.y+data.GetComponent<DataChest>().armory[index].transform.eulerAngles.y,bodyLocation.eulerAngles.z+data.GetComponent<DataChest>().armory[index].transform.eulerAngles.z));
			gear.bodyArmor = (Armor) Instantiate(data.GetComponent<DataChest>().armory[index],headLocation.position,butt); //data.GetComponent<DataChest>().weaponry[1].transform.position+
			gear.bodyArmor.transform.parent = bodyLocation.transform;
			gear.bodyArmor.equip(gameObject.GetComponent<Character>());
		}else if(code[0] == ('H')){
			//Quaternion butt = Quaternion.Euler(new Vector3(headLocation.eulerAngles.x+data.GetComponent<DataChest>().armory[index].transform.eulerAngles.x,headLocation.eulerAngles.y+data.GetComponent<DataChest>().armory[index].transform.eulerAngles.y,headLocation.eulerAngles.z+data.GetComponent<DataChest>().armory[index].transform.eulerAngles.z));
			gear.helmet = (Armor) Instantiate(data.GetComponent<DataChest>().armory[index],headLocation.position,transform.rotation); //data.GetComponent<DataChest>().weaponry[1].transform.position+
			gear.helmet.transform.parent = headLocation.transform;
			gear.helmet.equip(gameObject.GetComponent<Character>());
		}else if(code[0] == ('W')){
			
			Quaternion butt = Quaternion.Euler(new Vector3(weapLocation.eulerAngles.x+data.GetComponent<DataChest>().weaponry[index].transform.eulerAngles.x,weapLocation.eulerAngles.y+data.GetComponent<DataChest>().weaponry[index].transform.eulerAngles.y,weapLocation.eulerAngles.z+data.GetComponent<DataChest>().weaponry[index].transform.eulerAngles.z));
			gear.weapon = (Weapons) Instantiate(data.GetComponent<DataChest>().weaponry[index],weapLocation.position,butt); //data.GetComponent<DataChest>().weaponry[1].transform.position+
			gear.weapon.transform.parent = weapLocation.transform;
			gear.weapon.equip(gameObject.GetComponent<Character>());
		}else if(code[0] == ('I')){
			Quaternion butt = Quaternion.Euler(new Vector3(itemLocation.eulerAngles.x+data.GetComponent<DataChest>().inventory[index].transform.eulerAngles.x,itemLocation.eulerAngles.y+data.GetComponent<DataChest>().inventory[index].transform.eulerAngles.y,itemLocation.eulerAngles.z+data.GetComponent<DataChest>().inventory[index].transform.eulerAngles.z));
			inventory.items.Add((Item) Instantiate(data.GetComponent<DataChest>().inventory[index],itemLocation.position,butt)); //data.GetComponent<DataChest>().weaponry[1].transform.position+
			inventory.items[inventory.items.Count-1].transform.parent = itemLocation.transform;
			inventory.equipItems(gameObject.GetComponent<Character>());
		}else{
			Debug.Log("You fucked up, bitch");
		}
	}*/
}