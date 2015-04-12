using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIControllerTestHand : MonoBehaviour {
    public Controls controls;

    private InputField fieldCharName;
    private string tmpCharName;
    private float pressTime;
    private string currBtn = "";
    private string prevBtn = "";

    private GameObject[,] menu;
    private GameObject[,] menu2;
    private int menuVertLoc = 0;
    private int menuHoriLoc = 0;
    private bool menuMoved = false;
    private int vertSize = 4;
    private int horiSize = 3;
    private int arrLoc = 0;
    private int currPad = 1;

	// Use this for initialization
	void Start () {
        fieldCharName = GameObject.Find("FieldCharName").GetComponent<InputField>();

        //caps pad
        menu = new GameObject[vertSize, horiSize];

        // row 1
        menu[0, 0] = GameObject.Find("BtnDash");
        menu[0, 1] = GameObject.Find("BtnABC");
        menu[0, 2] = GameObject.Find("BtnDEF");

        menu[0, 0].GetComponent<Button>().onClick.AddListener(() =>
        {
            currBtn = "-";

            fieldCharName.text = fieldCharName.text + "-";
            tmpCharName = fieldCharName.text;

            prevBtn = "-";
        }
        );

        menu[0, 1].GetComponent<Button>().onClick.AddListener(() =>
        {
            char[] chars = new char[3];
            chars[0] = 'A';
            chars[1] = 'B';
            chars[2] = 'C';

            currBtn = "ABC";

            if (currBtn != prevBtn)
            {
                pressTime = Time.time;
                arrLoc = 0;
                tmpCharName = fieldCharName.text;
                fieldCharName.text = tmpCharName + chars[arrLoc];
            }
            else
            {
                if ((Time.time - pressTime) < 3.0)
                {
                    fieldCharName.text = tmpCharName + chars[arrLoc];
                    pressTime = Time.time;
                }
                else
                {
                    pressTime = Time.time;
                    arrLoc = 0;
                    tmpCharName = fieldCharName.text;
                    fieldCharName.text = tmpCharName + chars[arrLoc];
                }
            }

            ++arrLoc;
            if (arrLoc >= 3)
                arrLoc = 0;

            prevBtn = "ABC";
        }
        );

        menu[0, 2].GetComponent<Button>().onClick.AddListener(() =>
        {
            char[] chars = new char[3];
            chars[0] = 'D';
            chars[1] = 'E';
            chars[2] = 'F';

            currBtn = "DEF";

            if (currBtn != prevBtn)
            {
                pressTime = Time.time;
                arrLoc = 0;
                tmpCharName = fieldCharName.text;
                fieldCharName.text = tmpCharName + chars[arrLoc];
            }
            else
            {
                if ((Time.time - pressTime) < 3.0)
                {
                    fieldCharName.text = tmpCharName + chars[arrLoc];
                    pressTime = Time.time;
                }
                else
                {
                    pressTime = Time.time;
                    arrLoc = 0;
                    tmpCharName = fieldCharName.text;
                    fieldCharName.text = tmpCharName + chars[arrLoc];
                }
            }

            ++arrLoc;
            if (arrLoc >= 3)
                arrLoc = 0;

            prevBtn = "DEF";
        }
        );

        // row 2
        menu[1, 0] = GameObject.Find("BtnGHI");
        menu[1, 1] = GameObject.Find("BtnJKL");
        menu[1, 2] = GameObject.Find("BtnMNO");

        menu[1, 0].GetComponent<Button>().onClick.AddListener(() =>
        {
            char[] chars = new char[3];
            chars[0] = 'G';
            chars[1] = 'H';
            chars[2] = 'I';

            currBtn = "GHI";

            if (currBtn != prevBtn)
            {
                pressTime = Time.time;
                arrLoc = 0;
                tmpCharName = fieldCharName.text;
                fieldCharName.text = tmpCharName + chars[arrLoc];
            }
            else
            {
                if ((Time.time - pressTime) < 3.0)
                {
                    fieldCharName.text = tmpCharName + chars[arrLoc];
                    pressTime = Time.time;
                }
                else
                {
                    pressTime = Time.time;
                    arrLoc = 0;
                    tmpCharName = fieldCharName.text;
                    fieldCharName.text = tmpCharName + chars[arrLoc];
                }
            }

            ++arrLoc;
            if (arrLoc >= 3)
                arrLoc = 0;

            prevBtn = "GHI";
        }
        );

        menu[1, 1].GetComponent<Button>().onClick.AddListener(() =>
        {
            char[] chars = new char[3];
            chars[0] = 'J';
            chars[1] = 'K';
            chars[2] = 'L';

            currBtn = "JKL";

            if (currBtn != prevBtn)
            {
                pressTime = Time.time;
                arrLoc = 0;
                tmpCharName = fieldCharName.text;
                fieldCharName.text = tmpCharName + chars[arrLoc];
            }
            else
            {
                if ((Time.time - pressTime) < 3.0)
                {
                    fieldCharName.text = tmpCharName + chars[arrLoc];
                    pressTime = Time.time;
                }
                else
                {
                    pressTime = Time.time;
                    arrLoc = 0;
                    tmpCharName = fieldCharName.text;
                    fieldCharName.text = tmpCharName + chars[arrLoc];
                }
            }

            ++arrLoc;
            if (arrLoc >= 3)
                arrLoc = 0;

            prevBtn = "JKL";
        }
        );

        menu[1, 2].GetComponent<Button>().onClick.AddListener(() =>
        {
            char[] chars = new char[3];
            chars[0] = 'M';
            chars[1] = 'N';
            chars[2] = 'O';

            currBtn = "MNO";

            if (currBtn != prevBtn)
            {
                pressTime = Time.time;
                arrLoc = 0;
                tmpCharName = fieldCharName.text;
                fieldCharName.text = tmpCharName + chars[arrLoc];
            }
            else
            {
                if ((Time.time - pressTime) < 3.0)
                {
                    fieldCharName.text = tmpCharName + chars[arrLoc];
                    pressTime = Time.time;
                }
                else
                {
                    pressTime = Time.time;
                    arrLoc = 0;
                    tmpCharName = fieldCharName.text;
                    fieldCharName.text = tmpCharName + chars[arrLoc];
                }
            }

            ++arrLoc;
            if (arrLoc >= 3)
                arrLoc = 0;

            prevBtn = "MNO";
        }
        );

        // row 3
        menu[2, 0] = GameObject.Find("BtnPQRS");
        menu[2, 1] = GameObject.Find("BtnTUV");
        menu[2, 2] = GameObject.Find("BtnWXYZ");

        menu[2, 0].GetComponent<Button>().onClick.AddListener(() =>
        {
            char[] chars = new char[4];
            chars[0] = 'P';
            chars[1] = 'Q';
            chars[2] = 'R';
            chars[3] = 'S';

            currBtn = "PQRS";

            if (currBtn != prevBtn)
            {
                pressTime = Time.time;
                arrLoc = 0;
                tmpCharName = fieldCharName.text;
                fieldCharName.text = tmpCharName + chars[arrLoc];
            }
            else
            {
                if ((Time.time - pressTime) < 3.0)
                {
                    fieldCharName.text = tmpCharName + chars[arrLoc];
                    pressTime = Time.time;
                }
                else
                {
                    pressTime = Time.time;
                    arrLoc = 0;
                    tmpCharName = fieldCharName.text;
                    fieldCharName.text = tmpCharName + chars[arrLoc];
                }
            }

            ++arrLoc;
            if (arrLoc >= 4)
                arrLoc = 0;

            prevBtn = "PQRS";
        }
        );

        menu[2, 1].GetComponent<Button>().onClick.AddListener(() =>
        {
            char[] chars = new char[3];
            chars[0] = 'T';
            chars[1] = 'U';
            chars[2] = 'V';

            currBtn = "TUV";

            if (currBtn != prevBtn)
            {
                pressTime = Time.time;
                arrLoc = 0;
                tmpCharName = fieldCharName.text;
                fieldCharName.text = tmpCharName + chars[arrLoc];
            }
            else
            {
                if ((Time.time - pressTime) < 3.0)
                {
                    fieldCharName.text = tmpCharName + chars[arrLoc];
                    pressTime = Time.time;
                }
                else
                {
                    pressTime = Time.time;
                    arrLoc = 0;
                    tmpCharName = fieldCharName.text;
                    fieldCharName.text = tmpCharName + chars[arrLoc];
                }
            }

            ++arrLoc;
            if (arrLoc >= 3)
                arrLoc = 0;

            prevBtn = "TUV";
        }
        );

        menu[2, 2].GetComponent<Button>().onClick.AddListener(() =>
        {
            char[] chars = new char[4];
            chars[0] = 'W';
            chars[1] = 'X';
            chars[2] = 'Y';
            chars[3] = 'Z';

            currBtn = "WXYZ";

            if (currBtn != prevBtn)
            {
                pressTime = Time.time;
                arrLoc = 0;
                tmpCharName = fieldCharName.text;
                fieldCharName.text = tmpCharName + chars[arrLoc];
            }
            else
            {
                if ((Time.time - pressTime) < 3.0)
                {
                    fieldCharName.text = tmpCharName + chars[arrLoc];
                    pressTime = Time.time;
                }
                else
                {
                    pressTime = Time.time;
                    arrLoc = 0;
                    tmpCharName = fieldCharName.text;
                    fieldCharName.text = tmpCharName + chars[arrLoc];
                }
            }

            ++arrLoc;
            if (arrLoc >= 4)
                arrLoc = 0;

            prevBtn = "WXYZ";
        }
        );

        // row 4
        menu[3, 0] = GameObject.Find("BtnSpace1");
        menu[3, 1] = GameObject.Find("BtnUnderscore");
        menu[3, 2] = GameObject.Find("BtnSwap1");

        menu[3, 0].GetComponent<Button>().onClick.AddListener(() =>
        {
            currBtn = " ";

            fieldCharName.text = fieldCharName.text + " ";
            tmpCharName = fieldCharName.text;

            prevBtn = " ";
        }
        );

        menu[3, 1].GetComponent<Button>().onClick.AddListener(() =>
        {
            currBtn = "_";

            fieldCharName.text = fieldCharName.text + "_";
            tmpCharName = fieldCharName.text;

            prevBtn = "_";
        }
        );

        menu[3, 2].GetComponent<Button>().onClick.AddListener(() =>
        {
            currBtn = "Swap";

            showPad(3);
            menu2[3, 2].GetComponent<Button>().Select();

            prevBtn = "Swap";
        }
        );


        //lower pad
        menu2 = new GameObject[vertSize, horiSize];

        // row 1
        menu2[0, 0] = GameObject.Find("BtnOne");
        menu2[0, 1] = GameObject.Find("BtnTwo");
        menu2[0, 2] = GameObject.Find("BtnThree");

        // row 2
        menu2[1, 0] = GameObject.Find("BtnFour");
        menu2[1, 1] = GameObject.Find("BtnFive");
        menu2[1, 2] = GameObject.Find("BtnSix");

        // row 3
        menu2[2, 0] = GameObject.Find("BtnSeven");
        menu2[2, 1] = GameObject.Find("BtnEight");
        menu2[2, 2] = GameObject.Find("BtnNine");

        // row 4
        menu2[3, 0] = GameObject.Find("BtnSpace2");
        menu2[3, 1] = GameObject.Find("BtnZero");
        menu2[3, 2] = GameObject.Find("BtnSwap2");


        showPad(1);
        menu[0, 0].GetComponent<Button>().Select();
	}

    void stickUp ()
    {
        if (menuMoved == false)
        {
            menuMoved = true;
            --menuVertLoc;
            if (menuVertLoc < 0)
                menuVertLoc = vertSize - 1;
            prevBtn = "";
            arrLoc = 0;
            //Debug.Log(menuVertLoc);
        }
    }

    void stickDown()
    {
        if (menuMoved == false)
        {
            menuMoved = true;
            ++menuVertLoc;
            if (menuVertLoc > (vertSize - 1))
                menuVertLoc = 0;
            prevBtn = "";
            arrLoc = 0;
            //Debug.Log(menuVertLoc);
        }
    }

    void stickRight()
    {
        if (menuMoved == false)
        {
            menuMoved = true;
            ++menuHoriLoc;
            if (menuHoriLoc > (horiSize - 1))
                menuHoriLoc = 0;
            prevBtn = "";
            arrLoc = 0;
            //Debug.Log(menuHoriLoc);
        }
    }

    void stickLeft()
    {
        if (menuMoved == false)
        {
            menuMoved = true;
            --menuHoriLoc;
            if (menuHoriLoc < 0)
                menuHoriLoc = horiSize - 1;
            prevBtn = "";
            arrLoc = 0;
            //Debug.Log(menuHoriLoc);
        }
    }

    void stickNeutral()
    {
        menuMoved = false;
        //Debug.Log("nair");
    }

    void showPad(int padType)
    {
        switch (padType)
        {
            case 1:
                // caps pad
                foreach (GameObject btn in menu) {
                    btn.SetActive(true);
                }

                foreach (GameObject btn in menu2)
                {
                    btn.SetActive(false);
                }

                currPad = 1;
                break;
            case 2:
                Debug.Log("yeah");
                break;
            case 3:
                // num pad
                foreach (GameObject btn in menu2)
                {
                    btn.SetActive(true);
                }

                foreach (GameObject btn in menu)
                {
                    btn.SetActive(false);
                }

                currPad = 3;
                break;
            default:
                Debug.Log("padType is invalid.");
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(Input.GetAxisRaw(controls.vert));
        //Debug.Log(Time.time);

        if (Input.GetAxisRaw(controls.vert) > 0) {
            stickUp();
            switch (currPad) {
                case 1:
                    menu[menuVertLoc, menuHoriLoc].GetComponent<Button>().Select();
                    break;
                case 2:
                    Debug.Log("yeah");
                    break;
                case 3:
                    // num pad
                    menu2[menuVertLoc, menuHoriLoc].GetComponent<Button>().Select();
                    break;
                default:
                    Debug.Log("currPad is invalid.");
                    break;
            }
        }
        else if (Input.GetAxisRaw(controls.vert) < 0) {
            stickDown();
            switch (currPad)
            {
                case 1:
                    menu[menuVertLoc, menuHoriLoc].GetComponent<Button>().Select();
                    break;
                case 2:
                    Debug.Log("yeah");
                    break;
                case 3:
                    // num pad
                    menu2[menuVertLoc, menuHoriLoc].GetComponent<Button>().Select();
                    break;
                default:
                    Debug.Log("currPad is invalid.");
                    break;
            }
        }

        if (Input.GetAxisRaw(controls.hori) > 0)
        {
            stickRight();
            switch (currPad)
            {
                case 1:
                    menu[menuVertLoc, menuHoriLoc].GetComponent<Button>().Select();
                    break;
                case 2:
                    Debug.Log("yeah");
                    break;
                case 3:
                    // num pad
                    menu2[menuVertLoc, menuHoriLoc].GetComponent<Button>().Select();
                    break;
                default:
                    Debug.Log("currPad is invalid.");
                    break;
            }
        }
        else if (Input.GetAxisRaw(controls.hori) < 0)
        {
            stickLeft();
            switch (currPad)
            {
                case 1:
                    menu[menuVertLoc, menuHoriLoc].GetComponent<Button>().Select();
                    break;
                case 2:
                    Debug.Log("yeah");
                    break;
                case 3:
                    // num pad
                    menu2[menuVertLoc, menuHoriLoc].GetComponent<Button>().Select();
                    break;
                default:
                    Debug.Log("currPad is invalid.");
                    break;
            }
        }

        // prevent inputting stick controls every frame by requiring player to set stick to neutral to move again
        if (Input.GetAxisRaw(controls.vert) == 0 && Input.GetAxisRaw(controls.hori) == 0)
        {
            stickNeutral();
        }

        if (Input.GetButtonUp(controls.joySecItem))
        {
            if (fieldCharName.text.Length > 0)
            {
                fieldCharName.text = fieldCharName.text.Remove(fieldCharName.text.Length - 1);
                arrLoc = 0;
                prevBtn = "";
            }
        }
	}
}
