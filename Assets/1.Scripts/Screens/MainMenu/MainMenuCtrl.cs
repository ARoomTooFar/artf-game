﻿using UnityEngine;
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
            //MenuSwitch(Menu.LoginForm);

            Farts serv = gameObject.AddComponent<Farts>();
            serv.login("Paradoxium", "pass");
        });

		// setup login
		loginForm = new GameObject[loginFormHeight, loginFormWidth];
		loginForm[0, 0] = loginForm[0, 1] = GameObject.Find("/Canvas/" + menuContainerName + "/LoginForm/FieldAcctName");
        loginForm[1, 0] = loginForm[1, 1] = GameObject.Find("/Canvas/" + menuContainerName + "/LoginForm/FieldPasscode");
        loginForm[2, 0] = GameObject.Find("/Canvas/" + menuContainerName + "/LoginForm/BtnLogin");
        loginForm[2, 1] = GameObject.Find("/Canvas/" + menuContainerName + "/LoginForm/BtnBack");
		loginFormAnim = GameObject.Find ("/Canvas/" + menuContainerName + "/LoginForm").GetComponent<Animator>();
		
		// back button
        loginForm[2, 1].GetComponent<Button>().onClick.AddListener(() =>
        {
			MenuSwitch (Menu.StartMenu);
        });

		// switch to start menu
		MenuSwitch (Menu.StartMenu);

        // test
        MenuDisable();
        //MenuEnable();
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

            if (hori > 0)
            {
                locX = (locX + 1) % (currMenu.GetLength(1));
            }
            else if (hori < 0)
            {
                --locX;
                if (locX < 0)
                {
                    locX = currMenu.GetLength(1) - 1;
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
        ExecuteEvents.Execute(prevBtn, pointer, ExecuteEvents.pointerExitHandler); // unhighlight previous button
        locY = 0;
        locX = 0;
		ExecuteEvents.Execute(currMenu[locY, locX], pointer, ExecuteEvents.pointerEnterHandler);
		prevBtn = currMenu[locY, locX];
		currAnim.SetBool("show", true);
	}

    void MenuEnable() {
        // return color to buttons
        CanvasGroup groupContainer = GameObject.Find("/Canvas/" + menuContainerName).GetComponent<CanvasGroup>();
        groupContainer.interactable = true;

        // return color to image panel
        Image imgPanel = GameObject.Find("/Canvas/" + menuContainerName + "/Panel").GetComponent<Image>();
        imgPanel.color = new Color32(255, 255, 255, 100);

        // return text color in buttons
        BtnScript[] btnChild = this.GetComponentsInChildren<BtnScript>();
        foreach (BtnScript child in btnChild)
        {
            child.DehighlightTxt();
        }

        // highlight first button of currMenu
        locX = 0;
        locY = 0;
        var pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(currMenu[locY, locX], pointer, ExecuteEvents.pointerEnterHandler);

        // unlock controls
        menuLock = false;
    }

    void MenuDisable() {
        // grey buttons
        CanvasGroup groupContainer = GameObject.Find("/Canvas/" + menuContainerName).GetComponent<CanvasGroup>();
        groupContainer.interactable = false;

        // grey image panel
        Image imgPanel = GameObject.Find("/Canvas/" + menuContainerName + "/Panel").GetComponent<Image>();
        imgPanel.color = new Color(0.3f, 0.3f, 0.3f);

        // grey text in buttons
        /*BtnScript[] btnChild = this.GetComponentsInChildren<BtnScript>();
        foreach (BtnScript child in btnChild)
        {
            child.DisableTxt();
        }*/

        // grey text
        Text[] txtChild = this.GetComponentsInChildren<Text>();
        foreach (Text child in txtChild)
        {
            child.color = new Color(0.3f, 0.3f, 0.3f);
        }

        // lock controls
        menuLock = true;
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
