using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuCtrl : MonoBehaviour {
    private int menuWidth = 1;
    private int menuHeight = 2;

    private GameObject[,] p1Menu;
    private int p1LocX = 0;
    private int p1LocY = 0;

	void Start () {
        // create p1Menu
        p1Menu = new GameObject[menuHeight, menuWidth];

        p1Menu[0, 0] = GameObject.Find("/Canvas/BtnLogin");
        p1Menu[1, 0] = GameObject.Find("/Canvas/BtnRegister");

        p1Menu[0, 0].GetComponent<Button>().Select();
	}

    void Move () {
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
