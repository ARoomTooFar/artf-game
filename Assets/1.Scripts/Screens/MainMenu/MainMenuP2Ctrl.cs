using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuP2Ctrl : MonoBehaviour {
    public Controls controls;

    // UI state
    private int menuWidth = 1;
    private int menuHeight = 2;
    private bool menuMoved = false;

    // player1 menu
    private GameObject[,] p2Menu;
    private int p2LocX = 0;
    private int p2LocY = 0;

	void Start () {
        // create p1 Start menu
        p2Menu = new GameObject[menuHeight, menuWidth];

        p2Menu[0, 0] = GameObject.Find("/P2Canvas/P2MenuContainer/BtnP2Login");
        p2Menu[1, 0] = GameObject.Find("/P2Canvas/P2MenuContainer/BtnP2Register");

        p2Menu[0, 0].GetComponent<Button>().Select();

        p2Menu[0, 0].GetComponent<Button>().onClick.AddListener(() =>
        {
            //GameObject camera = GameObject.Find("/Main Camera");
            //camera.MoveCameraDown();

            //GameObject.Find("/Canvas").GetComponent<Animator>().SetTrigger("fadeOut");
            GameObject.Find("/Main Camera").GetComponent<MainMenuCamera>().slideDown = true;
            SendSubmitEventToSelectedObject();
        });
	}

    private bool SendSubmitEventToSelectedObject()
    {
        // Return if nothing is selected
        if (EventSystem.current.currentSelectedGameObject == null)
            return false;

        ExecuteEvents.Execute<ISubmitHandler>(EventSystem.current.currentSelectedGameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);

        return true;
    }

    void MenuMove(float hori, float vert)
    {
        if (vert == 0 && hori == 0)
        {
            menuMoved = false;
        }
        else if (menuMoved == false)
        {
            menuMoved = true;

            if (vert < 0)
            {
                p2LocY = (p2LocY + 1) % (menuHeight);
            }
            else if (vert > 0)
            {
                --p2LocY;
                if (p2LocY < 0)
                {
                    p2LocY = menuHeight - 1;
                }
            }

            p2Menu[p2LocY, p2LocX].GetComponent<Button>().Select();
            Debug.Log(p2LocY);
        }
    }
	
	// Update is called once per frame
    void Update()
    {
        MenuMove(Input.GetAxisRaw(controls.hori), Input.GetAxisRaw(controls.vert));
    }
}
