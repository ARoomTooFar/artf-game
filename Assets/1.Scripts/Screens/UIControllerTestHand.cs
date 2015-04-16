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
	private Button[,] menu;
	private Button[,] menu2;
	private Button[,] currentMenu;
	private int menuVertLoc = 0;
	private int menuHoriLoc = 0;
	private bool menuMoved = false;
	private int vertSize = 4;
	private int horiSize = 3;
	private int arrLoc = 0;
	//private int currPad = 1;

	string concatCharArray(char[] chars) {
		string retVal = "";
		foreach(char c in chars) {
			retVal += c;
		}
		return retVal;
	}

	void keyboardbuttons(char[] chars) {
		currBtn = concatCharArray(chars);
		if(currBtn != prevBtn) {
			pressTime = Time.time;
			arrLoc = 0;
			tmpCharName = fieldCharName.text;
			fieldCharName.text = tmpCharName + chars[arrLoc];
		} else {
			if((Time.time - pressTime) < 1.0) {
				fieldCharName.text = tmpCharName + chars[arrLoc];
				pressTime = Time.time;
			} else {
				pressTime = Time.time;
				arrLoc = 0;
				tmpCharName = fieldCharName.text;
				fieldCharName.text = tmpCharName + chars[arrLoc];
			}
		}
		
		++arrLoc;
		if(arrLoc >= chars.Length)
			arrLoc = 0;
		prevBtn = currBtn;
	}



	// Use this for initialization
	void Start() {
		fieldCharName = GameObject.Find("FieldCharName").GetComponent<InputField>();

		//caps pad
		menu = new Button[vertSize, horiSize];

		// row 1
		menu[0, 0] = GameObject.Find("BtnDash").GetComponent<Button>();
		menu[0, 1] = GameObject.Find("BtnABC").GetComponent<Button>();
		menu[0, 2] = GameObject.Find("BtnDEF").GetComponent<Button>();

		menu[0, 0].onClick.AddListener(() => keyboardbuttons(new char[1] { '-' }));
		menu[0, 1].onClick.AddListener(() => keyboardbuttons(new char[3] { 'A', 'B', 'C' }));
		menu[0, 2].onClick.AddListener(() => keyboardbuttons(new char[3] { 'D', 'E', 'F' }));

		// row 2
		menu[1, 0] = GameObject.Find("BtnGHI").GetComponent<Button>();
		menu[1, 1] = GameObject.Find("BtnJKL").GetComponent<Button>();
		menu[1, 2] = GameObject.Find("BtnMNO").GetComponent<Button>();

		menu[1, 0].onClick.AddListener(() => keyboardbuttons(new char[3] { 'G', 'H', 'I' }));
		menu[1, 1].onClick.AddListener(() => keyboardbuttons(new char[3] { 'J', 'K', 'L' }));
		menu[1, 2].onClick.AddListener(() => keyboardbuttons(new char[3] { 'M', 'N', 'O' }));

		// row 3
		menu[2, 0] = GameObject.Find("BtnPQRS").GetComponent<Button>();
		menu[2, 1] = GameObject.Find("BtnTUV").GetComponent<Button>();
		menu[2, 2] = GameObject.Find("BtnWXYZ").GetComponent<Button>();

		menu[2, 0].onClick.AddListener(() => keyboardbuttons(new char[4] { 'P', 'Q', 'R', 'S' }));
		menu[2, 1].onClick.AddListener(() => keyboardbuttons(new char[3] { 'T', 'U', 'V' }));
		menu[2, 2].onClick.AddListener(() => keyboardbuttons(new char[4] { 'W', 'X', 'Y', 'Z' }));

		// row 4
		menu[3, 0] = GameObject.Find("BtnSpace1").GetComponent<Button>();
		menu[3, 1] = GameObject.Find("BtnUnderscore").GetComponent<Button>();
		menu[3, 2] = GameObject.Find("BtnSwap1").GetComponent<Button>();

		menu[3, 0].onClick.AddListener(() => keyboardbuttons(new char[1] { ' ' }));
		menu[3, 1].onClick.AddListener(() => keyboardbuttons(new char[1] { '_' }));
		menu[3, 2].onClick.AddListener(() =>
		{
			currBtn = "Swap";

			showPad(3);
			menu2[3, 2].GetComponent<Button>().Select();

			prevBtn = "Swap";
		}
		);


		//lower pad
		menu2 = new Button[vertSize, horiSize];

		// row 1
		menu2[0, 0] = GameObject.Find("BtnOne").GetComponent<Button>();
		menu2[0, 1] = GameObject.Find("BtnTwo").GetComponent<Button>();
		menu2[0, 2] = GameObject.Find("BtnThree").GetComponent<Button>();

		menu2[0, 0].onClick.AddListener(() => keyboardbuttons(new char[1] { '1' }));
		menu2[0, 1].onClick.AddListener(() => keyboardbuttons(new char[1] { '2' }));
		menu2[0, 2].onClick.AddListener(() => keyboardbuttons(new char[1] { '3' }));

		// row 2
		menu2[1, 0] = GameObject.Find("BtnFour").GetComponent<Button>();
		menu2[1, 1] = GameObject.Find("BtnFive").GetComponent<Button>();
		menu2[1, 2] = GameObject.Find("BtnSix").GetComponent<Button>();

		menu2[1, 0].onClick.AddListener(() => keyboardbuttons(new char[1] { '4' }));
		menu2[1, 1].onClick.AddListener(() => keyboardbuttons(new char[1] { '5' }));
		menu2[1, 2].onClick.AddListener(() => keyboardbuttons(new char[1] { '6' }));

		// row 3
		menu2[2, 0] = GameObject.Find("BtnSeven").GetComponent<Button>();
		menu2[2, 1] = GameObject.Find("BtnEight").GetComponent<Button>();
		menu2[2, 2] = GameObject.Find("BtnNine").GetComponent<Button>();

		menu2[2, 0].onClick.AddListener(() => keyboardbuttons(new char[1] { '7' }));
		menu2[2, 1].onClick.AddListener(() => keyboardbuttons(new char[1] { '8' }));
		menu2[2, 2].onClick.AddListener(() => keyboardbuttons(new char[1] { '9' }));
		
		// row 4
		menu2[3, 0] = GameObject.Find("BtnSpace2").GetComponent<Button>();
		menu2[3, 1] = GameObject.Find("BtnZero").GetComponent<Button>();
		menu2[3, 2] = GameObject.Find("BtnSwap2").GetComponent<Button>();

		menu2[3, 0].onClick.AddListener(() => keyboardbuttons(new char[1] { ' ' }));
		menu2[3, 1].onClick.AddListener(() => keyboardbuttons(new char[1] { '0' }));
		menu2[3, 2].onClick.AddListener(() =>
		{
			currBtn = "Swap";

			showPad(1);
			menu[3, 2].Select();

			prevBtn = "Swap";
		});

		showPad(1);
		menu[0, 0].Select();
	}

	void move(float vert, float hori) {
		if(vert == 0 && hori == 0) {
			menuMoved = false;
			return;
		}

		if(menuMoved == false) {
			menuMoved = true;
			if(vert < 0) {
				menuVertLoc = (menuVertLoc +1)%(vertSize);
			} else if(vert > 0) {
				--menuVertLoc;
				if(menuVertLoc < 0) {
					menuVertLoc = vertSize - 1;
				}
			}

			if(hori > 0) {
				menuHoriLoc = (menuHoriLoc +1)%(horiSize);
			} else if(hori < 0) {
				--menuHoriLoc;
				if(menuHoriLoc < 0) {
					menuHoriLoc = horiSize - 1;
				}
			}
			prevBtn = "";
			arrLoc = 0;
		}
	}

	void showPad(int padType) {
		switch(padType) {
		case 1:
                // caps pad
			foreach(Button btn in menu) {
				btn.gameObject.SetActive(true);
			}

			foreach(Button btn in menu2) {
				btn.gameObject.SetActive(false);
			}
			currentMenu = menu;
			//currPad = 1;
			break;
		case 2:
			Debug.Log("yeah");
			break;
		case 3:
                // num pad
			foreach(Button btn in menu2) {
				btn.gameObject.SetActive(true);
			}

			foreach(Button btn in menu) {
				btn.gameObject.SetActive(false);
			}
			currentMenu = menu2;
			//currPad = 3;
			break;
		default:
			Debug.Log("padType is invalid.");
			break;
		}
	}
	
	// Update is called once per frame
	void Update() {
		move(Input.GetAxisRaw(controls.vert), Input.GetAxisRaw(controls.hori));
		currentMenu[menuVertLoc, menuHoriLoc].Select();
		if(Input.GetButtonUp(controls.joySecItem)) {
			if(fieldCharName.text.Length > 0) {
				fieldCharName.text = fieldCharName.text.Remove(fieldCharName.text.Length - 1);
				arrLoc = 0;
				prevBtn = "";
			}
		}
	}
}
