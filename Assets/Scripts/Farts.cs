using UnityEngine;
using System.Collections;

public class Farts : MonoBehaviour {
	//const string SERVERURI = "http://localhost:8081"; //local server
	const string SERVERURI = "https://api-dot-artf-server.appspot.com"; //live server
	const string LEVELPATH = "/levels/";
	
	// need to implement cases where level is not found and request timeout
	public string getLevel(string levelId) {
		WWW www = new WWW(SERVERURI + LEVELPATH + levelId);
		
		StartCoroutine(httpRequest(www));
		while(www.isDone == false) {
			//Debug.Log("HTTP request in progress...");
		}
		
		return www.text;
	}
	
	public string newLevel(string lvlName, string gameAcctId, string machId, string liveLvlData="", string draftLvlData="") {
		WWWForm form = new WWWForm();
		
		form.AddField("level_name", lvlName);
		form.AddField("game_acct_id", gameAcctId);
		form.AddField("mach_id", machId);
		if(liveLvlData != "")
			form.AddField ("live_level_data", liveLvlData);
		if(draftLvlData != "")
			form.AddField ("draft_level_data", draftLvlData);
		
		WWW www = new WWW(SERVERURI + LEVELPATH, form);
		StartCoroutine(httpRequest(www));
		while(www.isDone == false) {
			//Debug.Log("HTTP request in progress...");
		}
		
		return www.text;
	}
	
	public string updateLevel(string lvlId, string lvlName="", string gameAcctId="", string liveLvlData="", string draftLvlData="") {
		WWWForm form = new WWWForm();
		
		form.AddField ("flag", "update");
		if(lvlName != "")
			form.AddField ("level_name", lvlName);
		if(gameAcctId != "")
			form.AddField ("game_acct_id", gameAcctId);
		if(liveLvlData != "")
			form.AddField ("live_level_data", liveLvlData);
		if(draftLvlData != "")
			form.AddField ("draft_level_data", draftLvlData);
		
		WWW www = new WWW(SERVERURI + LEVELPATH + lvlId, form);
		StartCoroutine(httpRequest(www));
		while(www.isDone == false) {
			//Debug.Log("HTTP request in progress...");
		}
		
		return www.text;
	}
	
	public string deleteLevel(string lvlId) {
		WWWForm form = new WWWForm();
		
		form.AddField ("flag", "delete");
		form.AddField ("level_id", lvlId);
		
		WWW www = new WWW(SERVERURI + LEVELPATH + lvlId, form);
		StartCoroutine(httpRequest(www));
		while(www.isDone == false) {
			//Debug.Log("HTTP request in progress...");
		}
		
		return www.text;
	}
	
	IEnumerator httpRequest(WWW www) {
		yield return www;
		
		/*if (www.error == null) {
			Debug.Log("WWW SUCCESS: " + www.url);
		} else {
			Debug.Log("WWW ERROR: " + www.error);
		}*/
	}
}
