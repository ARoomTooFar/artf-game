using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Inventory : MonoBehaviour {
	public GameObject[] loadOuts;
	public string[] loadData;//From Loadgear
	public string[] loadLine;//Expansion
	public Controls controls;
	public bool on,actable,ready;
	public int playNumber, spotNumber, lineNumber; //1-4, 1-length of spot line, 1-6;
	// Use this for initialization
	void Start () {
		actable = true;
		lineNumber = 1; //Don't want player name
		spotNumber = 0; 
		loadFromText("P" + (playNumber).ToString());
		if(on){
			loadFromSplit(lineNumber);
		}
		
		//recompileLine(loadLine, lineNumber);
		//recompileText("P" + (playNumber).ToString());
	}
	
	// Update is called once per frame
	void Update () {
		if(actable&&on){
			if (Input.GetKey(controls.up) &&!ready) {
				lineNumber--;
				if(lineNumber<1){//Boundary
					lineNumber = 1;
				}
				actable = false;
				StartCoroutine(Wait(.25f));
			}
			//"Down" key assign pressed
			if (Input.GetKey(controls.down)&&!ready) {
				lineNumber++;
				if(lineNumber>6){//Boundary
					lineNumber = 6;
				}
				actable = false;
				StartCoroutine(Wait(.25f));
			}
			//"Left" key assign pressed
			if (Input.GetKey(controls.left)&&!ready) {
				spotNumber--;
				if(spotNumber<0){
					spotNumber = 0;
				}
				actable = false;
				StartCoroutine(Wait(.25f));
			}
			//"Right" key assign pressed
			if (Input.GetKey(controls.right)&&!ready) {
				spotNumber++;
				if(spotNumber>loadLine.Length){
					spotNumber = loadLine.Length;
				}
				actable = false;
				StartCoroutine(Wait(.25f));
			}
			if(Input.GetKeyDown(controls.attack) || (controls.joyUsed &&  Input.GetButtonDown(controls.joyAttack))) {
				ready = true;
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
	//Taken from Loadgear for base
	private void loadFromText(string text) {//Requires P#
		TextAsset gearData = (TextAsset)Resources.Load("Testdata/" + text, typeof(TextAsset));
		loadData = gearData.text.Split('\n');
		Debug.LogWarning(loadData.Length);
		if (loadData.Length == 0) Debug.LogWarning("Load data file " + text + " is empty.");
		else if (loadData.Length != 7) Debug.LogWarning("Load data file " + text + " does not contain exactly 7 lines.");
		
	}
	private void loadFromSplit(int number){//Requires number of line working on, 1-7
		loadLine = loadData[number].Split('#');
	}
	private void recompileLine(string[] text, int number){
		string temp = "";
		for(int i = 0; i<text.Length-1; i++){
			temp = temp + text[i] + '#';
		}
		temp = temp + text[text.Length];
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
