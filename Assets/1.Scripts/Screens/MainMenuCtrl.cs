using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuCtrl : MonoBehaviour {
    public Controls controls;

    // UI state
    private int menuWidth = 1;
    private int menuHeight = 2;
    private bool menuMoved = false;

    // player1 menu
    private GameObject[,] p1Menu;
    private int p1LocX = 0;
    private int p1LocY = 0;

    private enum Menu
    {
        Start,
        Register,
        Login,
        LevelSelect
    };



	void Start () {
        // create p1 Start menu
        p1Menu = new GameObject[menuHeight, menuWidth];

        p1Menu[0, 0] = GameObject.Find("/Canvas/BtnLogin");
        p1Menu[1, 0] = GameObject.Find("/Canvas/BtnRegister");

        p1Menu[0, 0].GetComponent<Button>().Select();

        //menu[0, 0].onClick.AddListener(() => keyboardbuttons(new char[1] { '-' }));
	}

    void MenuMove (float hori, float vert) {
        if (vert == 0 && hori == 0)
        {
            menuMoved = false;
        } else if (menuMoved == false) {
            menuMoved = true;

            if (vert < 0)
            {
                p1LocY = (p1LocY + 1) % (menuHeight);
            } else if (vert > 0) {
                --p1LocY;
                if (p1LocY < 0)
                {
                    p1LocY = menuHeight - 1;
                }
            }

            p1Menu[p1LocY, p1LocX].GetComponent<Button>().Select();
            Debug.Log(p1LocY);
        }
    }
	
	// Update is called once per frame
	void Update () {
        MenuMove(Input.GetAxisRaw(controls.hori), Input.GetAxisRaw(controls.vert));
	}
}
