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
	private GameObject btnLevel2;
	private GameObject btnLevel3;

	void Start ()
    {  
		gsManager = GameObject.Find("GSManager").GetComponent<GSManager>();
        btnStartGame = GameObject.Find("BtnStartGame").GetComponent<Button>();
        btnLevel1 = GameObject.Find("BtnLevel1");
		btnLevel2 = GameObject.Find("BtnLevel2");
		btnLevel3 = GameObject.Find("BtnLevel3");

		// hide LoadingBG. can't hide in inspector because Find() can't find hidden objects.
        loadingBG = GameObject.Find("LoadingBG");
        loadingBG.SetActive(false);

        // hide level select
		hideLevelSelect ();

        // add functions to be called when buttons are clicked
        btnStartGame.onClick.AddListener(() =>
            {
				showLevelSelect();
            }
        );

        btnLevel1.GetComponent<Button>().onClick.AddListener(() =>
            {
				hideLevelSelect ();
				gsManager.LoadLevel("5713573250596864");
			}
		);

		btnLevel2.GetComponent<Button>().onClick.AddListener(() =>
		    {
				hideLevelSelect ();
				gsManager.LoadLevel("4876504257265664");
			}
		);

		btnLevel3.GetComponent<Button>().onClick.AddListener(() =>
			{
				hideLevelSelect ();
				gsManager.LoadLevel("4851447149625344");
			}
		);
	}

	void hideLevelSelect () {
		btnLevel1.SetActive(false);
		btnLevel2.SetActive(false);
		btnLevel3.SetActive(false);
	}

	void showLevelSelect () {
		btnLevel1.SetActive(true);
		btnLevel2.SetActive(true);
		btnLevel3.SetActive(true);
	}
}
