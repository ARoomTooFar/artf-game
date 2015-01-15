using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Listener_BottomBar : MonoBehaviour {
	public Button Button_Tile = null;
	public Button Button_Puzzle = null;
	public Button Button_Monster = null;
	public Button Button_Trap = null;
	public Button Button_Rooms = null;
	public Button Button_Misc = null;

	void Start () {
		Button_Tile.onClick.AddListener (() => {
			checkWhichButtonClicked (); });
		Button_Puzzle.onClick.AddListener (() => {
			checkWhichButtonClicked ();});
		Button_Monster.onClick.AddListener (() => {
			checkWhichButtonClicked (); });
		Button_Trap.onClick.AddListener (() => {
			checkWhichButtonClicked ();});
		Button_Rooms.onClick.AddListener (() => {
			checkWhichButtonClicked (); });
		Button_Misc.onClick.AddListener (() => {
			checkWhichButtonClicked ();});

	}

	void Update () {

	}

	public void checkWhichButtonClicked(){
		
	}


}
