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
	public PlayerData[] dummyPlayerDataList;
    public Player[] players;
    public List<int> leaderList;
    public List<string> loot; //holds looted items

	// gear select ready up tracker
	public int currReady = 0; // checks how many players are currently ready
	public int maxReady = 0; // max possible readied players

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
			dummyPlayerDataList = new PlayerData[4];
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

		/*// create dummy data
		PlayerData dummyP1Data = serv.parseCharData("80PercentLean,123,456,789,9001,1,0,3,0,5,0,7,0,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52");
		PlayerData dummyP2Data = serv.parseCharData("Player2Dood,123,456,789,9001,0,1,3,2,4,6,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0");
		PlayerData dummyP3Data = serv.parseCharData("Prinny,123,456,789,9001,0,1,3,2,4,6,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0");
		PlayerData dummyP4Data = serv.parseCharData("Eyayayayaya,123,456,789,9001,0,1,3,2,4,6,5,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0");

		dummyPlayerDataList [0] = dummyP1Data;
		dummyPlayerDataList [1] = dummyP2Data;
		dummyPlayerDataList [2] = dummyP3Data;
		dummyPlayerDataList [3] = dummyP4Data;*/

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
                //DontDestroyOnLoad(playerDataList[i]);
                Debug.Log("dontdestroy: " + i);
            }
            else
            {
                Debug.Log("not dontdestroy: " + i);
            }
        }

        /*Debug.Log("dontdestroy");
        foreach (PlayerData playerData in playerDataList) {
            if (playerData != null) {
                Debug.Log(playerData);
                DontDestroyOnLoad(playerData);
            }
        }*/

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

	// clears the data stored
	public void ClearData () {
		playerDataList = new PlayerData[4];
		players = new Player[4];
		leaderList = new List<int>();
		loot = new List<string>();
		currReady = 0;
		maxReady = 0;
		currLevelId = "";
		currLevelData = "";
	}

	void Update() {
		if (currReady == maxReady && maxReady != 0) {
			currReady = 0;
			//gsManager.LoadScene("TestLevelSelect");
			gsManager.LoadLevel (currLevelId);
		}
	}
}
