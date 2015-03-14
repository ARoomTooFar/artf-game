using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Handles all logic for the title screen.
public class TitleHand : MonoBehaviour {
    private GSManager gsManager;
    private Button btnStartGame;
    private GameObject loadingBG;

	void Start ()
    {
        gsManager = GameObject.Find("GSManager").GetComponent<GSManager>();
        btnStartGame = GameObject.Find("BtnStartGame").GetComponent<Button>();
        loadingBG = GameObject.Find("LoadingBG");

        loadingBG.SetActive(false);  // hide LoadingBG. can't hide in inspector because Find() can't find hidden objects.

        // add functions to be called when buttons are clicked
        btnStartGame.onClick.AddListener(() =>
            {
                StartGame();
            }
        );
	}

    void StartGame()
    {
        loadingBG.SetActive(true);
        gsManager.LoadScene(13);
    }
}
