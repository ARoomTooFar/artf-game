using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Inventory : MonoBehaviour {
	//For the fix (Ignore later)
	public GameObject[] loadOuts;
	public string[] loadData;//From Loadgear
	public string[] loadLine;//Expansion
	public string[] itemsUsed;//Preventing duplicate clause
	public Controls controls;
	public bool on,actable,ready,dupItem;
	public int playNumber, spotNumber, lineNumber, itemDuplicate; //1-4, 1-length of spot line, 1-6;
	public IconChest iChest;
	public int[] spotNums;
	public GameObject[] iconSpots; //0 Weapon, 1 Chest, 2 Head, 3-5 Items
	// Use this for initialization
	void Start () {
		itemsUsed = new string[6];
		actable = true;
		//on = true;
		iChest = (IconChest) FindObjectOfType(typeof(IconChest));
		lineNumber = 1; //Don't want player name
		spotNumber = 0; 
		loadFromText("P" + (playNumber).ToString());
		//if(on){
		loadFromSplit(lineNumber);
		//}
		loadOutBuild(lineNumber);
		spotCheckIn();
		//recompileLine(loadLine, lineNumber);
		//recompileText("P" + (playNumber).ToString());
	}
	
	// Update is called once per frame
	void Update () {
		if(actable&&on){
			if (Input.GetKey(controls.up) &&!ready &&!dupItem) {
				recompileLine(lineNumber);
				spotNumber= 0;
				lineNumber--;
				if(lineNumber<1){//Boundary
					lineNumber = 1;
				}
				loadFromSplit(lineNumber);
				actable = false;
				loadOutBuild(lineNumber);
				spotCheckIn();
				//spotCheckIn(lineNumber);
				StartCoroutine(Wait(.15f));
			}
			//"Down" key assign pressed
			if (Input.GetKey(controls.down)&&!ready &&!dupItem) {
				recompileLine(lineNumber);
				spotNumber= 0;
				lineNumber++;
				if(lineNumber>6){//Boundary
					lineNumber = 6;
				}
				loadFromSplit(lineNumber);
				actable = false;
				loadOutBuild(lineNumber);
				spotCheckIn();
				//spotCheckIn(lineNumber);
				StartCoroutine(Wait(.15f));
			}
			//"Left" key assign pressed
			if (Input.GetKey(controls.left)&&!ready) {
				//spotCheckIn();
				spotNumber--;
				if(spotNumber<0){
					spotNumber = 0;
				}
				swapPartsInLine();
				actable = false;
				//loadOutBuild(lineNumber);
				spotCheckIn();
				StartCoroutine(Wait(.15f));
			}
			//"Right" key assign pressed
			if (Input.GetKey(controls.right)&&!ready) {
				spotNumber++;
				if(spotNumber>loadLine.Length-1){
					spotNumber = loadLine.Length-1;
				}
				swapPartsInLine();
				actable = false;
				//loadOutBuild(lineNumber);
				spotCheckIn();
				StartCoroutine(Wait(.15f));
			}
			if(Input.GetKeyDown(controls.attack) || (controls.joyUsed &&  Input.GetButtonDown(controls.joyAttack))) {
				ready = true;
				actable = false;
				recompileLine(lineNumber);
				loadFromSplit(lineNumber);
				loadOutBuild(lineNumber);
				spotCheckIn();
				StartCoroutine(Wait(.15f));
			}
			if(Input.GetKeyDown (controls.secItem) || (controls.joyUsed && Input.GetButtonDown(controls.joySecItem))) {//Choose item to be in slot
				if(ready){
					ready = false;
				}
				recompileLine(lineNumber);
				loadFromSplit(lineNumber);
				loadOutBuild(lineNumber);
				spotCheckIn();
				actable = false;
				StartCoroutine(Wait(.15f));
			}
		}
		if(actable&&!on){
			if(Input.GetKeyDown(controls.attack) || (controls.joyUsed &&  Input.GetButtonDown(controls.joyAttack))) {
				on = true;
				spotCheckIn();
				StartCoroutine(Wait(.15f));
			}
		}
		
	}
	private void spotCheckIn(){
		if(on){
			for(int i = 0; i< itemsUsed.Length; i++){
				if(itemsUsed[i] == "W0" || itemsUsed[i] == "C0" || itemsUsed[i] == "H0" || itemsUsed[i] == "I0"){
					spotNums[i] = 0;
				}else if(itemsUsed[i] == "W1" || itemsUsed[i] == "C1" || itemsUsed[i] == "H1" || itemsUsed[i] == "I1"){
					spotNums[i] = 1;
				}else if(itemsUsed[i] == "W2" || itemsUsed[i] == "C2" || itemsUsed[i] == "H2" || itemsUsed[i] == "I2"){
					spotNums[i] = 2;
				}else if(itemsUsed[i] == "W3" || itemsUsed[i] == "C3" || itemsUsed[i] == "H3" || itemsUsed[i] == "I3"){
					spotNums[i] = 3;
				}else if(itemsUsed[i] == "W4" || itemsUsed[i] == "C4" || itemsUsed[i] == "H4" || itemsUsed[i] == "I4"){
					spotNums[i] = 4;
				}else if(itemsUsed[i] == "W5" || itemsUsed[i] == "C5" || itemsUsed[i] == "H5" || itemsUsed[i] == "I5"){
					spotNums[i] = 5;
				}else if(itemsUsed[i] == "W6" || itemsUsed[i] == "C6" || itemsUsed[i] == "H6" || itemsUsed[i] == "I6"){
					spotNums[i] = 6;
				}else if(itemsUsed[i] == "W7" || itemsUsed[i] == "C7" || itemsUsed[i] == "H7" || itemsUsed[i] == "I7"){
					spotNums[i] = 7;
				}
				if(i == 0){
					iconSpots[i].GetComponent<Renderer>().material = iChest.weaponry[spotNums[i]];
				}else if(i == 1){
					iconSpots[i].GetComponent<Renderer>().material = iChest.armory[spotNums[i]];
				}else if(i == 2){
					iconSpots[i].GetComponent<Renderer>().material = iChest.hats[spotNums[i]];
				}else if(i >= 3){
					iconSpots[i].GetComponent<Renderer>().material = iChest.inventory[spotNums[i]];
				}
			}
		}else{
			for(int i = 0; i< itemsUsed.Length; i++){
				iconSpots[i].GetComponent<Renderer>().material = iChest.offState;
			}
		}
	}
	//Basic wait delay
	private IEnumerator Wait(float duration){
		if(!actable){
		for (float timer = 0; timer < duration; timer += Time.deltaTime){
			yield return 0;
		}
		actable = true;
		}
	}
	private void loadOutBuild(int num){
		loadFromSplit(1);
		itemsUsed[0] = loadLine[0];
		loadFromSplit(2);
		itemsUsed[1] = loadLine[0];
		loadFromSplit(3);
		itemsUsed[2] = loadLine[0];
		dupLoad(num);
	}
	//Set the duplicates
	private void dupLoad(int num){//Has to have loaded from text already
		loadFromSplit(4);
		itemsUsed[3] = loadLine[0];
		loadFromSplit(5);
		itemsUsed[4] = loadLine[0];
		loadFromSplit(6);
		itemsUsed[5] = loadLine[0];
		if(itemsUsed[3] == itemsUsed[4] || itemsUsed[3] == itemsUsed[5] || itemsUsed[4] == itemsUsed[5]){
			dupItem = true;
		}else{
			dupItem = false;
		}
		loadFromSplit(num);
	}
	//Taken from Loadgear for base
	private void loadFromText(string text) {//Requires P#
		TextAsset gearData = (TextAsset)Resources.Load("Testdata/" + text, typeof(TextAsset));
		loadData = gearData.text.Split('\n');
		Debug.LogWarning(loadData.Length);
		if (loadData.Length == 0) Debug.LogWarning("Load data file " + text + " is empty.");
		else if (loadData.Length != 7) Debug.LogWarning("Load data file " + text + " does not contain exactly 7 lines.");
		
	}
	private void swapPartsInLine(){//Takes spot number, moves it to front, makes spot number 
		string temp = loadLine[0];
		loadLine[0] = loadLine[spotNumber];
		loadLine[spotNumber] = temp;
	}
	private void loadFromSplit(int number){//Requires number of line working on, 1-7
		loadLine = loadData[number].Split('#');
	}
	private void recompileLine(int number){
		string temp = "";
		for(int i = 0; i<loadLine.Length-1; i++){
			temp = temp + loadLine[i] + '#';
		}
		temp = temp + loadLine[loadLine.Length-1];
		//temp = temp + "W8";//Was used to test adding a spot to the line
		loadData[number] = temp;
	}
	private void recompileText(string text){//Requires P#
		//loadData[number] = text;
		string temp = "";
		for(int i = 0; i<loadData.Length-1; i++){
			temp = temp + loadData[i] + '\n';
		}
		temp = temp + loadData[loadData.Length-1];//Don't want to give the text an 8th line
		StreamWriter sw = new StreamWriter("Assets/Resources/Testdata/" + text +".txt");
		sw.Write(temp);
		sw.Close();
		
	}
}
