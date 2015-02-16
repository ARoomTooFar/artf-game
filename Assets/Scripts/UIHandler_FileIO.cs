using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary; 
using System.Runtime.Serialization;
using System.Text;

//This class listens to save/deploy/load buttons
public class UIHandler_FileIO : MonoBehaviour
{
	Output_TileMap output_tileMap;
	public Button Button_Save = null;
	public Button Button_Deploy = null;
	public Button Button_Load = null;
	//	public InputField InputField_Save = null;
	//	public InputField InputField_Load = null;
//	MouseHandler_TileSelection tileSelection;
	//	Dictionary<string, Vector3> savedState;
//	DataHandler_Items data;
	BinaryWriter bin;
//	private StreamWriter writer; // This is the writer that writes to the file
//	private string assetText;
	static ItemClass itemClass;
//	List<string> savedFiles;
//	string fileToLoad;
	private Farts serv;
	long levelId;
	string levelTHING;
	
	//object in hierarchy that holds itemObjects
	Transform itemObjects;
	
	void Start ()
	{
		itemClass = new ItemClass ();
//		savedFiles = new List<string>();
		
		output_tileMap = GameObject.Find ("TileMap").GetComponent ("Output_TileMap") as Output_TileMap;
		
		serv = gameObject.AddComponent<Farts> ();
		Button_Save.onClick.AddListener (() => {
			saveFile (); });
		Button_Load.onClick.AddListener (() => {
			loadFile ();});
		
		
//		tileSelection = GameObject.Find ("TileMap").GetComponent ("MouseHandler_TileSelection") as MouseHandler_TileSelection;
		//		savedState = new Dictionary<string, Vector3> ();
		
//		data = GameObject.Find ("ItemObjects").GetComponent ("DataHandler_Items") as DataHandler_Items;
		
		itemObjects = GameObject.Find ("ItemObjects").GetComponent ("Transform") as Transform;
		
	}
	
	public void saveFile ()
	{
		
		
		//		BinaryFormatter bf = new BinaryFormatter ();
		//		FileStream file = File.Create ("Assets/Resources/savedLevel.txt");
		
		if (itemClass.getItemList ().Count != 0) {
			//old playerprefs way
			//		if (itemClass.getItemList ().Count != 0 && InputField_Save.text.Length != 0) {
			//			bf.Serialize (file, itemClass.getItemList ());
			//			File.WriteAllText("Assets/Resources/savedLevel.txt", bf.Serialize (file, itemClass.getItemList ()));
			//			Debug.Log ("Items saved right now: " + itemClass.getItemList ().Count);
			//			Debug.Log ("saving list " + itemClass.getItemList ().GetHashCode());
			
			BinaryFormatter bf = new BinaryFormatter ();
			MemoryStream stream = new MemoryStream (2048);
			bf.Serialize (stream, itemClass.getItemList ());
			string tmp = System.Convert.ToBase64String (stream.ToArray ());
			
			
			//old playerprefs method of saving
			//			PlayerPrefs.SetString (InputField_Save.text, tmp);
			//			if (!savedFiles.Contains(InputField_Save.text)){
			//				savedFiles.Add(InputField_Save.text);
			//			}
			
			//new 1/15 v3.0 server way
			//levelTHING = serv.newLevel("stuff",  "34534567", "34563456", tmp, "32453245");
			levelTHING = serv.updateLevel("6211504244260864",  "stuff", "34563456", tmp, "32453245");
			Debug.Log(levelTHING);
			
			
			
			//			string levelData = bf.Serialize (file, itemClass.getItemList ());
			//			levelId = serv.newLevel(123, "Level Name", levelData);
		} else if (itemClass.getItemList ().Count == 0) {
			Debug.Log ("ItemClass.itemList is empty. Nothing to write.");
		}
		
		//old playerprefs way
		//		} else if (InputField_Save.text.Length == 0) {
		//			Debug.Log ("Enter Filename");
		//		}
		//		file.Close ();
		
		//		if (data.getItemDictionary ().Count != 0) {
		//			savedState.Clear ();
		//			savedState = new Dictionary<string, Vector3> (data.getItemDictionary ());
		//		} else {
		//			Debug.Log ("Nothing to save");
		//		}
	}
	
	void buttonLoadFile(string s){
		
		Debug.Log ("loading: " + s);
		
		wipeItemObjects ();
		itemClass.clearItemList ();
		
		BinaryFormatter bf = new BinaryFormatter ();
		string tmp = PlayerPrefs.GetString (s);
		MemoryStream memoryStream = new MemoryStream (System.Convert.FromBase64String (tmp));
		List<ItemClass.ItemStruct> savedFile = (List<ItemClass.ItemStruct>)bf.Deserialize (memoryStream);
		
		for (int i = 0; i < savedFile.Count; i++) {
			Vector3 pos = new Vector3 (savedFile [i].x, savedFile [i].y, savedFile [i].z);
			Vector3 rot = new Vector3 (savedFile [i].xrot, savedFile [i].yrot, savedFile [i].zrot);
			string name = savedFile [i].item.Substring (0, savedFile [i].item.IndexOf ('_'));
			//				tileSelection.placeItems (name, pos);
			output_tileMap.instantiateItemObject (name, pos, rot);
		}
		
		Transform h = GameObject.Find("FilenameButtons").GetComponent("Transform") as Transform;
		foreach (Transform child in h) {
			GameObject.Destroy (child.gameObject);
		}
	}
	
	public void loadFile ()
	{
		
		//		if (savedFiles.Count != 0) {
		//			float y = -415f;
		//			foreach (string s in savedFiles) {
		//				GameObject newButton = Instantiate (Resources.Load ("Button_Filename")) as GameObject;
		//				Button button = newButton.GetComponent ("Button") as Button;
		//				button.transform.parent = GameObject.Find("FilenameButtons").GetComponent("Transform") as Transform;
		//				RectTransform rt = button.GetComponent("RectTransform") as RectTransform;
		//				rt.localScale = new Vector3(1f, 1f, 1f);
		//				rt.anchoredPosition = new Vector2(-286.8f, y);
		//				Text buttonText = (button.transform.Find("Text")).GetComponent ("Text") as Text;
		//				buttonText.text = s;
		//				y += 25f;
		//
		//				button.onClick.AddListener (() => {
		//					buttonLoadFile (buttonText.text);});
		//			}
		//		}
		
		
		
		
		
		
		
		//old playerprefs way
		//		bool butts = true;
		//		if (butts == true && InputField_Load.text.Length != 0 && PlayerPrefs.GetString (InputField_Load.text).Length != 0) {
		
		
		
		//		if (File.Exists ("Assets/Resources/savedLevel.txt")) {
		//			data.wipeItemObjects ();
		wipeItemObjects ();
		
		//			data.clearItemDictionary ();
		itemClass.clearItemList ();
		
		
		BinaryFormatter bf = new BinaryFormatter ();
		
		//old playerprefs way of storing
		//			string tmp = PlayerPrefs.GetString (InputField_Load.text);
		//new google drive way
		string tmp = serv.getLevel("6211504244260864");
		
		MemoryStream memoryStream = new MemoryStream (System.Convert.FromBase64String (tmp));
//		Debug.Log (" file contents: " + tmp);
		List<ItemClass.ItemStruct> savedFile = (List<ItemClass.ItemStruct>)bf.Deserialize (memoryStream);
		
		
		
		
		//download levelData from server
		//			string levelData = "";
		//			levelData = serverConnect.getLevel(levelId); //string is level ID
		
		
		
		
		//			BinaryFormatter bf = new BinaryFormatter ();
		//			FileStream file = File.Open ("Assets/Resources/savedLevel.txt", FileMode.Open);
		//			List<ItemClass.ItemStruct> savedFile = (List<ItemClass.ItemStruct>)bf.Deserialize (file);
		//			file.Close ();
		
		
		
		//			Debug.Log ((List<ItemClass.ItemStruct>)bf.Deserialize);
		for (int i = 0; i < savedFile.Count; i++) {
			Vector3 pos = new Vector3 (savedFile [i].x, savedFile [i].y, savedFile [i].z);
			Vector3 rot = new Vector3 (savedFile [i].xrot, savedFile [i].yrot, savedFile [i].zrot);
			string name = savedFile [i].item.Substring (0, savedFile [i].item.IndexOf ('_'));
			//				tileSelection.placeItems (name, pos);
			output_tileMap.instantiateItemObject (name, pos, rot);
		}
		
		
		//old playerprefs way
		//		} else if (PlayerPrefs.GetString (InputField_Load.text).Length == 0) {
		//			Debug.Log ("No saved level data");
		//		} else if (InputField_Load.text.Length == 0) {
		//			Debug.Log ("Enter a file to load");
		//		}
		
		
		
		//		} else {
		//			Debug.Log ("savedLevel.txt does not exist. Cannot load.");
		//		}
		
		
		
		//		if (savedState.Count != 0) {
		//			data.wipeItemObjects ();
		//			data.clearItemDictionary ();
		//			foreach (KeyValuePair<string, Vector3> entry in savedState) {
		//				string key = entry.Key.Substring (0, entry.Key.IndexOf ('_'));
		//
		//				tileSelection.placeItems (key, entry.Value);
		//			}
		//		} else {
		//			Debug.Log ("Nothing to load");
		//		}
	}
	
	public void wipeItemObjects ()
	{
		foreach (Transform child in itemObjects) {
			GameObject.Destroy (child.gameObject);
		}
		itemClass.resetNameCounter ();
	}
	
	
	
	//	public Transform getItemObjects ()
	//	{
	//		return itemObjects;
	//	}
	
	
}
