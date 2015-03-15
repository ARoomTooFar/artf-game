using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GSManager : MonoBehaviour {
    // persistent data
    public static GSManager gsManager;
    public string level1Data = "level1Data";
	public string level2Data = "level2Data";
	public string level3Data = "level3Data";
	public string level4Data = "level4Data";
	public string level5Data = "level5Data";
	public string level6Data = "level6Data";
	public string level7Data = "level7Data";
	public string level8Data = "level8Data";

    private Farts serv;
	private GameObject loadingBG;
	private Slider loadingBar;
	private AsyncOperation loadProgress;

	void Awake ()
	{
        // ensure gsManager is a singleton
        if (gsManager == null)
        {
            DontDestroyOnLoad(gameObject);  // don't destroy itself between scene loads
            gsManager = this;
        }
        else if (gsManager != this)
        {
            Destroy(gameObject);
        }
	}

	void Start ()
	{
        serv = gameObject.AddComponent<Farts>();
		loadingBG = GameObject.Find("LoadingBG");
		loadingBar = GameObject.Find("LoadingBar").GetComponent<Slider>();
	}

	public void LoadScene (int scene)
	{
		loadingBG.SetActive(true);
		StartCoroutine(LoadSceneAsync(scene));
	}

    public IEnumerator LoadLevel(string levelId)
    {
        loadingBG.SetActive(true);

        WWW www = serv.getLvlWww(levelId);

        yield return StartCoroutine(dlLevel(www));
        yield return StartCoroutine(LoadSceneAsync(13));

        // after loading is done, find new LoadingBG in new scene
        loadingBG = GameObject.Find("LoadingBG");
        loadingBG.SetActive(false);
    }

	IEnumerator LoadSceneAsync (int scene)
	{
		loadProgress = Application.LoadLevelAsync(scene);
		
		while (!loadProgress.isDone)
		{
			loadingBar.value = loadProgress.progress;
			yield return null;
		}

		// after loading is done, find new LoadingBG in new scene
		loadingBG = GameObject.Find("LoadingBG");
		loadingBG.SetActive (false);
	}

    public IEnumerator dlLevel(WWW www)
    {
        yield return www;

        gsManager.level1Data = www.text;

        if (serv.dataCheck(gsManager.level1Data))
        {
            Debug.Log("LVL DL SUCCESS: " + gsManager.level1Data);
        }
        else
        {
            Debug.Log("ERROR: Level download failed. Level ID doesn't exist.");
        }
    }
}
