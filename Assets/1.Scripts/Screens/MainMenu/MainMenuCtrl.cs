using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuCtrl : MonoBehaviour {
    public Controls controls;
    public string menuContainerName;

    // UI state
    private bool menuMoved = false;
    private bool menuLock = false;
    private GameObject prevBtn;
	private GameObject[,] currMenu;
	private Animator currAnim;
    private int locX = 0;
    private int locY = 0;
	private enum Menu {
		StartMenu,
		LoginForm
	}

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

	void Start () {
        // setup start menu
        startMenu = new GameObject[startMenuHeight, startMenuWidth];
        startMenu[0, 0] = GameObject.Find("/Canvas/" + menuContainerName + "/StartMenu/BtnLogin");
        startMenu[1, 0] = GameObject.Find("/Canvas/" + menuContainerName + "/StartMenu/BtnRegister");
		startMenuAnim = GameObject.Find ("/Canvas/" + menuContainerName + "/StartMenu").GetComponent<Animator>();
		
		// login button press handler
        startMenu[0, 0].GetComponent<Button>().onClick.AddListener(() =>
        {
			MenuSwitch (Menu.LoginForm);
            //GameObject.Find("/Main Camera").GetComponent<MainMenuCamera>().slideDown = true;
            //menuLock = true;
        });

        // register button press handler
        startMenu[1, 0].GetComponent<Button>().onClick.AddListener(() =>
        {
			Debug.Log ("Register button pressed!");
        });

		// setup login
		loginForm = new GameObject[loginFormHeight, loginFormWidth];
		loginForm[0, 0] = loginForm[0, 1] = GameObject.Find("/Canvas/" + menuContainerName + "/LoginForm/FieldAcctName");
        loginForm[1, 0] = loginForm[1, 1] = GameObject.Find("/Canvas/" + menuContainerName + "/LoginForm/FieldPasscode");
        loginForm[2, 0] = GameObject.Find("/Canvas/" + menuContainerName + "/LoginForm/BtnLogin");
        loginForm[2, 1] = GameObject.Find("/Canvas/" + menuContainerName + "/LoginForm/BtnBack");
		loginFormAnim = GameObject.Find ("/Canvas/" + menuContainerName + "/LoginForm").GetComponent<Animator>();
		
		// test button press handler
        /*loginForm[0, 0].GetComponent<Button>().onClick.AddListener(() =>
        {
			Debug.Log ("Test button pressed!");
			MenuSwitch (Menu.StartMenu);
        });*/

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
                locY = (locY + 1) % (currMenu.Length);
            } else if (vert > 0) {
                --locY;
                if (locY < 0)
                {
					locY = currMenu.Length - 1;
                }
            }

            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(prevBtn, pointer, ExecuteEvents.pointerExitHandler); // unhighlight previous button
            ExecuteEvents.Execute(currMenu[locY, locX], pointer, ExecuteEvents.pointerEnterHandler); //highlight current button
            prevBtn = currMenu[locY, locX];
        }
    }

	// handles menu switching (ex: start menu transition to login form)
	void MenuSwitch (Menu menuToSwitchTo) {
		// hide current menu
		if(currAnim != null)
			currAnim.SetBool("show", false);

		// switch to new menu
		switch (menuToSwitchTo) {
		case Menu.StartMenu:
			currMenu = startMenu;
			currAnim = startMenuAnim;
			break;
		case Menu.LoginForm:
			currMenu = loginForm;
			currAnim = loginFormAnim;
			break;
		default:
			Debug.Log ("Menu switch case invalid!");
			break;
		}

		// setup first button highlight and show new menu
		var pointer = new PointerEventData(EventSystem.current);
		ExecuteEvents.Execute(currMenu[locY, locX], pointer, ExecuteEvents.pointerEnterHandler);
		prevBtn = currMenu[locY, locX];
		currAnim.SetBool("show", true);
	}
	
	void Update () {
		// check for joystick movement
        MenuMove(Input.GetAxisRaw(controls.hori), Input.GetAxisRaw(controls.vert));

		// check for button press
        if (Input.GetButtonUp(controls.joyAttack) && menuLock == false)
        {
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(currMenu[locY, locX], pointer, ExecuteEvents.submitHandler);
        }
	}
}
