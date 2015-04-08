using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIControllerTestHand : MonoBehaviour {
    public Controls controls;
    private Button btn1;
    private Button btn2;
    private Button[] menu;

	// Use this for initialization
	void Start () {
        menu = new Button[2];

        menu[0] = GameObject.Find("Btn1").GetComponent<Button>();
        menu[1] = GameObject.Find("Btn2").GetComponent<Button>();

        menu[0].onClick.AddListener(() =>
        {
            Debug.Log("eh");
        }
        );

        menu[1].onClick.AddListener(() =>
        {
            Debug.Log("meh");
        }
        );
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis(controls.vert) > 0)
            menu[0].Select();
        else if (Input.GetAxis(controls.vert) < 0)
            menu[1].Select();
	}
}
