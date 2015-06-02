using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GearSelectCtrl : MonoBehaviour {
	public Controls controls;
	public string menuPanelName;
	
	private GSManager gsManager;
	
	// UI state
	private bool menuMoved = false;
	private bool menuLock = false;
	private GameObject prevBtn;
	private GameObject[,] currMenuPtr;
	private int locX = 0;
	private int locY = 0;
	private int itemsIndex = 0;

	// inventory data
	private PlayerData playerData;
	private int[][] items = new int[4][];
	private int[] weapons = new int[20];
	private int[] helmets = new int[13];
	private int[] armors = new int[9];
	private int[] actionSlots = new int[10];

	void Start () {
		gsManager = GameObject.Find("GSManager").GetComponent<GSManager>();
		Farts serv = gameObject.AddComponent<Farts>();

		currMenuPtr = new GameObject[7, 1];
		currMenuPtr[0, 0] = GameObject.Find("/Canvas/" + menuPanelName + "/WeaponSlot");
		currMenuPtr[1, 0] = GameObject.Find("/Canvas/" + menuPanelName + "/HelmetSlot");
		currMenuPtr[2, 0] = GameObject.Find("/Canvas/" + menuPanelName + "/ArmorSlot");
		currMenuPtr[3, 0] = GameObject.Find("/Canvas/" + menuPanelName + "/ActionSlot1");
		currMenuPtr[4, 0] = GameObject.Find("/Canvas/" + menuPanelName + "/ActionSlot2");
		currMenuPtr[5, 0] = GameObject.Find("/Canvas/" + menuPanelName + "/ActionSlot3");
		currMenuPtr[6, 0] = GameObject.Find("/Canvas/" + menuPanelName + "/BtnReady");

		var pointer = new PointerEventData(EventSystem.current);
		ExecuteEvents.Execute(prevBtn, pointer, ExecuteEvents.pointerExitHandler); // unhighlight previous button
		ExecuteEvents.Execute(currMenuPtr[locY, locX], pointer, ExecuteEvents.pointerEnterHandler); //highlight current button
		prevBtn = currMenuPtr[locY, locX];

		// begin to load player gear
		if (menuPanelName == "P1Gear") {
			playerData = gsManager.dummyPlayerDataList [0];
		} else if (menuPanelName == "P2Gear") {
			playerData = gsManager.dummyPlayerDataList [1];
		} else if (menuPanelName == "P3Gear") {
			playerData = gsManager.dummyPlayerDataList [2];
		} else {
			playerData = gsManager.dummyPlayerDataList [3];
		}

		items [0] = weapons;
		items [1] = helmets;
		items [2] = armors;
		items [3] = actionSlots;

		// load weapon data
		for (int i = 0; i < weapons.Length; ++i) {
			weapons[i] = playerData.inventory[i + 10];
		}

		// load helmet data
		for (int i = 0; i < helmets.Length; ++i) {
			helmets[i] = playerData.inventory[i + 39];
		}

		// load armor data
		for (int i = 0; i < armors.Length; ++i) {
			armors[i] = playerData.inventory[i + 31];
		}

		// load action slots
		for (int i = 0; i < actionSlots.Length; ++i) {
			actionSlots[i] = playerData.inventory[i];
		}

		// print items
		for (int i = 0; i < items.Length; ++i) {
			switch (i) {
			case 0:
				Debug.Log ("WEAPONS");
				break;
			case 1:
				Debug.Log ("HELMETS");
				break;
			case 2:
				Debug.Log ("ARMORS");
				break;
			case 3:
				Debug.Log ("ACTION SLOTS");
				break;
			}

			for (int j = 0; j < items[i].Length; ++j) {
				Debug.Log (items[i][j]);
			}
		}

		/*Image weaponIF;
		Sprite newSprite = Resources.Load<Sprite>("ItemFrames/HuntersRifle");
		Debug.Log (newSprite);
		weaponIF = GameObject.Find ("WeaponIF").GetComponent<Image> ();
		Debug.Log (weaponIF);
		Debug.Log (weaponIF.sprite);
		weaponIF.sprite = newSprite;
		Debug.Log (weaponIF.sprite);*/
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
	}
}
