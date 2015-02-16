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

	// Use this for initialization
	void Start () {


		Player1.onClick.AddListener (() => {
			toggle1 (); }); 

		Player2.onClick.AddListener (() => {
			toggle2 (); }); 

		Player3.onClick.AddListener (() => {
			toggle3 (); }); 

		Player4.onClick.AddListener (() => {
			toggle4 (); }); 


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
}
