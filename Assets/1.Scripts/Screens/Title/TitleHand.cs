using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Handles all logic for the title screen.
public class TitleHand : MonoBehaviour {
    private GSManager gsManager;
    private Button btnStartGame;
    private GameObject loadingBG;
    private Slider loadingBar;

    private AsyncOperation loadingProgress;

	void Start ()
    {
        gsManager = GameObject.Find("GSManager").GetComponent<GSManager>();
        btnStartGame = GameObject.Find("BtnStartGame").GetComponent<Button>();

        loadingBG = GameObject.Find("LoadingBG");
        loadingBar = GameObject.Find("LoadingBar").GetComponent<Slider>();

        loadingBG.SetActive(false);  // hide LoadingBG. can't hide in inspector because Find() can't find hidden objects.

        // add functions to be called when buttons are clicked
        btnStartGame.onClick.AddListener(() =>
            {
                loadSceneAsync(14);
                //StartGame();
            }
        );
	}

    void StartGame()
    {
        loadingBG.SetActive(true);
        gsManager.LoadScene(13);
    }

    void loadSceneAsync(int scene)
    {
        loadingBG.SetActive(true);
        StartCoroutine(LoadLevelWithBar(scene));
    }

    IEnumerator LoadLevelWithBar(int scene)
    {
        loadingProgress = Application.LoadLevelAsync(scene);

        while (!loadingProgress.isDone)
        {
            loadingBar.value = loadingProgress.progress;
            yield return null;
        }
    }
}
