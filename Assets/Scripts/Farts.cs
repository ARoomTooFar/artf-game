using UnityEngine;
using System.Collections;

public class Farts : MonoBehaviour
{
    //const string SERVERURI = "http://localhost:8081"; //local server
    const string SERVERURI = "https://api-dot-artf-server.appspot.com"; //live server
    const string LVLPATH = "/levels/";
    const string GAMEACCTPATH = "/gameaccount/";
    const float timeoutTime = 60f; //HTTP requests timeout after 1 minute

    // Checks if returned data is valid or not. Returns true if the data is valid, false otherwise.
    public bool dataCheck(string input)
    {
        if (input == "") return false;
        return true;
    }

    public WWW getLvlWww(string levelId)
    {
        WWW www = new WWW(SERVERURI + LVLPATH + levelId);
        return www;
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

    public string login(string gameAcctName, string gameAcctPass)
    {
        WWWForm form = new WWWForm();
        form.AddField("game_acct_name", gameAcctName);
        form.AddField("game_acct_password", gameAcctPass);

        WWW www = new WWW(SERVERURI + GAMEACCTPATH + "login", form);
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
