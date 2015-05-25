using UnityEngine;
using System.Collections;

public class Farts : MonoBehaviour
{
	//const string SERVERURI = "http://localhost:8080/api"; //local server
	const string SERVERURI = "https://artf-server.appspot.com/api"; //live server
	const string LVLPATH = "/levels/";
	const string GAMEACCTPATH = "/gameaccts/";
	const string CHARPATH = "/characters/";
	const string MATCHMAKEPATH = "/matchmake/";
	const float timeoutTime = 60f; //HTTP requests timeout after 1 minute


	/* DATA MANAGEMENT */
	// Checks if returned data is valid or not. Returns true if the data is valid, false otherwise.
	public bool dataCheck(string levelData)
	{
		if (levelData == "error" || levelData == "") return false;
		return true;
	}

	public PlayerData parseCharData(string charData) {
		PlayerData playerData = gameObject.AddComponent<PlayerData>();
		int[] inventory = new int[52];
		string[] parsedData = charData.Split (',');
		
		playerData.name = parsedData[0];
		playerData.char_id = int.Parse(parsedData [1]);
		playerData.hair_id = int.Parse(parsedData [2]);
		playerData.voice_id = int.Parse(parsedData [3]);
		playerData.money = int.Parse(parsedData [4]);
		
		for (int i = 5; i < parsedData.Length; ++i) {
			inventory[i - 5] = int.Parse(parsedData[i]);
		}
		
		playerData.inventory = inventory;
		
		return playerData;
	}
	
	public string stringifyCharData(PlayerData playerData) {
		string newCharData = "";
		
		newCharData += playerData.name;
		newCharData += "," + playerData.char_id.ToString ();
		newCharData += "," + playerData.hair_id.ToString ();
		newCharData += "," + playerData.voice_id.ToString ();
		newCharData += "," + playerData.money.ToString ();
		
		for (int i = 0; i < playerData.inventory.Length; ++i) {
			newCharData += "," + playerData.inventory[i];
		}
		
		return newCharData;
	}

	public string[] parseListLevelData (string levelListData) {
		string[] parsedData = levelListData.Split (',');
		return parsedData;
	}
	

	/* NETWORKING */
	public WWW getLvlWww(string levelId)
	{
		WWW www = new WWW(SERVERURI + LVLPATH + levelId);
		return www;
	}

	public string getLvl(string levelId)
	{
		WWW www = new WWW(SERVERURI + LVLPATH + levelId);
		StartCoroutine(httpRequest(www));
		
		float timeStart = Time.realtimeSinceStartup;
		
		while (!www.isDone)
		{
			if (Time.realtimeSinceStartup >= timeStart + timeoutTime)
			{
				Debug.LogError("ERROR: Request timeout");
				return "";
			}
			//Debug.Log("HTTP request time elapsed: " + (Time.realtimeSinceStartup - timeStart));
		}
		
		return www.text;
	}
	
	public WWW newLvlWww(string gameAcctId, string machId, string liveLvlData = "", string draftLvlData = "")
	{
		WWWForm form = new WWWForm();
		form.AddField("game_acct_id", gameAcctId);
		form.AddField("mach_id", machId);
		if (liveLvlData != "")
			form.AddField("live_level_data", liveLvlData);
		if (draftLvlData != "")
			form.AddField("draft_level_data", draftLvlData);
		WWW www = new WWW(SERVERURI + LVLPATH, form);
		
		return www;
	}
	
	public string newLvl(string gameAcctId, string machId, string liveLvlData = "", string draftLvlData = "")
	{
		WWWForm form = new WWWForm();
		form.AddField("game_acct_id", gameAcctId);
		form.AddField("mach_id", machId);
		if (liveLvlData != "")
			form.AddField("live_level_data", liveLvlData);
		if (draftLvlData != "")
			form.AddField("draft_level_data", draftLvlData);
		
		WWW www = new WWW(SERVERURI + LVLPATH, form);
		StartCoroutine(httpRequest(www));
		
		float timeStart = Time.realtimeSinceStartup;
		
		while (!www.isDone)
		{
			if (Time.realtimeSinceStartup >= timeStart + timeoutTime)
			{
				Debug.LogError("ERROR: Request timeout");
				return "";
			}
			//Debug.Log("HTTP request time elapsed: " + (Time.realtimeSinceStartup - timeStart));
		}
		
		return www.text;
	}
	
	public WWW updateLvl(string lvlId, string gameAcctId = "", string liveLvlData = "", string draftLvlData = "")
	{
		WWWForm form = new WWWForm();
		form.AddField("flag", "update");
		if (gameAcctId != "")
			form.AddField("game_acct_id", gameAcctId);
		if (liveLvlData != "")
			form.AddField("live_level_data", liveLvlData);
		if (draftLvlData != "")
			form.AddField("draft_level_data", draftLvlData);
		
		WWW www = new WWW(SERVERURI + LVLPATH + lvlId, form);
		StartCoroutine(httpRequest(www));
		
		return www;
	}
	
	public string delLvl(string lvlId)
	{
		WWWForm form = new WWWForm();
		form.AddField("flag", "delete");
		form.AddField("level_id", lvlId);
		
		WWW www = new WWW(SERVERURI + LVLPATH + lvlId, form);
		StartCoroutine(httpRequest(www));
		
		float timeStart = Time.realtimeSinceStartup;
		
		while (!www.isDone)
		{
			if (Time.realtimeSinceStartup >= timeStart + timeoutTime)
			{
				Debug.LogError("ERROR: Request timeout");
				return "";
			}
			//Debug.Log("HTTP request time elapsed: " + (Time.realtimeSinceStartup - timeStart));
		}
		
		return www.text;
	}
	
	public string register(string gameAcctName, string gameAcctPass, string charData)
	{
		WWWForm form = new WWWForm();
		form.AddField("game_acct_name", gameAcctName);
		form.AddField("game_acct_password", gameAcctPass);
		form.AddField("char_data", charData);
		
		WWW www = new WWW(SERVERURI + GAMEACCTPATH + "register", form);
		StartCoroutine(httpRequest(www));
		
		float timeStart = Time.realtimeSinceStartup;
		
		while (!www.isDone)
		{
			if (Time.realtimeSinceStartup >= timeStart + timeoutTime)
			{
				Debug.LogError("ERROR: Request timeout");
				return "";
			}
			//Debug.Log("HTTP request time elapsed: " + (Time.realtimeSinceStartup - timeStart));
		}

		return www.text;
	}

	public WWW loginWWW (string gameAcctName, string gameAcctPass) {
		WWWForm form = new WWWForm();
		form.AddField("game_acct_name", gameAcctName);
		form.AddField("game_acct_password", gameAcctPass);

		WWW www = new WWW(SERVERURI + GAMEACCTPATH + "login", form);
		StartCoroutine(httpRequest(www));
		
		return www;
	}
	
	public string login(string gameAcctName, string gameAcctPass)
	{
		WWWForm form = new WWWForm();
		form.AddField("game_acct_name", gameAcctName);
		form.AddField("game_acct_password", gameAcctPass);
		
		WWW www = new WWW(SERVERURI + GAMEACCTPATH + "login", form);
        //Debug.Log(SERVERURI + GAMEACCTPATH + "login");
		StartCoroutine(httpRequest(www));
		
		float timeStart = Time.realtimeSinceStartup;
		
		while (!www.isDone)
		{
			if (Time.realtimeSinceStartup >= timeStart + timeoutTime)
			{
				Debug.LogError("ERROR: Request timeout");
				return "";
			}
			//Debug.Log("HTTP request time elapsed: " + (Time.realtimeSinceStartup - timeStart));
		}

        Debug.Log(www.text); // display char data
		return www.text;
	}

	public string getChar(string charId)
	{
		WWW www = new WWW(SERVERURI + CHARPATH + charId);
		StartCoroutine(httpRequest(www));
		
		float timeStart = Time.realtimeSinceStartup;
		
		while (!www.isDone)
		{
			if (Time.realtimeSinceStartup >= timeStart + timeoutTime)
			{
				Debug.LogError("ERROR: Request timeout");
				return "";
			}
			//Debug.Log("HTTP request time elapsed: " + (Time.realtimeSinceStartup - timeStart));
		}
		
		return www.text;
	}
	
	public string updateChar(string charId, string charData)
	{
		WWWForm form = new WWWForm();
		form.AddField("char_id", charId);
		form.AddField("char_data", charData);
		
		WWW www = new WWW(SERVERURI + CHARPATH + charId, form);
		StartCoroutine(httpRequest(www));
		
		float timeStart = Time.realtimeSinceStartup;
		
		while (!www.isDone)
		{
			if (Time.realtimeSinceStartup >= timeStart + timeoutTime)
			{
				Debug.LogError("ERROR: Request timeout");
				return "";
			}
			//Debug.Log("HTTP request time elapsed: " + (Time.realtimeSinceStartup - timeStart));
		}
		
		return www.text;
	}

	// get random levels from server
	public string matchmakeWWW () {
		WWW www = new WWW(SERVERURI + MATCHMAKEPATH + "rand");
		StartCoroutine(httpRequest(www));
		
		float timeStart = Time.realtimeSinceStartup;
		
		while (!www.isDone)
		{
			if (Time.realtimeSinceStartup >= timeStart + timeoutTime)
			{
				Debug.LogError("ERROR: Request timeout");
				return "";
			}
			//Debug.Log("HTTP request time elapsed: " + (Time.realtimeSinceStartup - timeStart));
		}
		
		Debug.Log(www.text); // display matchmake data
		return www.text;
	}

	// get levels from server that match within difficulty
	public WWW matchmakeWWW (double difficulty) {
		double matchmakeVal = System.Math.Floor (difficulty);
		WWW www = new WWW(SERVERURI + MATCHMAKEPATH + matchmakeVal);
		return www;
	}
	
	IEnumerator httpRequest(WWW www)
	{
		//Debug.Log("COROUTINE WWW REQUEST START");
		
		yield return www;
		
		/*if (www.error == null)
        {
            Debug.Log("COROUTINE WWW SUCCESS: " + www.text);
        }
        else
        {
            Debug.Log("COROUTINE WWW ERROR: " + www.error);
        }*/
	}
}
