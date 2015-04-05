using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Handles all logic for the title screen.
public class TitleHand : MonoBehaviour {
    private GSManager gsManager;
    
    /* TITLE SCREEN */
    private GameObject loadingBG;
    private Button btnStartGame;

    /* LEVEL SELECT */
    private GameObject btnLevel1;

	void Start ()
    {  
		gsManager = GameObject.Find("GSManager").GetComponent<GSManager>();
        btnStartGame = GameObject.Find("BtnStartGame").GetComponent<Button>();
        btnLevel1 = GameObject.Find("BtnLevel1");

		// hide LoadingBG. can't hide in inspector because Find() can't find hidden objects.
        loadingBG = GameObject.Find("LoadingBG");
        loadingBG.SetActive(false);

        // hide level select
        btnLevel1.SetActive(false);

        // add functions to be called when buttons are clicked
        btnStartGame.onClick.AddListener(() =>
            {
                btnLevel1.SetActive(true);
            }
        );

        btnLevel1.GetComponent<Button>().onClick.AddListener(() =>
            {
				gsManager.level1Data = "topkek";
				gsManager.LoadLevel("5713573250596864");
			}
		);
	}
}
