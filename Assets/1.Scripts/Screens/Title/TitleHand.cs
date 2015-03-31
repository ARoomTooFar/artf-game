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
		//<- this one   
		gsManager = GameObject.Find("GSManager").GetComponent<GSManager>(); 
        btnStartGame = GameObject.Find("BtnStartGame").GetComponent<Button>();

		// hide LoadingBG. can't hide in inspector because Find() can't find hidden objects.
        loadingBG = GameObject.Find("LoadingBG");
        loadingBG.SetActive(false);

        // add functions to be called when buttons are clicked
        btnStartGame.onClick.AddListener(() =>
            {
				gsManager.level1Data = "topkek";
				//<-- this one
                gsManager.LoadScene(13); 
            }
        );
	}
}
