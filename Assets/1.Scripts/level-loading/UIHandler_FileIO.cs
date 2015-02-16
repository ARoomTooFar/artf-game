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
	BinaryWriter bin;
	private StreamWriter writer; // This is the writer that writes to the file
	private string assetText;
	static ItemClass itemClass;
	List<string> savedFiles;
	string fileToLoad;
	private Farts serv;
	long levelId;
	string levelTHING;

	//object in hierarchy that holds itemObjects
	Transform itemObjects;

	void Start ()
	{
		itemClass = new ItemClass ();
		savedFiles = new List<string> ();

		output_tileMap = GameObject.Find ("TileMap").GetComponent ("Output_TileMap") as Output_TileMap;

		serv = gameObject.AddComponent<Farts> ();

		Button_Save.onClick.AddListener (() => {
			saveFile (); });
		Button_Load.onClick.AddListener (() => {
			loadFile ();});


		itemObjects = GameObject.Find ("ItemObjects").GetComponent ("Transform") as Transform;

	}

	public void saveFile ()
	{
	
		if (itemClass.getItemList ().Count != 0) {

			BinaryFormatter bf = new BinaryFormatter ();
			MemoryStream stream = new MemoryStream (2048);
			bf.Serialize (stream, itemClass.getItemList ());
			string tmp = System.Convert.ToBase64String (stream.ToArray ());

			levelTHING = serv.newLevel ("stuff", "34534567", "34563456", tmp, "32453245");
			Debug.Log (levelTHING);


		} else if (itemClass.getItemList ().Count == 0) {
			Debug.Log ("ItemClass.itemList is empty. Nothing to write.");
		}

	}
	
	public void loadFile ()
	{

		//for loading list of saved files
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

		//clear items in world and in data structure
		wipeItemObjects ();
		itemClass.clearItemList ();

		//load level data and store in data structure
		BinaryFormatter bf = new BinaryFormatter ();
		string tmp = serv.getLevel ("6211504244260864");
		MemoryStream memoryStream = new MemoryStream (System.Convert.FromBase64String (tmp));
		Debug.Log (" file contents: " + tmp);
		List<ItemClass.ItemStruct> savedFile = (List<ItemClass.ItemStruct>)bf.Deserialize (memoryStream);

		//populate world with level prefabs
		for (int i = 0; i < savedFile.Count; i++) {
			Vector3 pos = new Vector3 (savedFile [i].x, savedFile [i].y, savedFile [i].z);
			Vector3 rot = new Vector3 (savedFile [i].xrot, savedFile [i].yrot, savedFile [i].zrot);
			string name = savedFile [i].item.Substring (0, savedFile [i].item.IndexOf ('_'));
			output_tileMap.instantiateItemObject (name, pos, rot);
		}
	}

	public void wipeItemObjects ()
	{
		foreach (Transform child in itemObjects) {
			GameObject.Destroy (child.gameObject);
		}
		itemClass.resetNameCounter ();
	}
	

	//for loading list of saved levels
	//	void buttonLoadFile(string s){
	//
	//		Debug.Log ("loading: " + s);
	//
	//		wipeItemObjects ();
	//		itemClass.clearItemList ();
	//
	//		BinaryFormatter bf = new BinaryFormatter ();
	//		string tmp = PlayerPrefs.GetString (s);
	//		MemoryStream memoryStream = new MemoryStream (System.Convert.FromBase64String (tmp));
	//		List<ItemClass.ItemStruct> savedFile = (List<ItemClass.ItemStruct>)bf.Deserialize (memoryStream);
	//		
	//		for (int i = 0; i < savedFile.Count; i++) {
	//			Vector3 pos = new Vector3 (savedFile [i].x, savedFile [i].y, savedFile [i].z);
	//			Vector3 rot = new Vector3 (savedFile [i].xrot, savedFile [i].yrot, savedFile [i].zrot);
	//			string name = savedFile [i].item.Substring (0, savedFile [i].item.IndexOf ('_'));
	//			//				tileSelection.placeItems (name, pos);
	//			output_tileMap.instantiateItemObject (name, pos, rot);
	//		}
	//
	//		Transform h = GameObject.Find("FilenameButtons").GetComponent("Transform") as Transform;
	//		foreach (Transform child in h) {
	//			GameObject.Destroy (child.gameObject);
	//		}
	//	}
	
}
