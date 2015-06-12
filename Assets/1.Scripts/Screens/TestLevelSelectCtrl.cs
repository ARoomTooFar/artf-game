using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Rewired;

public class TestLevelSelectCtrl : MonoBehaviour {
	public Controls controls;

	private GSManager gsManager;
	private string[] levelIdList = new string[20];
	private string[] levelNameList = new string[20];
	private string[] levelOwnerList = new string[20];
	private string[] levelDifficultyList = new string[20];
	//private string testLevelListData = "1,3,2,4,5";
	//private WWW matchmakeReq;

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
		Farts serv = gameObject.AddComponent<Farts>();

		//matchmakeReq = serv.matchmakeWWW (5);
		string matchmakeData = serv.matchmakeWWW ();
		string[] levelDetailData = matchmakeData.Split (',');
		string[] levelDetailData2 = new string[4];

		for (int i = 0; i < levelDetailData.Length; ++i) {
			levelDetailData2 = levelDetailData[i].Split ('|');
			for (int j = 0; j < levelDetailData2.Length; ++j) {
				switch (j) {
				case 0:
					levelIdList[i] = levelDetailData2[j];
					break;
				case 1:
					levelNameList[i] = levelDetailData2[j];
					break;
				case 2:
					levelOwnerList[i] = levelDetailData2[j];
					break;
				case 3:
					levelDifficultyList[i] = levelDetailData2[j];
					break;
				}
			}
		}

		//levelIdList [0] = "4867770441269248";
		//levelNameList [0] = "walk in the park";

		/*string[] test = serv.parseListLevelData(testLevelListData);
		Debug.Log (test);
		foreach (string data in test) {
			Debug.Log (data);
		}*/

		// setup level select control
		currMenuPtr = new GameObject[5, 4];
		currMenuPtr[0, 0] = GameObject.Find("/Canvas/Button");
		currMenuPtr[1, 0] = GameObject.Find("/Canvas/Button 1");
		currMenuPtr[2, 0] = GameObject.Find("/Canvas/Button 2");
		currMenuPtr[3, 0] = GameObject.Find("/Canvas/Button 3");
		currMenuPtr[4, 0] = GameObject.Find("/Canvas/Button 4");
		currMenuPtr[0, 1] = GameObject.Find("/Canvas/Button 5");
		currMenuPtr[1, 1] = GameObject.Find("/Canvas/Button 6");
		currMenuPtr[2, 1] = GameObject.Find("/Canvas/Button 7");
		currMenuPtr[3, 1] = GameObject.Find("/Canvas/Button 8");
		currMenuPtr[4, 1] = GameObject.Find("/Canvas/Button 9");
		currMenuPtr[0, 2] = GameObject.Find("/Canvas/Button 10");
		currMenuPtr[1, 2] = GameObject.Find("/Canvas/Button 11");
		currMenuPtr[2, 2] = GameObject.Find("/Canvas/Button 12");
		currMenuPtr[3, 2] = GameObject.Find("/Canvas/Button 13");
		currMenuPtr[4, 2] = GameObject.Find("/Canvas/Button 14");
		currMenuPtr[0, 3] = GameObject.Find("/Canvas/Button 15");
		currMenuPtr[1, 3] = GameObject.Find("/Canvas/Button 16");
		currMenuPtr[2, 3] = GameObject.Find("/Canvas/Button 17");
		currMenuPtr[3, 3] = GameObject.Find("/Canvas/Button 18");
		currMenuPtr[4, 3] = GameObject.Find("/Canvas/Button 19");

		SetTxt (currMenuPtr [0, 0], 0);
		SetTxt (currMenuPtr [1, 0], 1);
		SetTxt (currMenuPtr [2, 0], 2);
		SetTxt (currMenuPtr [3, 0], 3);
		SetTxt (currMenuPtr [4, 0], 4);
		SetTxt (currMenuPtr [0, 1], 5);
		SetTxt (currMenuPtr [1, 1], 6);
		SetTxt (currMenuPtr [2, 1], 7);
		SetTxt (currMenuPtr [3, 1], 8);
		SetTxt (currMenuPtr [4, 1], 9);
		SetTxt (currMenuPtr [0, 2], 10);
		SetTxt (currMenuPtr [1, 2], 11);
		SetTxt (currMenuPtr [2, 2], 12);
		SetTxt (currMenuPtr [3, 2], 13);
		SetTxt (currMenuPtr [4, 2], 14);
		SetTxt (currMenuPtr [0, 3], 15);
		SetTxt (currMenuPtr [1, 3], 16);
		SetTxt (currMenuPtr [2, 3], 17);
		SetTxt (currMenuPtr [3, 3], 18);
		SetTxt (currMenuPtr [4, 3], 19);

		currMenuPtr[0, 0].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (0);
		});

		currMenuPtr[1, 0].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (1);
		});

		currMenuPtr[2, 0].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (2);
		});

		currMenuPtr[3, 0].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (3);
		});

		currMenuPtr[4, 0].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (4);
		});

		currMenuPtr[0, 1].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (5);
		});
		
		currMenuPtr[1, 1].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (6);
		});
		
		currMenuPtr[2, 1].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (7);
		});
		
		currMenuPtr[3, 1].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (8);
		});
		
		currMenuPtr[4, 1].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (9);
		});

		currMenuPtr[0, 2].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (10);
		});
		
		currMenuPtr[1, 2].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (11);
		});
		
		currMenuPtr[2, 2].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (12);
		});
		
		currMenuPtr[3, 2].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (13);
		});
		
		currMenuPtr[4, 2].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (14);
		});

		currMenuPtr[0, 3].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (15);
		});
		
		currMenuPtr[1, 3].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (16);
		});
		
		currMenuPtr[2, 3].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (17);
		});
		
		currMenuPtr[3, 3].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (18);
		});
		
		currMenuPtr[4, 3].GetComponent<Button>().onClick.AddListener(() => {
			BtnAction (19);
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

		gsManager.maxReady = 0;
		
		// figure out max number of players playing
		foreach (PlayerData playerData in gsManager.playerDataList) {
			if (playerData != null) {
				++gsManager.maxReady;
			}
		}

		Debug.Log ("maxReady: " + gsManager.maxReady);
	}

	void BtnAction (int levelIdListIndex) {
        /*Debug.Log("YOYOYO PERSISTENCE DAWG");
        foreach (PlayerData playerData in gsManager.playerDataList) {
            if (playerData != null) {
                Debug.Log(playerData);
                playerData.PrintData();
            } else {
                Debug.Log("Empty Player");
			}
        }
        Debug.Log(gsManager.playerDataList);*/
		gsManager.currLevelId = levelIdList[levelIdListIndex];
		//gsManager.LoadLevel (levelIdList[levelIdListIndex]);
		gsManager.LoadScene ("GearSelect");
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

	void SetTxt (GameObject button, int lvlListLoc) {
		if (lvlListLoc < levelIdList.Length) {
			Text[] btnText = button.GetComponent<Button> ().GetComponentsInChildren<Text> ();

			for (int i = 0; i < btnText.Length; ++i) {
				switch (i) {
				case 0:
					btnText [0].text = levelNameList [lvlListLoc];
					break;
				case 1:
					btnText [1].text = "Owner: " + levelOwnerList [lvlListLoc];
					break;
				case 2:
					btnText [2].text = "Difficulty: " + levelDifficultyList [lvlListLoc];
					break;
				}
			}
		}
	}

	void Update () {
		// UI controls
		if (menuLock == false) {
//			MenuMove (Input.GetAxisRaw (controls.hori), Input.GetAxisRaw (controls.vert));
			float horiz = 0;
			float verti = 0;
			if (Input.GetKeyDown(controls.up)) verti = 1;
			if (Input.GetKeyDown(controls.down)) verti = -1;
			if (Input.GetKeyDown(controls.left)) horiz = -1;
			if (Input.GetKeyDown(controls.right)) horiz = 1;
			MenuMove (horiz, verti);
			MenuMove (cont.GetAxisRaw ("Move Horizontal"), cont.GetAxisRaw ("Move Vertical") * (-1f));
			
			// check for button presses
//			if (Input.GetButtonUp (controls.joyAttack)) {
			if (cont.GetButtonUp ("Fire") || Input.GetKeyDown(controls.attack)){
				var pointer = new PointerEventData (EventSystem.current);
				ExecuteEvents.Execute (currMenuPtr [locY, locX], pointer, ExecuteEvents.submitHandler);
			}
		}

		/*// matchmake loading
		if (matchmakeReq != null && matchmakeReq.isDone == false) {
			//Debug.Log ("login progress"); // show login loading msg
		} else if (matchmakeReq != null && matchmakeReq.isDone) {
			Debug.Log (matchmakeReq.url);
			Debug.Log (matchmakeReq.text);
			matchmakeReq = null;
		}*/
	}
}
