using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rewired;

public class GearSelectCtrl : MonoBehaviour {
	public Controls controls;
	public string panelName;
	public string confirmPopUpName;
	public string upgradePopUpName;
	
	private GSManager gsManager;
	private PlayerData playerData;

	//Input
	public int playerControl;
	private Rewired.Player cont;
	
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
	private int actionSlot2Index = 1;
	private int actionSlot3Index = 2;
	private int[] weapons = new int[20];
	private int[] helmets = new int[13];
	private int[] armors = new int[9];
	private int[] actionSlot1 = new int[10];
	private int[] actionSlot2 = new int[10];
	private int[] actionSlot3 = new int[10];

	void Start () {
		gsManager = GameObject.Find("GSManager").GetComponent<GSManager>();
		Farts serv = gameObject.AddComponent<Farts>();

		prevMenu = Menu.Panel;

		cont = ReInput.players.GetPlayer (playerControl);

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

		// uncomment the block below to load testlevelselect directly
		PlayerData dummyData = serv.parseCharData("86572838366478336,123,0,0,9001,1,1,1,0,1,1,1,1,0,0,6,6,6,6,6,6,6,6,6,6,6,6,0,6,6,6,6,0,6,6,6,0,6,6,6,6,6,6,6,6,6,6,6,0,0,0,6,6,0,6,6,0");
		gsManager.playerDataList[0] = dummyData;
		gsManager.playerDataList[1] = dummyData;
		gsManager.playerDataList[2] = dummyData;
		gsManager.playerDataList[3] = dummyData;

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
				//Debug.Log (currItemArr);
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
			//Debug.Log (currItemArr[weaponsIndex]);
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
			//Debug.Log (currItemArr[helmetsIndex]);
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
			//Debug.Log (currItemArr[armorsIndex]);
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
			//Debug.Log (currItemArr[actionSlot1Index]);
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
			//Debug.Log (currItemArr[actionSlot2Index]);
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
			//Debug.Log (currItemArr[actionSlot3Index]);
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
			imgWeapon.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0038_shiv.png");
			playerData.weapon = 10;
			break;
		case 1:
			txtWeaponSlot.text = "Utility Blade";
			imgWeapon.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0013_fist.png");
			playerData.weapon = 11;
			break;
		case 2:
			txtWeaponSlot.text = "Pruning Blade";
			imgWeapon.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0034_PrundingBlade.png");
			playerData.weapon = 12;
			break;
		case 3:
			txtWeaponSlot.text = "Plasma Blade";
			imgWeapon.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0033_PlasmaBlade.png");
			playerData.weapon = 13;
			break;
		case 4:
			txtWeaponSlot.text = "Rebar Sword";
			imgWeapon.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0035_rebar.png");
			playerData.weapon = 14;
			break;
		case 5:
			txtWeaponSlot.text = "Long Sword";
			imgWeapon.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0026_LongSwrod.png");
			playerData.weapon = 15;
			break;
		case 6:
			txtWeaponSlot.text = "Machette";
			imgWeapon.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0028_Machete.png");
			playerData.weapon = 16;
			break;
		case 7:
			txtWeaponSlot.text = "Thin Blade";
			imgWeapon.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0045_ThinBlade.png");
			playerData.weapon = 17;
			break;
		case 8:
			txtWeaponSlot.text = "Flame Pike";
			imgWeapon.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0016_fusionpike.png");
			playerData.weapon = 18;
			break;
		case 9:
			txtWeaponSlot.text = "Lumber Saw";
			imgWeapon.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0027_lumbersaw.png");
			playerData.weapon = 19;
			break;
		case 10:
			txtWeaponSlot.text = "Chainsaw Sword";
			imgWeapon.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0006_chainswordcon.png");
			playerData.weapon = 20;
			break;
		case 11:
			txtWeaponSlot.text = "Six Shooter";
			imgWeapon.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0039_SixShooter.png");
			playerData.weapon = 21;
			break;
		case 12:
			txtWeaponSlot.text = "Cop Gun";
			imgWeapon.sprite = null;
			playerData.weapon = 22;
			break;
		case 13:
			txtWeaponSlot.text = "Hi-Compression Pistol";
			imgWeapon.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0020_HiCompressionPistol.png");
			playerData.weapon = 23;
			break;
		case 14:
			txtWeaponSlot.text = "Hunting Rifle";
			imgWeapon.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0022_HuntersRifle.png");
			playerData.weapon = 24;
			break;
		case 15:
			txtWeaponSlot.text = "Laser Rifle";
			imgWeapon.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0025_LaserSniperRifle.png");
			playerData.weapon = 25;
			break;
		case 16:
			txtWeaponSlot.text = "Machine Gun";
			imgWeapon.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0024_Laserrifle.png");
			playerData.weapon = 26;
			break;
		case 17:
			txtWeaponSlot.text = "Automatic Laser Rifle";
			imgWeapon.sprite = null;
			playerData.weapon = 27;
			break;
		case 18:
			txtWeaponSlot.text = "Shotgun";
			imgWeapon.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0023_HuntersShotgun.png");
			playerData.weapon = 28;
			break;
		case 19:
			txtWeaponSlot.text = "Wall of Lead";
			imgWeapon.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0053_WallofLead(2).png");
			playerData.weapon = 29;
			break;
		}

		switch (helmetsIndex) {
		case 0:
			txtHelmetSlot.text = "Trash Helmet Light Bulb";
			imgHelmet.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0050_TrashHelmet2.png");
			playerData.headgear = 39;
			break;
		case 1:
			txtHelmetSlot.text = "Trash Helmet Bucket";
			imgHelmet.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0049_TrashHelmet1.png");
			playerData.headgear = 40;
			break;
		case 2:
			txtHelmetSlot.text = "Traffic Cone";
			imgHelmet.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0046_TrafficCone.png");
			playerData.headgear = 41;
			break;
		case 3:
			txtHelmetSlot.text = "Military Spike Helmet";
			imgHelmet.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0041_SpikeHelmet.png");
			playerData.headgear = 42;
			break;
		case 4:
			txtHelmetSlot.text = "Military Helmet";
			imgHelmet.sprite = null;
			playerData.headgear = 43;
			break;
		case 5:
			txtHelmetSlot.text = "Biker Helmet";
			imgHelmet.sprite = null;
			playerData.headgear = 44;
			break;
		case 6:
			txtHelmetSlot.text = "Police Helmet";
			imgHelmet.sprite = null;
			playerData.headgear = 45;
			break;
		case 7:
			txtHelmetSlot.text = "Com. Helmet";
			imgHelmet.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0007_CommsHelmet.png");
			playerData.headgear = 46;
			break;
		case 8:
			txtHelmetSlot.text = "Targeting Visor";
			imgHelmet.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0044_TargetingGoggles.png");
			playerData.headgear = 47;
			break;
		case 9:
			txtHelmetSlot.text = "Bionic Eye";
			imgHelmet.sprite = null;
			playerData.headgear = 48;
			break;
		case 10:
			txtHelmetSlot.text = "Cyber Face Robot";
			imgHelmet.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0009_CyberFace2.png");
			playerData.headgear = 49;
			break;
		case 11:
			txtHelmetSlot.text = "Cyber Face Horns";
			imgHelmet.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0010_CyberFaceDevil.png");
			playerData.headgear = 50;
			break;
		case 12:
			txtHelmetSlot.text = "Brain Case Visor";
			imgHelmet.sprite = null;
			playerData.headgear = 51;
			break;
		}

		switch (armorsIndex) {
		case 0:
			txtArmorSlot.text = "Trash Can Armor";
			imgArmor.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0048_TrashCanArmor.png");
			playerData.armor = 30;
			break;
		case 1:
			txtArmorSlot.text = "Poncho";
			imgArmor.sprite = null;
			playerData.armor = 31;
			break;
		case 2:
			txtArmorSlot.text = "Bullet Proof Vest";
			imgArmor.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0001_BulletProofVest.png");
			playerData.armor = 32;
			break;
		case 3:
			txtArmorSlot.text = "Smuggler's Jacket";
			imgArmor.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0040_SmugglersJacket.png");
			playerData.armor = 33;
			break;
		case 4:
			txtArmorSlot.text = "Mixed Plate Uniform";
			imgArmor.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0030_MixedUniform.png");
			playerData.armor = 34;
			break;
		case 5:
			txtArmorSlot.text = "Mixed Army Uniform";
			imgArmor.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0001_BulletProofVest.png");
			playerData.armor = 35;
			break;
		case 6:
			txtArmorSlot.text = "Ceramic Plate";
			imgArmor.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0005_CeramicPlate.png");
			playerData.armor = 36;
			break;
		case 7:
			txtArmorSlot.text = "Carbon Fibronic Mesh Suit";
			imgArmor.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0031_MixedUniform2.png");;
			playerData.armor = 37;
			break;
		case 8:
			txtArmorSlot.text = "Delver's Duster";
			imgArmor.sprite = Resources.Load<Sprite> ("ItemFrames/IF1_0012_DelversDuster.png");
			playerData.armor = 38;
			break;
		}

		switch (actionSlot1Index) {
		case 0:
			txtActionSlot1.text = "Sprint";
			imgActionSlot1.sprite = Resources.Load<Sprite> ("ItemFrames/sprintif(2)");
			playerData.actionslot1 = 0;
			break;
		case 1:
			txtActionSlot1.text = "Roll";
			imgActionSlot1.sprite = Resources.Load<Sprite> ("ItemFrames/roll");
			playerData.actionslot1 = 1;
			break;
		case 2:
			txtActionSlot1.text = "Charge";
			imgActionSlot1.sprite = Resources.Load<Sprite> ("ItemFrames/bullchargif3");
			playerData.actionslot1 = 2;
			break;
		case 3:
			txtActionSlot1.text = "Lunge";
			imgActionSlot1.sprite = null;
			playerData.actionslot1 = 3;
			break;
		case 4:
			txtActionSlot1.text = "Riot Shield";
			imgActionSlot1.sprite = Resources.Load<Sprite> ("ItemFrames/shieldUI");
			playerData.actionslot1 = 4;
			break;
		case 5:
			txtActionSlot1.text = "Nano Triage";
			imgActionSlot1.sprite = Resources.Load<Sprite> ("ItemFrames/triageUI");
			playerData.actionslot1 = 5;
			break;
		case 6:
			txtActionSlot1.text = "Shock Net";
			imgActionSlot1.sprite = Resources.Load<Sprite> ("ItemFrames/shocknetUI");
			playerData.actionslot1 = 6;
			break;
		case 7:
			txtActionSlot1.text = "Chain Grab";
			imgActionSlot1.sprite = Resources.Load<Sprite> ("ItemFrames/chaingrabif");
			playerData.actionslot1 = 7;
			break;
		case 8:
			txtActionSlot1.text = "Flare";
			imgActionSlot1.sprite = null;
			playerData.actionslot1 = 8;
			break;
		case 9:
			txtActionSlot1.text = "Lantern";
			imgActionSlot1.sprite = null;
			playerData.actionslot1 = 9;
			break;
		}

		switch (actionSlot2Index) {
		case 0:
			txtActionSlot2.text = "Sprint";
			imgActionSlot2.sprite = Resources.Load<Sprite> ("ItemFrames/sprintif(2)");
			playerData.actionslot2 = 0;
			break;
		case 1:
			txtActionSlot2.text = "Roll";
			imgActionSlot2.sprite = Resources.Load<Sprite> ("ItemFrames/roll");
			playerData.actionslot2 = 1;
			break;
		case 2:
			txtActionSlot2.text = "Charge";
			imgActionSlot2.sprite = Resources.Load<Sprite> ("ItemFrames/bullchargif3");
			playerData.actionslot2 = 2;
			break;
		case 3:
			txtActionSlot2.text = "Lunge";
			imgActionSlot2.sprite = null;
			playerData.actionslot2 = 3;
			break;
		case 4:
			txtActionSlot2.text = "Riot Shield";
			imgActionSlot2.sprite = Resources.Load<Sprite> ("ItemFrames/shieldUI");
			playerData.actionslot2 = 4;
			break;
		case 5:
			txtActionSlot2.text = "Nano Triage";
			imgActionSlot2.sprite = Resources.Load<Sprite> ("ItemFrames/triageUI");
			playerData.actionslot2 = 5;
			break;
		case 6:
			txtActionSlot2.text = "Shock Net";
			imgActionSlot2.sprite = Resources.Load<Sprite> ("ItemFrames/shocknetUI");
			playerData.actionslot2 = 6;
			break;
		case 7:
			txtActionSlot2.text = "Chain Grab";
			imgActionSlot2.sprite = Resources.Load<Sprite> ("ItemFrames/chaingrabif");
			playerData.actionslot2 = 7;
			break;
		case 8:
			txtActionSlot2.text = "Flare";
			imgActionSlot2.sprite = null;
			playerData.actionslot2 = 8;
			break;
		case 9:
			txtActionSlot2.text = "Lantern";
			imgActionSlot2.sprite = null;
			playerData.actionslot2 = 9;
			break;
		}

		switch (actionSlot3Index) {
		case 0:
			txtActionSlot3.text = "Sprint";
			imgActionSlot3.sprite = Resources.Load<Sprite> ("ItemFrames/sprintif(2)");
			playerData.actionslot3 = 0;
			break;
		case 1:
			txtActionSlot3.text = "Roll";
			imgActionSlot3.sprite = Resources.Load<Sprite> ("ItemFrames/roll");
			playerData.actionslot3 = 1;
			break;
		case 2:
			txtActionSlot3.text = "Charge";
			imgActionSlot3.sprite = Resources.Load<Sprite> ("ItemFrames/bullchargif3");
			playerData.actionslot3 = 2;
			break;
		case 3:
			txtActionSlot3.text = "Lunge";
			imgActionSlot3.sprite = null;
			playerData.actionslot3 = 3;
			break;
		case 4:
			txtActionSlot3.text = "Riot Shield";
			imgActionSlot3.sprite = Resources.Load<Sprite> ("ItemFrames/shieldUI");
			playerData.actionslot3 = 4;
			break;
		case 5:
			txtActionSlot3.text = "Nano Triage";
			imgActionSlot3.sprite = Resources.Load<Sprite> ("ItemFrames/triageUI");
			playerData.actionslot3 = 5;
			break;
		case 6:
			txtActionSlot3.text = "Shock Net";
			imgActionSlot3.sprite = Resources.Load<Sprite> ("ItemFrames/shocknetUI");
			playerData.actionslot3 = 6;
			break;
		case 7:
			txtActionSlot3.text = "Chain Grab";
			imgActionSlot3.sprite = Resources.Load<Sprite> ("ItemFrames/chaingrabif");
			playerData.actionslot3 = 7;
			break;
		case 8:
			txtActionSlot3.text = "Flare";
			imgActionSlot3.sprite = null;
			playerData.actionslot3 = 8;
			break;
		case 9:
			txtActionSlot3.text = "Lantern";
			imgActionSlot3.sprite = null;
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
//			MenuMove (Input.GetAxisRaw (controls.hori), Input.GetAxisRaw (controls.vert));
			float horiz = 0;
			float verti = 0;
			if (Input.GetKeyDown(controls.up)) verti = 1;
			if (Input.GetKeyDown(controls.down)) verti = -1;
			if (Input.GetKeyDown(controls.left)) horiz = -1;
			if (Input.GetKeyDown(controls.right)) horiz = 1;
			//MenuMove (horiz, verti);
			MenuMove (cont.GetAxisRaw ("Move Horizontal"), cont.GetAxisRaw ("Move Vertical") * (-1f));
			
			// check for button presses
//			if (Input.GetButtonUp (controls.joyAttack)) {
			if (cont.GetButtonUp ("Fire") || Input.GetKeyDown(controls.attack)){
				var pointer = new PointerEventData (EventSystem.current);
				ExecuteEvents.Execute (currMenuPtr [locY, locX], pointer, ExecuteEvents.submitHandler);
			}
		}

		DisplayItem ();
	}
}
