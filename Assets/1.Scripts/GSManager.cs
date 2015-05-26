using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GSManager : MonoBehaviour {
    // persistent data
    public static GSManager gsManager;
	public string currLevelId = "";
    public string currLevelData = "";
	public PlayerData[] playerDataList;
    public Player[] players;
    public List<int> leaderList;
    public List<string> loot; //holds looted items

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

            playerDataList = new PlayerData[4];
            players = new Player[4];
            leaderList = new List<int>();
            loot = new List<string>();
            
            /*for (int i = 0; i < playerDataList.Length; ++i)
            {
                playerDataList[i] = gameObject.AddComponent<PlayerData>();
                DontDestroyOnLoad(playerDataList[i]);
            }*/
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

		if(GameObject.Find("LoadingBar") != null)
			loadingBar = GameObject.Find("LoadingBar").GetComponent<Slider>();

        /*DontDestroyOnLoad(loot);
        DontDestroyOnLoad(playerDataList);
        DontDestroyOnLoad(leaderList);
        DontDestroyOnLoad(players);*/
	}

	public void LoadScene (string sceneName)
	{
		if(loadingBG != null)
			loadingBG.SetActive(true);

        /*for (int i = 0; i < playerDataList.Length; ++i)
        {
            if (playerDataList[i] != null)
            {
                Debug.Log("called");
                DontDestroyOnLoad(playerDataList[i]);
            }
        }*/

		StartCoroutine(LoadSceneAsync(sceneName));
	}

	IEnumerator LoadSceneAsync (string sceneName)
	{
        for (int i = 0; i < playerDataList.Length; ++i)
        {
            if (playerDataList[i] != null)
            {
                DontDestroyOnLoad(playerDataList[i]);
                Debug.Log("dontdestroy: " + i);
            }
            else
            {
                Debug.Log("not dontdestroy: " + i);
            }
        }

		loadProgress = Application.LoadLevelAsync(sceneName);
		
		while (!loadProgress.isDone)
		{
			if(loadingBar != null)
				loadingBar.value = loadProgress.progress;
			yield return null;
		}

		// after loading is done, find new LoadingBG in new scene
		//loadingBG = GameObject.Find("LoadingBG");
		//loadingBG.SetActive (false);
	}

	public void LoadLevel (string levelId)
	{
		//loadingBG.SetActive(true);
		StartCoroutine(DlLevel(levelId));
	}

	public IEnumerator DlLevel(string levelId)
	{
		WWW www = serv.getLvlWww(levelId);
		
		yield return www;
		
		currLevelData = www.text;
		
		if (serv.dataCheck(currLevelData))
		{
			Debug.Log("LVL DL SUCCESS: " + currLevelData);
			Debug.Log ("LVL ID: " + currLevelId);
			yield return StartCoroutine(LoadSceneAsync("Proto-level-loading"));
			
			// after loading is done, find new LoadingBG in new scene
			//loadingBG = GameObject.Find("LoadingBG");
			//loadingBG.SetActive(false);
		}
		else
		{
			Debug.Log("ERROR: Level download failed. Level ID doesn't exist.");
		}
	}
}
