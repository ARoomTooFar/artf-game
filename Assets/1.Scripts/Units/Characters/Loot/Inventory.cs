using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Inventory : MonoBehaviour {
	public GameObject[] loadOuts;
	public string[] loadData;//From Loadgear
	public string[] loadLine;//Expansion
	public Controls control;
	public bool on;
	public int playNumber, spotNumber;
	// Use this for initialization
	void Start () {
		loadFromText("P" + (playNumber).ToString());
		
		loadFromSplit(spotNumber);
		
		recompileLine(loadLine, spotNumber);
		recompileText("P" + (playNumber).ToString());
	}
	
	// Update is called once per frame
	void Update () {
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
		for(int i = 0; i<text.Length; i++){
			temp = temp + text[i] + '#';
		}
		temp = temp + "W8";
		loadData[number] = temp;
	}
	private void recompileText(string text){//Requires P#
		//loadData[number] = text;
		string temp = "";
		for(int i = 0; i<loadData.Length-1; i++){
			temp = temp + loadData[i] + '\n';
		}
		temp = temp + loadData[loadData.Length-1];
		
		StreamWriter sw = new StreamWriter("Assets/Resources/Testdata/" + text +".txt");
		//sw.Write("Item escolhido: ");
		//sw.WriteLine(item.name);
		//sw.Write("Tempo: ");'
		sw.Write(temp);
		//sw.WriteLine(Time.time);
		sw.Close();
		
	}
}
