using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestLevelSelectCtrl : MonoBehaviour {
	public Controls controls;

	private GSManager gsManager;
	private string[] levelList;
	//private string testLevelListData = "1,3,2,4,5";
	//private WWW matchmakeReq;

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
		levelList = matchmakeData.Split (',');

		/*string[] test = serv.parseListLevelData(testLevelListData);
		Debug.Log (test);
		foreach (string data in test) {
			Debug.Log (data);
		}*/

		// setup keypad (caps)
		currMenuPtr = new GameObject[5, 1];
		currMenuPtr[0, 0] = GameObject.Find("/Canvas/Button");
		currMenuPtr[1, 0] = GameObject.Find("/Canvas/Button 1");
		currMenuPtr[2, 0] = GameObject.Find("/Canvas/Button 2");
		currMenuPtr[3, 0] = GameObject.Find("/Canvas/Button 3");
		currMenuPtr[4, 0] = GameObject.Find("/Canvas/Button 4");
		/*currMenuPtr[0, 1] = GameObject.Find("/Canvas/Button 5");
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
		currMenuPtr[4, 3] = GameObject.Find("/Canvas/Button 19");*/

		currMenuPtr [0, 0].GetComponent<Button>().GetComponentInChildren<Text> ().text = levelList[0];
		currMenuPtr [1, 0].GetComponent<Button>().GetComponentInChildren<Text> ().text = levelList[1];
		currMenuPtr [2, 0].GetComponent<Button>().GetComponentInChildren<Text> ().text = levelList[2];
		currMenuPtr [3, 0].GetComponent<Button>().GetComponentInChildren<Text> ().text = levelList[3];
		currMenuPtr [4, 0].GetComponent<Button>().GetComponentInChildren<Text> ().text = levelList[4];

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

		var pointer = new PointerEventData(EventSystem.current);
		ExecuteEvents.Execute(prevBtn, pointer, ExecuteEvents.pointerExitHandler); // unhighlight previous button
		ExecuteEvents.Execute(currMenuPtr[locY, locX], pointer, ExecuteEvents.pointerEnterHandler); //highlight current button
		prevBtn = currMenuPtr[locY, locX];
	}

	void BtnAction (int levelListIndex) {
		gsManager.currLevelId = levelList[levelListIndex];
		gsManager.LoadLevel (levelList[levelListIndex]);
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
			// check for joystick movement
			MenuMove (Input.GetAxisRaw (controls.hori), Input.GetAxisRaw (controls.vert));
			
			// check for button presses
			if (Input.GetButtonUp (controls.joyAttack)) {
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
