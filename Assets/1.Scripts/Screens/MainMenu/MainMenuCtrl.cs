using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuCtrl : MonoBehaviour {
	public Controls controls;
    public string menuContainerName;
	public string menuPopUpName;

	private Farts serv;

    // UI state
    private bool menuMoved = false;
    private bool menuLock = false;
    private GameObject prevBtn;
	private GameObject[,] currMenuPtr;
	private Animator currAnim;
    private int locX = 0;
    private int locY = 0;
	private enum Menu {
		StartMenu,
		LoginForm,
        PopUp
	}
    private Menu currMenu;
    private Menu prevMenu;
    private Text currFieldPtr;

    // start menu
    private GameObject[,] startMenu;
    private int startMenuWidth = 1;
    private int startMenuHeight = 2;
	private Animator startMenuAnim;

	// login form
    private GameObject[,] loginForm;
    private int loginFormWidth = 2;
    private int loginFormHeight = 3;
	private Animator loginFormAnim;
    private Text txtFieldAcctName;
	private Text txtFieldPasscode;

	// pop-up
	private GameObject[,] popUp;
	private GameObject[,] numPad;
	private int popUpWidth = 3;
	private int popUpHeight = 5;
	private Animator popUpAnim;

    // keypad
	private string currKey = "";
	private string prevKey = "";
	private float pressTime;
	private string tmpCharName;
	private int charArrLoc = 0;
	private Text txtDisplayField;

	void Start () {
		serv = gameObject.AddComponent<Farts>(); // add networking component

        // setup start menu
        startMenu = new GameObject[startMenuHeight, startMenuWidth];
        startMenu[0, 0] = GameObject.Find("/Canvas/" + menuContainerName + "/StartMenu/BtnLogin");
        startMenu[1, 0] = GameObject.Find("/Canvas/" + menuContainerName + "/StartMenu/BtnRegister");
		startMenuAnim = GameObject.Find ("/Canvas/" + menuContainerName + "/StartMenu").GetComponent<Animator>();
		
		// login button press handler
        startMenu[0, 0].GetComponent<Button>().onClick.AddListener(() =>
        {
			MenuSwitch (Menu.LoginForm);
			//MenuDisable();
            //GameObject.Find("/Main Camera").GetComponent<MainMenuCamera>().slideDown = true;
        });

        // register button press handler
        startMenu[1, 0].GetComponent<Button>().onClick.AddListener(() =>
        {
			Debug.Log ("Register button pressed!");
            //MenuSwitch(Menu.LoginForm);

            serv.login("Paradoxium", "pass");
        });

		// setup login
		loginForm = new GameObject[loginFormHeight, loginFormWidth];
		loginForm[0, 0] = loginForm[0, 1] = GameObject.Find("/Canvas/" + menuContainerName + "/LoginForm/FieldAcctName");
        txtFieldAcctName = GameObject.Find("/Canvas/" + menuContainerName + "/LoginForm/FieldAcctName/TxtFieldAcctName").GetComponent<Text>();
		txtFieldPasscode = GameObject.Find("/Canvas/" + menuContainerName + "/LoginForm/FieldPasscode/TxtFieldPasscode").GetComponent<Text>();
        loginForm[1, 0] = loginForm[1, 1] = GameObject.Find("/Canvas/" + menuContainerName + "/LoginForm/FieldPasscode");
        loginForm[2, 0] = GameObject.Find("/Canvas/" + menuContainerName + "/LoginForm/BtnLogin");
        loginForm[2, 1] = GameObject.Find("/Canvas/" + menuContainerName + "/LoginForm/BtnBack");
		loginFormAnim = GameObject.Find ("/Canvas/" + menuContainerName + "/LoginForm").GetComponent<Animator>();

		// acct name field
		loginForm[0, 0].GetComponent<Button>().onClick.AddListener(() =>
		{
			currFieldPtr = txtFieldAcctName;
			PopUpEnable ();
		});

		// passcode field
		loginForm[1, 0].GetComponent<Button>().onClick.AddListener(() =>
        {
			currFieldPtr = txtFieldPasscode;
			PopUpEnable ();
		});

		// submit button
		loginForm[2, 0].GetComponent<Button>().onClick.AddListener(() =>
		{
			serv.login(txtFieldAcctName.text, txtFieldPasscode.text);
		});

		// back button
        loginForm[2, 1].GetComponent<Button>().onClick.AddListener(() =>
        {
			MenuSwitch (Menu.StartMenu);
			MenuReset ();
        });

		// setup keypad (caps)
		popUp = new GameObject[popUpHeight, popUpWidth];
        popUp[0, 0] = GameObject.Find("/Canvas/" + menuPopUpName + "/KeySymbol");
        popUp[0, 1] = GameObject.Find("/Canvas/" + menuPopUpName + "/KeyABC");
        popUp[0, 2] = GameObject.Find("/Canvas/" + menuPopUpName + "/KeyDEF");
        popUp[1, 0] = GameObject.Find("/Canvas/" + menuPopUpName + "/KeyGHI");
        popUp[1, 1] = GameObject.Find("/Canvas/" + menuPopUpName + "/KeyJKL");
        popUp[1, 2] = GameObject.Find("/Canvas/" + menuPopUpName + "/KeyMNO");
        popUp[2, 0] = GameObject.Find("/Canvas/" + menuPopUpName + "/KeyPQRS");
        popUp[2, 1] = GameObject.Find("/Canvas/" + menuPopUpName + "/KeyTUV");
        popUp[2, 2] = GameObject.Find("/Canvas/" + menuPopUpName + "/KeyWXYZ");
        popUp[3, 0] = GameObject.Find("/Canvas/" + menuPopUpName + "/KeyDel");
        popUp[3, 1] = GameObject.Find("/Canvas/" + menuPopUpName + "/KeySpace");
        popUp[3, 2] = GameObject.Find("/Canvas/" + menuPopUpName + "/KeySwap");
        popUp[4, 0] = popUp[4, 1] = popUp[4, 2] = GameObject.Find("/Canvas/" + menuPopUpName + "/BtnSubmit");
		popUpAnim = GameObject.Find ("/Canvas/" + menuPopUpName).GetComponent<Animator>();
        txtDisplayField = GameObject.Find("/Canvas/" + menuPopUpName + "/DisplayField/TxtDisplayField").GetComponent<Text>();

		// setup number keypad
		numPad = new GameObject[popUpHeight, popUpWidth];
		numPad[0, 0] = GameObject.Find("/Canvas/" + menuPopUpName + "/Key1");
		numPad[0, 1] = GameObject.Find("/Canvas/" + menuPopUpName + "/Key2");
		numPad[0, 2] = GameObject.Find("/Canvas/" + menuPopUpName + "/Key3");
		numPad[1, 0] = GameObject.Find("/Canvas/" + menuPopUpName + "/Key4");
		numPad[1, 1] = GameObject.Find("/Canvas/" + menuPopUpName + "/Key5");
		numPad[1, 2] = GameObject.Find("/Canvas/" + menuPopUpName + "/Key6");
		numPad[2, 0] = GameObject.Find("/Canvas/" + menuPopUpName + "/Key7");
		numPad[2, 1] = GameObject.Find("/Canvas/" + menuPopUpName + "/Key8");
		numPad[2, 2] = GameObject.Find("/Canvas/" + menuPopUpName + "/Key9");
		numPad[3, 0] = popUp[3, 0];
		numPad[3, 1] = GameObject.Find("/Canvas/" + menuPopUpName + "/Key0");
		numPad[3, 2] = popUp[3, 2];
		numPad[4, 0] = numPad[4, 1] = numPad[4, 2] = popUp[4, 0];

		foreach (GameObject child in numPad) {
			if (child.name != "KeyDel" && child.name != "KeySwap" && child.name != "BtnSubmit") {
				child.SetActive (false);
			}
		}
		
		popUp[0, 0].GetComponent<Button>().onClick.AddListener(() =>
            KeyInput(new char[4] { '@', '.', '-', '_' })
		);

        popUp[0, 1].GetComponent<Button>().onClick.AddListener(() =>
            KeyInput(new char[3] { 'A', 'B', 'C' })
		);

        popUp[0, 2].GetComponent<Button>().onClick.AddListener(() =>
            KeyInput(new char[3] { 'D', 'E', 'F' })
		);

		popUp[1, 0].GetComponent<Button>().onClick.AddListener(() =>
		    KeyInput(new char[3] { 'G', 'H', 'I' })
		);

		popUp[1, 1].GetComponent<Button>().onClick.AddListener(() =>
		    KeyInput(new char[3] { 'J', 'K', 'L' })
		);

		popUp[1, 2].GetComponent<Button>().onClick.AddListener(() =>
		    KeyInput(new char[3] { 'M', 'N', 'O' })
		);

		popUp[2, 0].GetComponent<Button>().onClick.AddListener(() =>
		    KeyInput(new char[4] { 'P', 'Q', 'R', 'S' })
		);

		popUp[2, 1].GetComponent<Button>().onClick.AddListener(() =>
		    KeyInput(new char[3] { 'T', 'U', 'V' })
		);

		popUp[2, 2].GetComponent<Button>().onClick.AddListener(() =>
		    KeyInput(new char[4] { 'W', 'X', 'Y', 'Z' })
		);

		popUp[3, 0].GetComponent<Button>().onClick.AddListener(() =>
		    DeleteChar()
		);

		popUp[3, 1].GetComponent<Button>().onClick.AddListener(() =>
		    KeyInput(new char[1] { ' ' })
		);

		// swap button
		popUp[3, 2].GetComponent<Button>().onClick.AddListener(() => {
			if (currMenuPtr == popUp) {
				Debug.Log ("Currently on popUp");
			} else {

			}
		});

		// submit button
		popUp[4, 0].GetComponent<Button>().onClick.AddListener(() =>
		{
            PopUpDisable();
			KeypadSubmit();
		});

		// switch to start menu
		MenuSwitch (Menu.StartMenu);
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
            prevKey = ""; // reset keypad on move

            //Debug.Log(locX + "," + locY);
        }
    }

	// handles menu switching (ex: start menu transition to login form)
	void MenuSwitch (Menu menuToSwitchTo) {
		// hide current menu
        if (currAnim != null) {
			currAnim.SetBool("show", false);
            prevMenu = currMenu;
        }

		// switch to new menu
		switch (menuToSwitchTo) {
		case Menu.StartMenu:
			currMenuPtr = startMenu;
			currAnim = startMenuAnim;
			break;
		case Menu.LoginForm:
			currMenuPtr = loginForm;
			currAnim = loginFormAnim;
			break;
        case Menu.PopUp:
            currMenuPtr = popUp;
			txtDisplayField.text = currFieldPtr.text;
            currAnim = popUpAnim;
            break;
		default:
			Debug.Log ("Menu switch case invalid!");
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
		currAnim.SetBool("show", true);
	}

	void PopUpEnable() {
		MenuDisable ();
        MenuSwitch(Menu.PopUp);
	}

    void PopUpDisable() {
        MenuEnable();
        MenuSwitch(prevMenu);
    }

    void KeyInput(char[] chars)
    {
		if (txtDisplayField.text == "Enter an account name..." || txtDisplayField.text == "Enter your passcode...") {
			txtDisplayField.text = "";
			txtDisplayField.color = Color.white;
		}

        currKey = ConcatCharArray(chars);
        if (currKey != prevKey)
        {
            pressTime = Time.time;
            charArrLoc = 0;
            tmpCharName = txtDisplayField.text;
            txtDisplayField.text = tmpCharName + chars[charArrLoc];
        }
        else
        {
            if ((Time.time - pressTime) < 1.0)
            {
                txtDisplayField.text = tmpCharName + chars[charArrLoc];
                pressTime = Time.time;
            }
            else
            {
                pressTime = Time.time;
                charArrLoc = 0;
                tmpCharName = txtDisplayField.text;
                txtDisplayField.text = tmpCharName + chars[charArrLoc];
            }
        }

        ++charArrLoc;
        if (charArrLoc >= chars.Length)
            charArrLoc = 0;
        prevKey = currKey;
    }

	void KeypadSubmit() {
		if (txtDisplayField.text == "") {
			if (currFieldPtr == txtFieldAcctName) {
				txtDisplayField.text = "Enter an account name...";
			} else if (currFieldPtr == txtFieldPasscode) {
				txtDisplayField.text = "Enter your passcode...";
			}
		}
		currFieldPtr.text = txtDisplayField.text;
	}

    void MenuEnable() {
		// unlock controls
		//menuLock = false;

        // return color to buttons
        CanvasGroup groupContainer = GameObject.Find("/Canvas/" + menuContainerName).GetComponent<CanvasGroup>();
        groupContainer.interactable = true;

        // return color to image panel
        Image imgPanel = GameObject.Find("/Canvas/" + menuContainerName + "/Panel").GetComponent<Image>();
        imgPanel.color = new Color32(255, 255, 255, 100);

		// return color to text
		Text[] txtChild = this.GetComponentsInChildren<Text>();
		foreach (Text child in txtChild)
		{
			child.color = new Color32(152, 213, 217, 255);
		}

        // highlight first button of currMenuPtr
        locX = 0;
        locY = 0;
        var pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(currMenuPtr[locY, locX], pointer, ExecuteEvents.pointerEnterHandler);
    }

    void MenuDisable() {
		// lock controls
		//menuLock = true;

        // grey buttons
        CanvasGroup groupContainer = GameObject.Find("/Canvas/" + menuContainerName).GetComponent<CanvasGroup>();
        groupContainer.interactable = false;

        // grey image panel
        Image imgPanel = GameObject.Find("/Canvas/" + menuContainerName + "/Panel").GetComponent<Image>();
        imgPanel.color = new Color(0.3f, 0.3f, 0.3f);

        // grey text
        Text[] txtChild = this.GetComponentsInChildren<Text>();
        foreach (Text child in txtChild)
        {
            child.color = new Color(0.3f, 0.3f, 0.3f);
        }
    }

	void MenuReset() {
		txtFieldAcctName.text = "Enter an account name...";
		txtFieldPasscode.text = "Enter your passcode...";
	}

    string ConcatCharArray(char[] chars)
    {
        string retVal = "";
        foreach (char c in chars)
        {
            retVal += c;
        }
        return retVal;
    }

	void DeleteChar() {
		if (txtDisplayField.text.Length > 0)
		{
			txtDisplayField.text = txtDisplayField.text.Remove(txtDisplayField.text.Length - 1);
			charArrLoc = 0;
			prevKey = "";
		}
	}

	void Update () {
		// check for joystick movement
        MenuMove(Input.GetAxisRaw(controls.hori), Input.GetAxisRaw(controls.vert));

		// check for button press
        if (Input.GetButtonUp(controls.joyAttack) && menuLock == false)
        {
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(currMenuPtr[locY, locX], pointer, ExecuteEvents.submitHandler);
        }
        

        if (Input.GetButtonUp(controls.joySecItem) && currMenu == Menu.PopUp)
        {
			DeleteChar();
        }
	}
}
