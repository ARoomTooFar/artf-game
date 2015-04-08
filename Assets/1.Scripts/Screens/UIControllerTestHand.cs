using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIControllerTestHand : MonoBehaviour {
    public Controls controls;

    private Button[,] menu;
    private int menuVertLoc = 0;
    private int menuHoriLoc = 0;
    private bool menuMoved = false;

	// Use this for initialization
	void Start () {
        menu = new Button[2, 2];

        // top row
        menu[0, 0] = GameObject.Find("Btn1").GetComponent<Button>();
        menu[0, 1] = GameObject.Find("Btn3").GetComponent<Button>();

        // bottom row
        menu[1, 0] = GameObject.Find("Btn2").GetComponent<Button>();
        menu[1, 1] = GameObject.Find("Btn4").GetComponent<Button>();

        menu[0, 0].onClick.AddListener(() =>
        {
            Debug.Log("Btn1 pressed");
        }
        );

        menu[0, 1].onClick.AddListener(() =>
        {
            Debug.Log("Btn3 pressed");
        }
        );

        menu[1, 0].onClick.AddListener(() =>
        {
            Debug.Log("Btn2 pressed");
        }
        );

        menu[1, 1].onClick.AddListener(() =>
        {
            Debug.Log("Btn4 pressed");
        }
        );

        menu[0, 0].Select();
	}

    void stickUp () {
        if (menuMoved == false)
        {
            menuMoved = true;
            ++menuVertLoc;
            if (menuVertLoc > 1)
                menuVertLoc = 0;
            Debug.Log(menuVertLoc);
        }
    }

    void stickDown()
    {
        if (menuMoved == false)
        {
            menuMoved = true;
            --menuVertLoc;
            if (menuVertLoc < 0)
                menuVertLoc = 1;
            Debug.Log(menuVertLoc);
        }
    }

    void stickRight()
    {
        if (menuMoved == false)
        {
            menuMoved = true;
            ++menuHoriLoc;
            if (menuHoriLoc > 1)
                menuHoriLoc = 0;
            Debug.Log(menuHoriLoc);
        }
    }

    void stickLeft()
    {
        if (menuMoved == false)
        {
            menuMoved = true;
            --menuHoriLoc;
            if (menuHoriLoc < 0)
                menuHoriLoc = 1;
            Debug.Log(menuHoriLoc);
        }
    }

    void stickNeutral()
    {
        menuMoved = false;
        //Debug.Log("nair");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxisRaw(controls.vert) > 0) {
            stickUp();
            menu[menuVertLoc, menuHoriLoc].Select();
            
        }
        else if (Input.GetAxisRaw(controls.vert) < 0) {
            stickDown();
            menu[menuVertLoc, menuHoriLoc].Select();
        }

        if (Input.GetAxisRaw(controls.hori) > 0)
        {
            stickRight();
            menu[menuVertLoc, menuHoriLoc].Select();

        }
        else if (Input.GetAxisRaw(controls.hori) < 0)
        {
            stickLeft();
            menu[menuVertLoc, menuHoriLoc].Select();
        }

        if (Input.GetAxisRaw(controls.vert) == 0 && Input.GetAxisRaw(controls.hori) == 0)
        {
            stickNeutral();
        }
	}
}
