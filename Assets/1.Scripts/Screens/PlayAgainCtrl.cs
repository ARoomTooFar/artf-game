using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Rewired;

public class PlayAgainCtrl : MonoBehaviour {
	public Controls controls;
	
	private GSManager gsManager;
	
	//Input
	public int playerControl;
	private Rewired.Player cont;
	
	// UI state
	private bool menuMoved = false;
	private bool menuLock = false;
	private GameObject prevBtn;
	private GameObject[,] currMenuPtr;
	private int locX = 0;
	private int locY = 0;
	
	void Start () {
		gsManager = GameObject.Find("GSManager").GetComponent<GSManager>();

		currMenuPtr = new GameObject[1, 2];
		currMenuPtr[0, 0] = GameObject.Find("/Canvas/BtnAgain");
		currMenuPtr[0, 1] = GameObject.Find("/Canvas/BtnMenu");
		
		currMenuPtr[0, 0].GetComponent<Button>().onClick.AddListener(() => {
			gsManager.LoadScene ("TestLevelSelect");
		});
		
		currMenuPtr[0, 1].GetComponent<Button>().onClick.AddListener(() => {
			gsManager.LoadScene ("MainMenu");
		});
		
		var pointer = new PointerEventData(EventSystem.current);
		ExecuteEvents.Execute(prevBtn, pointer, ExecuteEvents.pointerExitHandler); // unhighlight previous button
		ExecuteEvents.Execute(currMenuPtr[locY, locX], pointer, ExecuteEvents.pointerEnterHandler); //highlight current button
		prevBtn = currMenuPtr[locY, locX];
		
		// setup leader control
		if (gsManager.leaderList.Count > 0) {
			switch (gsManager.leaderList [0]) {
			case 1:
				Debug.Log ("leader is P2");
				controls.up = "up";
				controls.down = "down";
				controls.left = "left";
				controls.right = "right";
				controls.attack = "return";
				controls.secItem = "delete";
				controls.cycItem = "end";
				controls.hori = "Hori2";
				controls.vert = "Vert2";
				controls.joyAttack = "Attack2";
				controls.joySecItem = "SecItem2";
				controls.joyCycItem = "CycItem2";
				cont = ReInput.players.GetPlayer (3);
				break;
			case 2:
				Debug.Log ("leader is P3");
				controls.up = "i";
				controls.down = "k";
				controls.left = "j";
				controls.right = "l";
				controls.attack = "o";
				controls.secItem = "u";
				controls.cycItem = ".";
				controls.hori = "Hori3";
				controls.vert = "Vert3";
				controls.joyAttack = "Attack3";
				controls.joySecItem = "SecItem3";
				controls.joyCycItem = "CycItem3";
				cont = ReInput.players.GetPlayer (0);
				break;
			case 3:
				Debug.Log ("leader is P4");
				controls.up = "[8]";
				controls.down = "[5]";
				controls.left = "[4]";
				controls.right = "[6]";
				controls.attack = "[9]";
				controls.secItem = "[7]";
				controls.cycItem = "[3]";
				controls.hori = "Hori4";
				controls.vert = "Vert4";
				controls.joyAttack = "Attack4";
				controls.joySecItem = "SecItem4";
				controls.joyCycItem = "CycItem4";
				cont = ReInput.players.GetPlayer (1);
				break;
			default:
				Debug.Log ("leader is P1");
				controls.up = "w";
				controls.down = "s";
				controls.left = "a";
				controls.right = "d";
				controls.attack = "e";
				controls.secItem = "q";
				controls.cycItem = "f";
				controls.hori = "Hori";
				controls.vert = "Vert";
				controls.joyAttack = "Attack";
				controls.joySecItem = "SecItem";
				controls.joyCycItem = "CycItem";
				cont = ReInput.players.GetPlayer (2);
				break;
			}
		} else {
			Debug.Log("no leader set, but defaulting to P1");
			controls.up = "w";
			controls.down = "s";
			controls.left = "a";
			controls.right = "d";
			controls.attack = "e";
			controls.secItem = "q";
			controls.cycItem = "f";
			controls.hori = "Hori";
			controls.vert = "Vert";
			controls.joyAttack = "Attack";
			controls.joySecItem = "SecItem";
			controls.joyCycItem = "CycItem";
			cont = ReInput.players.GetPlayer (2);
		}
	}
	
	// handles menu joystick movement control
	void MenuMove (float hori, float vert) {
		if (vert == 0 && hori == 0)
		{
			menuMoved = false;
		} else if (menuMoved == false) {
			menuMoved = true;
			
			if (vert < 0)
			{
				locY = (locY + 1) % (currMenuPtr.GetLength(0));
			} else if (vert > 0) {
				--locY;
				if (locY < 0)
				{
					locY = currMenuPtr.GetLength(0) - 1;
				}
			}
			
			if (hori > 0)
			{
				locX = (locX + 1) % (currMenuPtr.GetLength(1));
			}
			else if (hori < 0)
			{
				--locX;
				if (locX < 0)
				{
					locX = currMenuPtr.GetLength(1) - 1;
				}
			}
			
			var pointer = new PointerEventData(EventSystem.current);
			ExecuteEvents.Execute(prevBtn, pointer, ExecuteEvents.pointerExitHandler); // unhighlight previous button
			ExecuteEvents.Execute(currMenuPtr[locY, locX], pointer, ExecuteEvents.pointerEnterHandler); //highlight current button
			prevBtn = currMenuPtr[locY, locX];
			
			//Debug.Log(locX + "," + locY);
		}
	}
	
	void Update () {
		// UI controls
		if (menuLock == false) {
			//			MenuMove (Input.GetAxisRaw (controls.hori), Input.GetAxisRaw (controls.vert));
			MenuMove (cont.GetAxisRaw ("Move Horizontal"), cont.GetAxisRaw ("Move Vertical") * (-1f));
			
			// check for button presses
			//			if (Input.GetButtonUp (controls.joyAttack)) {
			if (cont.GetButtonUp ("Fire")){
				var pointer = new PointerEventData (EventSystem.current);
				ExecuteEvents.Execute (currMenuPtr [locY, locX], pointer, ExecuteEvents.submitHandler);
			}
		}
	}
}
