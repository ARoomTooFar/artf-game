using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Loadgear : MonoBehaviour {
	public List<GameObject> gear;
	
	public List<Character> players = new List<Character>();
	
	// Use this for initialization
	protected void Start () {
		// this.LoadPlayers();
	}
	
	protected void Update() {
	}
	
	public void LoadPlayers() {
		GSManager manager = GameObject.Find("GSManager").GetComponent<GSManager>();
		
		manager.
		
		for (int i = 0; i < players.Count; i++) {
			if(players[i] == null) continue;
			
			loadFromText("P" + (i + 1).ToString());
			players[i].equipTest(equipment.ToArray(), abilities.ToArray());
			equipment.Clear();
			abilities.Clear();
		}
	}
	
	/*
	public string[] loadData;
	public string[] loadLine;
	
	public List<GameObject> equipment = new List<GameObject>();
	public List<GameObject> abilities = new List<GameObject>();
	//public List<GameObject> 
	public string cName;

	
	


	private void loadFromText(string text) {
		TextAsset gearData = (TextAsset)Resources.Load("Testdata/" + text, typeof(TextAsset));
		loadData = gearData.text.Split('\n');
		if (loadData.Length == 0) Debug.LogWarning("Load data file " + text + " is empty.");
		else if (loadData.Length != 7) Debug.LogWarning("Load data file " + text + " does not contain exactly 7 lines.");
		else {
			cName = loadData[0];	// Set the players ign

			for(int i = 1; i<loadData.Length; i++){ //Set our data
				loadFromSplit(i);
				equipPiece(loadLine[0]);
			}
		}
	}
	private void loadFromSplit(int number){//Requires number of line working on, 1-7
		loadLine = loadData[number].Split('#');
	}
	private void equipPiece(string code){
		int index = (int)(code[1]-'0');
		if (code[0] == ('C') ) { // Chest Items
			equipment.Add (data.armory[index].gameObject);
		} else if (code[0] == ('H')) { // Helmet Items
			equipment.Add (data.hats[index].gameObject);
		} else if (code[0] == ('W')) { // Weapon Items
			//equipment.Add (data.weaponry[index].gameObject);
		} else if (code[0] == ('I')) { // Ability Items
			abilities.Add (data.inventory[index].gameObject);
		} else {
			Debug.LogWarning("You fucked up, betch! Your data files are bad and you should feel bad.");
		}
	}
	*/
}
