using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.IO; 
//using UnityEditor;

public class SayHi : MonoBehaviour {
	public string FileName; // This contains the name of the file. Don't add the ".txt"
	// Assign in inspector
	//private TextAsset asset; // Gets assigned through code. Reads the file.
	private StreamWriter writer; // This is the writer that writes to the file
	private string assetText;
	public TextAsset asset;
	public int testnum;
	
	[SerializeField]
	private Button readbutton = null; // assign in the editor

	[SerializeField]
	private Button writebutton = null; // assign in the editor

	//[SerializeField]
	public Slider sliderguy = null;
	public InputField inputguy = null;

	public Transform thingtoslide;

	public string url = "https://github.com/ARoomTooFar/artf-level-editor/blob/aaronuiexperiments/Assets/FileIO.cs";
	WWW www;

	void Start()
	{
		testnum = 1;
		FileName = "textfile";
//		readbutton.onClick.AddListener(() => { getText(); });
//		writebutton.onClick.AddListener(() => { writeText();});
//		sliderguy.onValueChanged.AddListener(this.sliderstuff);


	}


	void Update () {

	}



	public void sliderstuff(float t){
		Debug.Log(t);
		t = t * 2;


		Vector3 thingscale = thingtoslide.transform.localScale;
		if (t <= 0.5) t = 0.5f;
		float multi = Mathf.Pow(t, 3f);
		thingscale = new Vector3(multi,multi,multi);


		thingtoslide.transform.localScale = thingscale;
	}



	IEnumerator getURL() {
		www = new WWW(url);
		yield return www;
		Debug.Log(www.text);
		inputguy.text = www.text;
	}
	
	public void writeText(){
		StartCoroutine (getURL());

//		AppendString ("i like pie: gg" + testnum++ + inputguy.text);
//		inputguy.text = Application.dataPath;
	}
	
	public void getText(){
		//Debug.Log("hio");
		//AssetDatabase.Refresh();
		assetText = asset.text;
		Debug.Log(assetText);
	}

	void AppendString(string appendString) {
		//asset = Resources.Load(FileName + ".txt") as TextAsset;
		//writer = new StreamWriter("Assets/Resources/" + FileName + ".txt"); // Does this work?
		writer = new StreamWriter("Assets/Resources/" + asset.name  + ".txt");
		writer.WriteLine(appendString);
		writer.Close();
	}
}
