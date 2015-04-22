using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuCtrl : MonoBehaviour {
    public Controls controls;
    public string menuContainerName;

    // UI state
    private int currMenuWidth;
    private int currMenuHeight;
    private bool menuMoved = false;
    private bool menuLock = false;
    private GameObject prevBtn;
    private int locX = 0;
    private int locY = 0;

    // menus
    private GameObject[,] startMenu;
    private int startMenuWidth = 1;
    private int startMenuHeight = 2;
    private GameObject[,] loginForm;
    private int loginFormWidth = 1;
    private int loginFormHeight = 2;

    private enum Menu
    {
        Start,
        Register,
        Login,
        LevelSelect
    };

	void Start () {
        // setup start menu
        startMenu = new GameObject[startMenuHeight, startMenuWidth];
        startMenu[0, 0] = GameObject.Find("/Canvas/" + menuContainerName + "/StartMenu/BtnLogin");
        startMenu[1, 0] = GameObject.Find("/Canvas/" + menuContainerName + "/StartMenu/BtnRegister");

        // setup login
        loginForm = new GameObject[startMenuHeight, startMenuWidth];
        loginForm[0, 0] = GameObject.Find("/Canvas/" + menuContainerName + "/StartMenu/BtnLogin");
        startMenu[1, 0] = GameObject.Find("/Canvas/" + menuContainerName + "/StartMenu/BtnRegister");

        // login button press handler
        startMenu[0, 0].GetComponent<Button>().onClick.AddListener(() =>
        {
            GameObject.Find("/Canvas/" + menuContainerName).GetComponent<Animator>().SetTrigger("startMenuFadeOut");
            //GameObject.Find("/Main Camera").GetComponent<MainMenuCamera>().slideDown = true;
            menuLock = true;
        });

        // register button press handler
        startMenu[1, 0].GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("Register button pressed!");
        });

        var pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(startMenu[locY, locX], pointer, ExecuteEvents.pointerEnterHandler); // highlight first button
        prevBtn = startMenu[locY, locX];
        currMenuWidth = startMenuWidth;
        currMenuHeight = startMenuHeight;
	}

    void MenuMove (float hori, float vert) {
        if (vert == 0 && hori == 0)
        {
            menuMoved = false;
        } else if (menuMoved == false) {
            menuMoved = true;

            if (vert < 0)
            {
                locY = (locY + 1) % (currMenuHeight);
            } else if (vert > 0) {
                --locY;
                if (locY < 0)
                {
                    locY = currMenuHeight - 1;
                }
            }

            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(prevBtn, pointer, ExecuteEvents.pointerExitHandler); // unhighlight previous button
            ExecuteEvents.Execute(startMenu[locY, locX], pointer, ExecuteEvents.pointerEnterHandler); //highlight current button
            prevBtn = startMenu[locY, locX];
        }
    }
	
	void Update () {
        MenuMove(Input.GetAxisRaw(controls.hori), Input.GetAxisRaw(controls.vert));

        if (Input.GetButtonUp(controls.joyAttack) && menuLock == false)
        {
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(startMenu[locY, locX], pointer, ExecuteEvents.submitHandler);
        }
	}
}
