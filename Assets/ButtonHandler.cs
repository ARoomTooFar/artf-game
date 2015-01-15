using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHandler : MonoBehaviour, IPointerClickHandler {
	public GameObject[] tests;
	public GameObject testsHolder;
	int numberOftests;
	
	void Start () {
		numberOftests = testsHolder.gameObject.transform.GetChildCount();
		Debug.Log (numberOftests);
		tests = new GameObject[numberOftests];

		for(int i = 0; i < numberOftests; i++)
		{
			tests[i] = testsHolder.gameObject.transform.GetChild(i).gameObject;
		}

	}

	void Update () {
	
	}
	
	public void OnPointerClick(PointerEventData data) {
		tests[0].SetActive(!tests[0].activeSelf);
		tests[1].SetActive(!tests[1].activeSelf);
		tests[2].SetActive(!tests[2].activeSelf);

		setFolderState(this.name);


	}

	void setFolderState(string name){
		Debug.Log (name.Substring(name.IndexOf('_', name.Length)));

//		for(int i = 0; i < numberOftests; i++){
//			string typeOfFolder = name.Substring(name.IndexOf('_', name.Length);
//			if(String.Equals(name, "Button_Puzzle")) Debug.Log ("sf");
//		}
	}
}
