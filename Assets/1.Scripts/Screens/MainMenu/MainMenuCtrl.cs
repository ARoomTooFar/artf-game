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
    private int locX = 0;
    private int locY = 0;

    // start menu
    private GameObject[,] startMenu;
    private int startMenuWidth = 1;
    private int startMenuHeight = 2;
	private Animator startMenuObj;

	// login form
    private GameObject[,] loginForm;
    private int loginFormWidth = 1;
    private int loginFormHeight = 2;
	private Animator loginFormObj;

	void Start () {
        // setup start menu
        startMenu = new GameObject[startMenuHeight, startMenuWidth];
        startMenu[0, 0] = GameObject.Find("/Canvas/" + menuContainerName + "/StartMenu/BtnLogin");
        startMenu[1, 0] = GameObject.Find("/Canvas/" + menuContainerName + "/StartMenu/BtnRegister");
		startMenuObj = GameObject.Find ("/Canvas/" + menuContainerName + "/StartMenu").GetComponent<Animator>();

        // setup login
        loginForm = new GameObject[startMenuHeight, startMenuWidth];
        loginForm[0, 0] = GameObject.Find("/Canvas/" + menuContainerName + "/LoginForm/BtnPrefab");
		loginFormObj = GameObject.Find ("/Canvas/" + menuContainerName + "/LoginForm").GetComponent<Animator>();
		
		// login button press handler
        startMenu[0, 0].GetComponent<Button>().onClick.AddListener(() =>
        {
			startMenuObj.GetComponent<Animator>().SetBool("startMenuBool", false);
			loginFormObj.GetComponent<Animator>().SetBool("loginFormBool", true);
            //GameObject.Find("/Main Camera").GetComponent<MainMenuCamera>().slideDown = true;
            //menuLock = true;
        });

        // register button press handler
        startMenu[1, 0].GetComponent<Button>().onClick.AddListener(() =>
        {
			startMenuObj.GetComponent<Animator>().SetBool("startMenuBool", false);
        });

		// switch to start menu
		currMenu = startMenu;
		GameObject.Find("/Canvas/" + menuContainerName + "/StartMenu").GetComponent<Animator>().SetBool("startMenuBool", true);
        var pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(currMenu[locY, locX], pointer, ExecuteEvents.pointerEnterHandler); // highlight first button
        prevBtn = currMenu[locY, locX];
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
            ExecuteEvents.Execute(startMenu[locY, locX], pointer, ExecuteEvents.pointerEnterHandler); //highlight current button
            prevBtn = currMenu[locY, locX];
        }
    }

	// handles menu switching (ex: start menu transition to login form)
	void MenuSwitch () {

	}
	
	void Update () {
		// check for joystick movement
        MenuMove(Input.GetAxisRaw(controls.hori), Input.GetAxisRaw(controls.vert));

		// check for button press
        if (Input.GetButtonUp(controls.joyAttack) && menuLock == false)
        {
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(startMenu[locY, locX], pointer, ExecuteEvents.submitHandler);
        }
	}
}
