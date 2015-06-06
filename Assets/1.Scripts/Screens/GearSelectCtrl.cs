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
	private PlayerData playerData;
	
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

	// item slot text
	public Text txtWeaponSlot;
	public Text txtHelmetSlot;
	public Text txtArmorSlot;
	public Text txtActionSlot1;
	public Text txtActionSlot2;
	public Text txtActionSlot3;
	public Text txtWeaponTier;
	public Text txtHelmetTier;
	public Text txtArmorTier;
	public Text txtActionTier1;
	public Text txtActionTier2;
	public Text txtActionTier3;
	public Image imgWeapon;
	public Image imgHelmet;
	public Image imgArmor;
	public Image imgActionSlot1;
	public Image imgActionSlot2;
	public Image imgActionSlot3;

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
		Farts serv = gameObject.AddComponent<Farts>();

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
			PanelDisable ();
			MenuSwitch (Menu.Confirm);
			++gsManager.currReady;
			/*//gsManager.playerDataList[0] = gsManager.dummyPlayerDataList[0];
			Debug.Log ("PLAYER DATA LIST");
			Debug.Log ("weapon: " + gsManager.playerDataList[0].weapon);
			Debug.Log ("helmet: " + gsManager.playerDataList[0].headgear);
			Debug.Log ("armor: " + gsManager.playerDataList[0].armor);
			Debug.Log ("actionslot1: " + gsManager.playerDataList[0].actionslot1);
			Debug.Log ("actionslot2: " + gsManager.playerDataList[0].actionslot2);
			Debug.Log ("actionslot3: " + gsManager.playerDataList[0].actionslot3);*/
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
			--gsManager.currReady;
		});

		confirmPopUpAnim = GameObject.Find ("/Canvas/" + gameObject.name + "/" + confirmPopUpName).GetComponent<Animator>();

		/* setup upgrade pop-up */
		upgradePopUp = new GameObject[upgradePopUpHeight, upgradePopUpWidth];
		upgradePopUp[0, 0] = GameObject.Find("/Canvas/" + gameObject.name + "/" + upgradePopUpName + "/BtnBack");
		
		// back button
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

		// fill player data with dummy data
		//gsManager.playerDataList[0] = serv.parseCharData("80PercentLean,123,456,789,9001,1,0,3,0,5,0,7,0,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52");
		//gsManager.playerDataList[1] = serv.parseCharData("Player2Dood,123,456,789,8999,1,0,3,0,5,0,7,0,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52");
		//gsManager.playerDataList[2] = serv.parseCharData("Prinny,123,456,789,9001,0,1,3,2,4,6,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0");
		//gsManager.playerDataList[3] = serv.parseCharData("Eyayayayaya,123,456,789,9001,0,1,3,2,4,6,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0");

		// begin to load player gear
		if (panelName == "P1Panel") {
			playerData = gsManager.playerDataList [0];
		} else if (panelName == "P2Panel") {
			playerData = gsManager.playerDataList [1];
		} else if (panelName == "P3Panel") {
			playerData = gsManager.playerDataList [2];
		} else {
			playerData = gsManager.playerDataList [3];
		}

		// hide panel if player has not logged in
		if (playerData == null) {
			gameObject.SetActive (false);
		} else {
			items [0] = weapons;
			items [1] = helmets;
			items [2] = armors;
			items [3] = actionSlot1;
			items [4] = actionSlot2;
			items [5] = actionSlot3;
			items [7] = new int[1]{0}; // empty slot to cover left and right controls on ready btn

			// load weapon data
			for (int i = 0; i < weapons.Length; ++i) {
				weapons [i] = playerData.inventory [i + 10];
			}

			// load helmet data
			for (int i = 0; i < helmets.Length; ++i) {
				helmets [i] = playerData.inventory [i + 39];
			}

			// load armor data
			for (int i = 0; i < armors.Length; ++i) {
				armors [i] = playerData.inventory [i + 30];
			}

			// load action slot 1
			for (int i = 0; i < actionSlot1.Length; ++i) {
				actionSlot1 [i] = playerData.inventory [i];
			}

			// load action slot 2
			for (int i = 0; i < actionSlot2.Length; ++i) {
				actionSlot2 [i] = playerData.inventory [i];
			}

			// load action slot 3
			for (int i = 0; i < actionSlot3.Length; ++i) {
				actionSlot3 [i] = playerData.inventory [i];
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
				//Debug.Log (currItemArr);
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

	void DisplayItem () {
		switch (weaponsIndex) {
		case 0:
			txtWeaponSlot.text = "Shiv";
			playerData.weapon = 10;
			break;
		case 1:
			txtWeaponSlot.text = "Utility Blade";
			playerData.weapon = 11;
			break;
		case 2:
			txtWeaponSlot.text = "Pruning Blade";
			playerData.weapon = 12;
			break;
		case 3:
			txtWeaponSlot.text = "Plasma Blade";
			playerData.weapon = 13;
			break;
		case 4:
			txtWeaponSlot.text = "Rebar Sword";
			playerData.weapon = 14;
			break;
		case 5:
			txtWeaponSlot.text = "Long Sword";
			playerData.weapon = 15;
			break;
		case 6:
			txtWeaponSlot.text = "Machette";
			playerData.weapon = 16;
			break;
		case 7:
			txtWeaponSlot.text = "Thin Blade";
			playerData.weapon = 17;
			break;
		case 8:
			txtWeaponSlot.text = "Flame Pike";
			playerData.weapon = 18;
			break;
		case 9:
			txtWeaponSlot.text = "Lumber Saw";
			playerData.weapon = 19;
			break;
		case 10:
			txtWeaponSlot.text = "Chainsaw Sword";
			playerData.weapon = 20;
			break;
		case 11:
			txtWeaponSlot.text = "Six Shooter";
			playerData.weapon = 21;
			break;
		case 12:
			txtWeaponSlot.text = "Cop Gun";
			playerData.weapon = 22;
			break;
		case 13:
			txtWeaponSlot.text = "Hi-Compression Pistol";
			playerData.weapon = 23;
			break;
		case 14:
			txtWeaponSlot.text = "Hunting Rifle";
			imgWeapon.sprite = Resources.Load<Sprite> ("ItemFrames/HuntersRifle");
			playerData.weapon = 24;
			break;
		case 15:
			txtWeaponSlot.text = "Laser Rifle";
			playerData.weapon = 25;
			break;
		case 16:
			txtWeaponSlot.text = "Machine Gun";
			playerData.weapon = 26;
			break;
		case 17:
			txtWeaponSlot.text = "Automatic Laser Rifle";
			playerData.weapon = 27;
			break;
		case 18:
			txtWeaponSlot.text = "Shotgun";
			playerData.weapon = 28;
			break;
		case 19:
			txtWeaponSlot.text = "Wall of Lead";
			playerData.weapon = 29;
			break;
		}

		switch (helmetsIndex) {
		case 0:
			txtHelmetSlot.text = "Trash Helmet Light Bulb";
			playerData.headgear = 39;
			break;
		case 1:
			txtHelmetSlot.text = "Trash Helmet Bucket";
			playerData.headgear = 40;
			break;
		case 2:
			txtHelmetSlot.text = "Traffic Cone";
			playerData.headgear = 41;
			break;
		case 3:
			txtHelmetSlot.text = "Military Spike Helmet";
			playerData.headgear = 42;
			break;
		case 4:
			txtHelmetSlot.text = "Military Helmet";
			playerData.headgear = 43;
			break;
		case 5:
			txtHelmetSlot.text = "Biker Helmet";
			playerData.headgear = 44;
			break;
		case 6:
			txtHelmetSlot.text = "Police Helmet";
			playerData.headgear = 45;
			break;
		case 7:
			txtHelmetSlot.text = "Com. Helmet";
			playerData.headgear = 46;
			break;
		case 8:
			txtHelmetSlot.text = "Targeting Visor";
			playerData.headgear = 47;
			break;
		case 9:
			txtHelmetSlot.text = "Bionic Eye";
			playerData.headgear = 48;
			break;
		case 10:
			txtHelmetSlot.text = "Cyber Face Robot";
			playerData.headgear = 49;
			break;
		case 11:
			txtHelmetSlot.text = "Cyber Face Horns";
			playerData.headgear = 50;
			break;
		case 12:
			txtHelmetSlot.text = "Brain Case Visor";
			playerData.headgear = 51;
			break;
		}

		switch (armorsIndex) {
		case 0:
			txtArmorSlot.text = "Shirt & Pants";
			playerData.armor = 30;
			break;
		case 1:
			txtArmorSlot.text = "Poncho";
			playerData.armor = 31;
			break;
		case 2:
			txtArmorSlot.text = "Bullet Proof Vest";
			playerData.armor = 32;
			break;
		case 3:
			txtArmorSlot.text = "Smuggler's Jacket";
			playerData.armor = 33;
			break;
		case 4:
			txtArmorSlot.text = "Mixed Plate Uniform";
			playerData.armor = 34;
			break;
		case 5:
			txtArmorSlot.text = "Mixed Army Uniform";
			playerData.armor = 35;
			break;
		case 6:
			txtArmorSlot.text = "Ceramic Plate";
			playerData.armor = 36;
			break;
		case 7:
			txtArmorSlot.text = "Carbon Fibronic Mesh Suit";
			playerData.armor = 37;
			break;
		case 8:
			txtArmorSlot.text = "Delver's Duster";
			playerData.armor = 38;
			break;
		}

		switch (actionSlot1Index) {
		case 0:
			txtActionSlot1.text = "Sprint";
			playerData.actionslot1 = 0;
			break;
		case 1:
			txtActionSlot1.text = "Roll";
			playerData.actionslot1 = 1;
			break;
		case 2:
			txtActionSlot1.text = "Charge";
			playerData.actionslot1 = 2;
			break;
		case 3:
			txtActionSlot1.text = "Lunge";
			playerData.actionslot1 = 3;
			break;
		case 4:
			txtActionSlot1.text = "Riot Shield";
			playerData.actionslot1 = 4;
			break;
		case 5:
			txtActionSlot1.text = "Nano Triage";
			playerData.actionslot1 = 5;
			break;
		case 6:
			txtActionSlot1.text = "Shock Net";
			playerData.actionslot1 = 6;
			break;
		case 7:
			txtActionSlot1.text = "Chain Grab";
			playerData.actionslot1 = 7;
			break;
		case 8:
			txtActionSlot1.text = "Flare";
			playerData.actionslot1 = 8;
			break;
		case 9:
			txtActionSlot1.text = "Lantern";
			playerData.actionslot1 = 9;
			break;
		}

		switch (actionSlot2Index) {
		case 0:
			txtActionSlot2.text = "Sprint";
			playerData.actionslot2 = 0;
			break;
		case 1:
			txtActionSlot2.text = "Roll";
			playerData.actionslot2 = 1;
			break;
		case 2:
			txtActionSlot2.text = "Charge";
			playerData.actionslot2 = 2;
			break;
		case 3:
			txtActionSlot2.text = "Lunge";
			playerData.actionslot2 = 3;
			break;
		case 4:
			txtActionSlot2.text = "Riot Shield";
			playerData.actionslot2 = 4;
			break;
		case 5:
			txtActionSlot2.text = "Nano Triage";
			playerData.actionslot2 = 5;
			break;
		case 6:
			txtActionSlot2.text = "Shock Net";
			playerData.actionslot2 = 6;
			break;
		case 7:
			txtActionSlot2.text = "Chain Grab";
			playerData.actionslot2 = 7;
			break;
		case 8:
			txtActionSlot2.text = "Flare";
			playerData.actionslot2 = 8;
			break;
		case 9:
			txtActionSlot2.text = "Lantern";
			playerData.actionslot2 = 9;
			break;
		}

		switch (actionSlot3Index) {
		case 0:
			txtActionSlot3.text = "Sprint";
			playerData.actionslot3 = 0;
			break;
		case 1:
			txtActionSlot3.text = "Roll";
			playerData.actionslot3 = 1;
			break;
		case 2:
			txtActionSlot3.text = "Charge";
			playerData.actionslot3 = 2;
			break;
		case 3:
			txtActionSlot3.text = "Lunge";
			playerData.actionslot3 = 3;
			break;
		case 4:
			txtActionSlot3.text = "Riot Shield";
			playerData.actionslot3 = 4;
			break;
		case 5:
			txtActionSlot3.text = "Nano Triage";
			playerData.actionslot3 = 5;
			break;
		case 6:
			txtActionSlot3.text = "Shock Net";
			playerData.actionslot3 = 6;
			break;
		case 7:
			txtActionSlot3.text = "Chain Grab";
			playerData.actionslot3 = 7;
			break;
		case 8:
			txtActionSlot3.text = "Flare";
			playerData.actionslot3 = 8;
			break;
		case 9:
			txtActionSlot3.text = "Lantern";
			playerData.actionslot3 = 9;
			break;
		}

		if (playerData.inventory [playerData.weapon] == 0) {
			gearMenu [0, 0].GetComponent<Button> ().interactable = false;
		} else {
			gearMenu [0, 0].GetComponent<Button> ().interactable = true;
		}

		if (playerData.inventory [playerData.headgear] == 0) {
			gearMenu [1, 0].GetComponent<Button> ().interactable = false;
		} else {
			gearMenu [1, 0].GetComponent<Button> ().interactable = true;
		}

		if (playerData.inventory [playerData.armor] == 0) {
			gearMenu [2, 0].GetComponent<Button> ().interactable = false;
		} else {
			gearMenu [2, 0].GetComponent<Button> ().interactable = true;
		}

		if (playerData.inventory [playerData.actionslot1] == 0) {
			gearMenu [3, 0].GetComponent<Button> ().interactable = false;
		} else {
			gearMenu [3, 0].GetComponent<Button> ().interactable = true;
		}

		if (playerData.inventory [playerData.actionslot2] == 0) {
			gearMenu [4, 0].GetComponent<Button> ().interactable = false;
		} else {
			gearMenu [4, 0].GetComponent<Button> ().interactable = true;
		}

		if (playerData.inventory [playerData.actionslot3] == 0) {
			gearMenu [5, 0].GetComponent<Button> ().interactable = false;
		} else {
			gearMenu [5, 0].GetComponent<Button> ().interactable = true;
		}

		// if any items are locked do not let player ready
		if (playerData.inventory [playerData.weapon] == 0 || playerData.inventory [playerData.headgear] == 0 || playerData.inventory [playerData.armor] == 0 || playerData.inventory [playerData.actionslot1] == 0 || playerData.inventory [playerData.actionslot2] == 0 || playerData.inventory [playerData.actionslot3] == 0) {
			gearMenu [6, 0].GetComponent<Button> ().interactable = false;
		} else {
			gearMenu [6, 0].GetComponent<Button> ().interactable = true;
		}
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
			currItemArr = weapons;
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

		DisplayItem ();
	}
}
