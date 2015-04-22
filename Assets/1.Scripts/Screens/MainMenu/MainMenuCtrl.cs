using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuCtrl : MonoBehaviour {
    public Controls controls;
    public string menuContainerName;

    // UI state
    private int menuWidth = 1;
    private int menuHeight = 2;
    private bool menuMoved = false;
    private bool menuLock = false;
    private GameObject prevBtn;
    private int locX = 0;
    private int locY = 0;

    // menus
    private GameObject[,] startMenu;

    private enum Menu
    {
        Start,
        Register,
        Login,
        LevelSelect
    };

	void Start () {
        // create start menu
        startMenu = new GameObject[menuHeight, menuWidth];
        startMenu[0, 0] = GameObject.Find("/Canvas/" + menuContainerName + "/BtnLogin");
        startMenu[1, 0] = GameObject.Find("/Canvas/" + menuContainerName + "/BtnRegister");

        var pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(startMenu[locY, locX], pointer, ExecuteEvents.pointerEnterHandler); // highlight first button
        prevBtn = startMenu[locY, locX];

        // login button press handler
        startMenu[0, 0].GetComponent<Button>().onClick.AddListener(() => {
            GameObject.Find("/Canvas").GetComponent<Animator>().SetTrigger("fadeOut");
            GameObject.Find("/Main Camera").GetComponent<MainMenuCamera>().slideDown = true;
            menuLock = true;
        });

        // register button press handler
        startMenu[1, 0].GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("Register button pressed!");
        });
	}

    void MenuMove (float hori, float vert) {
        if (vert == 0 && hori == 0)
        {
            menuMoved = false;
        } else if (menuMoved == false) {
            menuMoved = true;

            if (vert < 0)
            {
                locY = (locY + 1) % (menuHeight);
            } else if (vert > 0) {
                --locY;
                if (locY < 0)
                {
                    locY = menuHeight - 1;
                }
            }

            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(prevBtn, pointer, ExecuteEvents.pointerExitHandler); // unhighlight previous button
            ExecuteEvents.Execute(startMenu[locY, locX], pointer, ExecuteEvents.pointerEnterHandler); //highlight current button

            prevBtn = startMenu[locY, locX];
        }
    }
	
	// Update is called once per frame
	void Update () {
        MenuMove(Input.GetAxisRaw(controls.hori), Input.GetAxisRaw(controls.vert));

        if (Input.GetButtonUp(controls.joyAttack) && menuLock == false)
        {
            var pointer = new PointerEventData(EventSystem.current);
            ExecuteEvents.Execute(startMenu[locY, locX], pointer, ExecuteEvents.submitHandler);
        }
	}
}
