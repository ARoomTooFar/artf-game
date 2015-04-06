using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Inventory : MonoBehaviour {
	public GameObject[] loadOuts;
	public string[] loadData;//From Loadgear
	public string[] loadLine;//Expansion
	public string[] itemsUsed;//Preventing duplicate clause
	public Controls controls;
	public bool on,actable,ready,dupItem;
	public int playNumber, spotNumber, lineNumber, itemDuplicate; //1-4, 1-length of spot line, 1-6;
	// Use this for initialization
	void Start () {
		itemsUsed = new string[3];
		actable = true;
		on = true;
		lineNumber = 1; //Don't want player name
		spotNumber = 0; 
		loadFromText("P" + (playNumber).ToString());
		//if(on){
		loadFromSplit(lineNumber);
		//}
		dupLoad(lineNumber);
		
		//recompileLine(loadLine, lineNumber);
		//recompileText("P" + (playNumber).ToString());
	}
	
	// Update is called once per frame
	void Update () {
		if(actable&&on){
			if (Input.GetKey(controls.up) &&!ready &&!dupItem) {
				recompileLine(lineNumber);
				lineNumber--;
				if(lineNumber<1){//Boundary
					lineNumber = 1;
				}
				loadFromSplit(lineNumber);
				actable = false;
				StartCoroutine(Wait(.25f));
			}
			//"Down" key assign pressed
			if (Input.GetKey(controls.down)&&!ready &&!dupItem) {
				recompileLine(lineNumber);
				lineNumber++;
				if(lineNumber>6){//Boundary
					lineNumber = 6;
				}
				loadFromSplit(lineNumber);
				actable = false;
				StartCoroutine(Wait(.25f));
			}
			//"Left" key assign pressed
			if (Input.GetKey(controls.left)&&!ready) {
				spotNumber--;
				if(spotNumber<0){
					spotNumber = 0;
				}
				swapPartsInLine();
				actable = false;
				StartCoroutine(Wait(.25f));
			}
			//"Right" key assign pressed
			if (Input.GetKey(controls.right)&&!ready) {
				spotNumber++;
				if(spotNumber>loadLine.Length-1){
					spotNumber = loadLine.Length-1;
				}
				swapPartsInLine();
				actable = false;
				StartCoroutine(Wait(.25f));
			}
			if(Input.GetKeyDown(controls.attack) || (controls.joyUsed &&  Input.GetButtonDown(controls.joyAttack))) {
				ready = true;
				actable = false;
				StartCoroutine(Wait(.25f));
			}
			if(Input.GetKeyDown (controls.secItem) || (controls.joyUsed && Input.GetButtonDown(controls.joySecItem))) {//Choose item to be in slot
				if(ready){
					ready = false;
				}
				actable = false;
				StartCoroutine(Wait(.25f));
			}
		}
		if(actable&&!on){
			if(Input.GetKeyDown(controls.attack) || (controls.joyUsed &&  Input.GetButtonDown(controls.joyAttack))) {
				on = true;
				StartCoroutine(Wait(.25f));
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
	//Set the duplicates
	private void dupLoad(int num){//Has to have loaded from text already
		loadFromSplit(4);
		itemsUsed[0] = loadLine[0];
		loadFromSplit(5);
		itemsUsed[1] = loadLine[0];
		loadFromSplit(6);
		itemsUsed[2] = loadLine[0];
		if(itemsUsed[0] == itemsUsed[1] || itemsUsed[0] == itemsUsed[2] || itemsUsed[1] == itemsUsed[2]){
			dupItem = true;
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
		loadLine[spotNumber] = loadLine[0];
	}
	private void loadFromSplit(int number){//Requires number of line working on, 1-7
		loadLine = loadData[number].Split('#');
	}
	private void recompileLine(int number){
		string temp = "";
		for(int i = 0; i<loadLine.Length-2; i++){
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
