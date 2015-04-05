using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GSManager : MonoBehaviour {
    // persistent data
    public static GSManager gsManager;
    public string currLevelData = "";

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

	public void LoadLevel (string levelId)
	{
		loadingBG.SetActive(true);
		StartCoroutine(DlLevel(levelId));
	}

	public IEnumerator DlLevel(string levelId)
	{
		Debug.Log ("started");
		
		WWW www = serv.getLvlWww(levelId);
		
		yield return www;
		
		currLevelData = www.text;
		
		if (serv.dataCheck(currLevelData))
		{
			Debug.Log("LVL DL SUCCESS: " + currLevelData);
			yield return StartCoroutine(LoadSceneAsync(1));
			
			// after loading is done, find new LoadingBG in new scene
			loadingBG = GameObject.Find("LoadingBG");
			loadingBG.SetActive(false);
		}
		else
		{
			Debug.Log("ERROR: Level download failed. Level ID doesn't exist.");
		}
	}
}
