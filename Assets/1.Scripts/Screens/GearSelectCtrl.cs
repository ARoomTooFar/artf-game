using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GearSelectCtrl : MonoBehaviour {
	public Controls controls;
	public string panelName;
	public string confirmPopUpName;
	public string upgradePopUpName;
	
	private GSManager gsManager;
	
	// UI state
	private bool menuMoved = false;
	private bool menuLock = false;
	private GameObject prevBtn;
	private GameObject[,] currMenuPtr;
	private Animator currAnim;
	private int locX = 0;
	private int locY = 0;
	private int[] currItemArr;
	private enum Menu {
		Panel,
		Confirm,
		Upgrade
	}
	private Menu currMenu;
	private Menu prevMenu;
	
	// gear menu
	private GameObject[,] gearMenu;
	private int gearMenuWidth = 1;
	private int gearMenuHeight = 7;

	// confirm pop-up
	private GameObject[,] confirmPopUp;
	private int confirmPopUpWidth = 1;
	private int confirmPopUpHeight = 1;
	private Animator confirmPopUpAnim;

	// upgrade pop-up
	private GameObject[,] upgradePopUp;
	private int upgradePopUpWidth = 1;
	private int upgradePopUpHeight = 1;
	private Animator upgradePopUpAnim;
	
	// inventory data
	private PlayerData playerData;
	private int[][] items = new int[8][];
	private int weaponsIndex = 0;
	private int helmetsIndex = 0;
	private int armorsIndex = 0;
	private int actionSlot1Index = 0;
	private int actionSlot2Index = 0;
	private int actionSlot3Index = 0;
	private int[] weapons = new int[20];
	private int[] helmets = new int[13];
	private int[] armors = new int[9];
	private int[] actionSlot1 = new int[10];
	private int[] actionSlot2 = new int[10];
	private int[] actionSlot3 = new int[10];

	void Start () {
		gsManager = GameObject.Find("GSManager").GetComponent<GSManager>();
//		Farts serv = gameObject.AddComponent<Farts>();

		/* setup gear menu */
		gearMenu = new GameObject[gearMenuHeight, gearMenuWidth];
		gearMenu[0, 0] = GameObject.Find("/Canvas/" + gameObject.name + "/" + panelName + "/WeaponSlot");
		gearMenu[1, 0] = GameObject.Find("/Canvas/" + gameObject.name + "/" + panelName + "/HelmetSlot");
		gearMenu[2, 0] = GameObject.Find("/Canvas/" + gameObject.name + "/" + panelName + "/ArmorSlot");
		gearMenu[3, 0] = GameObject.Find("/Canvas/" + gameObject.name + "/" + panelName + "/ActionSlot1");
		gearMenu[4, 0] = GameObject.Find("/Canvas/" + gameObject.name + "/" + panelName + "/ActionSlot2");
		gearMenu[5, 0] = GameObject.Find("/Canvas/" + gameObject.name + "/" + panelName + "/ActionSlot3");
		gearMenu[6, 0] = GameObject.Find("/Canvas/" + gameObject.name + "/" + panelName + "/BtnReady");

		gearMenu[0, 0].GetComponent<Button>().onClick.AddListener(() =>
		{
			PanelDisable ();
			MenuSwitch (Menu.Upgrade);
		});


		// ready button
		gearMenu[6, 0].GetComponent<Button>().onClick.AddListener(() =>
		{
			Debug.Log ("ready");
			PanelDisable ();
			MenuSwitch (Menu.Confirm);
		});

		/* setup confirm pop-up */
		confirmPopUp = new GameObject[confirmPopUpHeight, confirmPopUpWidth];
		confirmPopUp[0, 0] = GameObject.Find("/Canvas/" + gameObject.name + "/" + confirmPopUpName + "/BtnUnready");

		// unready button
		confirmPopUp[0, 0].GetComponent<Button>().onClick.AddListener(() =>
		{
			Debug.Log ("unready");
			PanelEnable ();
			MenuSwitch (Menu.Panel);
		});

		confirmPopUpAnim = GameObject.Find ("/Canvas/" + gameObject.name + "/" + confirmPopUpName).GetComponent<Animator>();

		/* setup upgrade pop-up */
		upgradePopUp = new GameObject[upgradePopUpHeight, upgradePopUpWidth];
		upgradePopUp[0, 0] = GameObject.Find("/Canvas/" + gameObject.name + "/" + upgradePopUpName + "/BtnBack");
		
		// unready button
		upgradePopUp[0, 0].GetComponent<Button>().onClick.AddListener(() =>
		{
			Debug.Log ("upgrade");
			PanelEnable ();
			MenuSwitch (Menu.Panel);
		});
		
		upgradePopUpAnim = GameObject.Find ("/Canvas/" + gameObject.name + "/" + upgradePopUpName).GetComponent<Animator>();

		// start controls on gear menu
		currMenuPtr = gearMenu;
		var pointer = new PointerEventData(EventSystem.current);
		ExecuteEvents.Execute(prevBtn, pointer, ExecuteEvents.pointerExitHandler); // unhighlight previous button
		ExecuteEvents.Execute(currMenuPtr[locY, locX], pointer, ExecuteEvents.pointerEnterHandler); //highlight current button
		prevBtn = currMenuPtr[locY, locX];

		// begin to load player gear
		if (panelName == "P1Panel") {
			playerData = gsManager.dummyPlayerDataList [0];
		} else if (panelName == "P2Panel") {
			playerData = gsManager.dummyPlayerDataList [1];
		} else if (panelName == "P3Panel") {
			playerData = gsManager.dummyPlayerDataList [2];
		} else {
			playerData = gsManager.dummyPlayerDataList [3];
		}

		items [0] = weapons;
		items [1] = helmets;
		items [2] = armors;
		items [3] = actionSlot1;
		items [4] = actionSlot2;
		items [5] = actionSlot3;
		items [7] = new int[1]{0}; // empty slot to cover left and right controls on ready btn

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

		// load action slot 1
		for (int i = 0; i < actionSlot1.Length; ++i) {
			actionSlot1[i] = playerData.inventory[i];
		}

		// load action slot 2
		for (int i = 0; i < actionSlot2.Length; ++i) {
			actionSlot2[i] = playerData.inventory[i];
		}

		// load action slot 3
		for (int i = 0; i < actionSlot3.Length; ++i) {
			actionSlot3[i] = playerData.inventory[i];
		}

		// print items
		/*for (int i = 0; i < items.Length; ++i) {
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
		}*/

		currItemArr = items [locY]; //align item array with first highlighted button

		Image weaponIF;
		Sprite newSprite = Resources.Load<Sprite>("ItemFrames/HuntersRifle");
		Debug.Log (newSprite);
		weaponIF = GameObject.Find ("ImgItemFrame").GetComponent<Image> ();
		Debug.Log (weaponIF);
		Debug.Log (weaponIF.sprite);
		weaponIF.sprite = newSprite;
		Debug.Log (weaponIF.sprite);
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
				if (locY <= 3) {
					currItemArr = items[locY];
				}
				Debug.Log (currItemArr);
			} else if (vert > 0) {
				--locY;
				if (locY < 0)
				{
					locY = currMenuPtr.GetLength(0) - 1;
				}
				currItemArr = items[locY];
				Debug.Log (currItemArr);
			}

			ItemSwitch (hori);
			
			var pointer = new PointerEventData(EventSystem.current);
			ExecuteEvents.Execute(prevBtn, pointer, ExecuteEvents.pointerExitHandler); // unhighlight previous button
			ExecuteEvents.Execute(currMenuPtr[locY, locX], pointer, ExecuteEvents.pointerEnterHandler); //highlight current button
			prevBtn = currMenuPtr[locY, locX];
			currAnim = confirmPopUpAnim;
			
			//Debug.Log(locX + "," + locY);
		}
	}

	// controls item switching on slot when left or right is pressed
	void ItemSwitch(float hori) {
		switch (locY) {
		case 0:
			if (hori > 0) {
				weaponsIndex = (weaponsIndex + 1) % (currItemArr.Length);
			} else if (hori < 0) {
				--weaponsIndex;
				if (weaponsIndex < 0) {
					weaponsIndex = currItemArr.Length - 1;
				}
			};
			Debug.Log (currItemArr[weaponsIndex]);
			break;
		case 1:
			if (hori > 0) {
				helmetsIndex = (helmetsIndex + 1) % (currItemArr.Length);
			} else if (hori < 0) {
				--helmetsIndex;
				if (helmetsIndex < 0) {
					helmetsIndex = currItemArr.Length - 1;
				}
			};
			Debug.Log (currItemArr[helmetsIndex]);
			break;
		case 2:
			if (hori > 0) {
				armorsIndex = (armorsIndex + 1) % (currItemArr.Length);
			} else if (hori < 0) {
				--armorsIndex;
				if (armorsIndex < 0) {
					armorsIndex = currItemArr.Length - 1;
				}
			};
			Debug.Log (currItemArr[armorsIndex]);
			break;
		case 3:
			if (hori > 0) {
				actionSlot1Index = (actionSlot1Index + 1) % (currItemArr.Length);
			} else if (hori < 0) {
				--actionSlot1Index;
				if (actionSlot1Index < 0) {
					actionSlot1Index = currItemArr.Length - 1;
				}
			};
			Debug.Log (currItemArr[actionSlot1Index]);
			break;
		case 4:
			if (hori > 0) {
				actionSlot2Index = (actionSlot2Index + 1) % (currItemArr.Length);
			} else if (hori < 0) {
				--actionSlot2Index;
				if (actionSlot2Index < 0) {
					actionSlot2Index = currItemArr.Length - 1;
				}
			};
			Debug.Log (currItemArr[actionSlot2Index]);
			break;
		case 5:
			if (hori > 0) {
				actionSlot3Index = (actionSlot3Index + 1) % (currItemArr.Length);
			} else if (hori < 0) {
				--actionSlot3Index;
				if (actionSlot3Index < 0) {
					actionSlot3Index = currItemArr.Length - 1;
				}
			};
			Debug.Log (currItemArr[actionSlot3Index]);
			break;
		}
	}

	void PanelEnable() {
		// unlock controls
		//menuLock = false;
		
		// return color to buttons
		CanvasGroup panel = GameObject.Find("/Canvas/" + gameObject.name + "/" + panelName).GetComponent<CanvasGroup>();
		panel.interactable = true;
		
		// return color to images
		Image[] imgChildren = panel.gameObject.GetComponentsInChildren <Image> ();
		
		foreach (Image imgChild in imgChildren) {
			if (imgChild.name == panelName) {
				imgChild.color = new Color32(255, 255, 255, 100);
			} else if (imgChild.name == "ImgItemFrame") {
				imgChild.color = new Color(1f, 1f, 1f);
			} else {
				imgChild.color = new Color32(152, 213, 217, 178);
			}
		}
		
		/*// return color to text
		Text[] txtChild = this.GetComponentsInChildren<Text>();
		foreach (Text child in txtChild)
		{
			child.color = new Color32(152, 213, 217, 255);
		}*/
		
		// highlight first button of currMenuPtr
		locX = 0;
		locY = 0;
		var pointer = new PointerEventData(EventSystem.current);
		ExecuteEvents.Execute(currMenuPtr[locY, locX], pointer, ExecuteEvents.pointerEnterHandler);
	}
	
	void PanelDisable() {
		// lock controls
		//menuLock = true;
		
		// grey buttons
		CanvasGroup panel = GameObject.Find("/Canvas/" + gameObject.name + "/" + panelName).GetComponent<CanvasGroup>();
		panel.interactable = false;

		// grey images
		Image[] imgChildren = panel.gameObject.GetComponentsInChildren <Image> ();

		foreach (Image imgChild in imgChildren) {
			imgChild.color = new Color(0.3f, 0.3f, 0.3f);
		}
		
		/*// grey text
		Text[] txtChild = this.GetComponentsInChildren<Text>();
		foreach (Text child in txtChild)
		{
			child.color = new Color(0.3f, 0.3f, 0.3f);
		}*/
	}

	// handles menu switching
	void MenuSwitch (Menu menuToSwitchTo) {
		// hide current menu
		if (currAnim != null) {
			currAnim.SetBool("show", false);
			prevMenu = currMenu;
		}
		
		// switch to new menu
		switch (menuToSwitchTo) {
		case Menu.Panel:
			currMenuPtr = gearMenu;
			currAnim = null;
			break;
		case Menu.Confirm:
			currMenuPtr = confirmPopUp;
			currAnim = confirmPopUpAnim;
			break;
		case Menu.Upgrade:
			currMenuPtr = upgradePopUp;
			currAnim = upgradePopUpAnim;
			break;
		}
		
		// setup first button highlight and show new menu
		currMenu = menuToSwitchTo;
		var pointer = new PointerEventData(EventSystem.current);
		ExecuteEvents.Execute(prevBtn, pointer, ExecuteEvents.pointerExitHandler); // unhighlight previous button
		locY = 0;
		locX = 0;
		ExecuteEvents.Execute(currMenuPtr[locY, locX], pointer, ExecuteEvents.pointerEnterHandler);
		prevBtn = currMenuPtr[locY, locX];
		if (currAnim != null) {
			currAnim.SetBool("show", true);
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
