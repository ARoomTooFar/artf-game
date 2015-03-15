using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class ButtonBehavior : MonoBehaviour {

	public Button Player1;
	public Button Player2;
	public Button Player3;
	public Button Player4;
	public GameObject Player1Items;
	public GameObject Player2Items;
	public GameObject Player3Items;
	public GameObject Player4Items;
	public InputField UserField1;
	public InputField UserField2;
	public InputField UserField3;
	public InputField UserField4;
	public InputField PassField1;
	public InputField PassField2;
	public InputField PassField3;
	public InputField PassField4;


	// Use this for initialization
	void Start () {

		//These are for the Player buttons that unfold the menus.
		Player1.onClick.AddListener (() => {
			toggle1 (); }); 

		Player2.onClick.AddListener (() => {
			toggle2 (); }); 

		Player3.onClick.AddListener (() => {
			toggle3 (); }); 

		Player4.onClick.AddListener (() => {
			toggle4 (); }); 

		//These are for text input fields Login and Password.
		UserField1.onEndEdit.AddListener((value) => submitUN1(value));

		UserField2.onEndEdit.AddListener((value) => submitUN2(value));

		UserField3.onEndEdit.AddListener((value) => submitUN3(value));

		UserField4.onEndEdit.AddListener((value) => submitUN4(value));

		PassField1.onEndEdit.AddListener((value) => submitPW1(value));

		PassField2.onEndEdit.AddListener((value) => submitPW2(value));

		PassField3.onEndEdit.AddListener((value) => submitPW3(value));

		PassField4.onEndEdit.AddListener((value) => submitPW4(value));


	}

	void toggle1(){
		Player1Items.SetActive (!Player1Items.activeSelf);
	}

	void toggle2(){
		Player2Items.SetActive (!Player2Items.activeSelf);
	}

	void toggle3(){
		Player3Items.SetActive (!Player3Items.activeSelf);
	}

	void toggle4(){
		Player4Items.SetActive (!Player4Items.activeSelf);
	}

	void submitUN1(string un){
		gamestate.Instance.addUsername (un, 1);
	}

	void submitUN2(string un){
		gamestate.Instance.addUsername (un, 2);
	}

	void submitUN3(string un){
		gamestate.Instance.addUsername (un, 3);
	}

	void submitUN4(string un){
		gamestate.Instance.addUsername (un, 4);
	}

	void submitPW1(string pw){
		gamestate.Instance.addPassword (pw, 1);
	}

	void submitPW2(string pw){
		gamestate.Instance.addPassword (pw, 2);
	}

	void submitPW3(string pw){
		gamestate.Instance.addPassword (pw, 3);
	}

	void submitPW4(string pw){
		gamestate.Instance.addPassword (pw, 4);
	}
}
