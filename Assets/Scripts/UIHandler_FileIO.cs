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
	public Button Button_Save = null;
	public Button Button_Deploy = null;
	public Button Button_Load = null;
	MouseHandler_TileSelection tileSelection;
//	Dictionary<string, Vector3> savedState;
	DataHandler_Items data;
	BinaryWriter bin;
	private StreamWriter writer; // This is the writer that writes to the file
	private string assetText;
	ItemClass itemClass = new ItemClass ();
	private Farts serv;
	long levelId;

	//object in hierarchy that holds itemObjects
	Transform itemObjects;

	void Start ()
	{

		serv = gameObject.AddComponent<Farts> ();
		Button_Save.onClick.AddListener (() => {
			saveFile (); });
		Button_Load.onClick.AddListener (() => {
			loadFile ();});


		tileSelection = GameObject.Find ("TileMap").GetComponent ("MouseHandler_TileSelection") as MouseHandler_TileSelection;
//		savedState = new Dictionary<string, Vector3> ();

		data = GameObject.Find ("ItemObjects").GetComponent ("DataHandler_Items") as DataHandler_Items;

		itemObjects = GameObject.Find ("ItemObjects").GetComponent ("Transform") as Transform;

	}

	public void saveFile ()
	{

		
//		BinaryFormatter bf = new BinaryFormatter ();
//		FileStream file = File.Create ("Assets/Resources/savedLevel.txt");
		if (itemClass.getItemList ().Count != 0) {
//			bf.Serialize (file, itemClass.getItemList ());
//			File.WriteAllText("Assets/Resources/savedLevel.txt", bf.Serialize (file, itemClass.getItemList ()));






//			string hi = "";
//			StreamWriter outStream = new StreamWriter(hi);
//			bf.Serialize (outStream, itemClass.getItemList ());


//			Debug.Log (outStream);


			BinaryFormatter bf = new BinaryFormatter ();
			MemoryStream stream = new MemoryStream (2048);
//			ASCIIEncoding charEncoding = new ASCIIEncoding ();
			bf.Serialize (stream, itemClass.getItemList ());
			string tmp = System.Convert.ToBase64String (stream.ToArray ());
			PlayerPrefs.SetString ("levelData", tmp);
//			Debug.Log (PlayerPrefs.GetString("levelData"));





//			MemoryStream m = new MemoryStream();
//			if (itemClass.getItemList () != null)
//			{
//				BinaryFormatter b = new BinaryFormatter();
//				b.Serialize(m, itemClass.getItemList ());
//			}
//
//
//			ASCIIEncoding charEncoding = new ASCIIEncoding ();
//			byte[] byteArray;
//			byteArray = new byte[m.Length];
//			for(int i = 0; i < m.Length; i++){
//				byteArray[i] = Convert.ToByte (m.ReadByte ());
//			}
//			char[] charArray;
//			charArray = new char[charEncoding.GetCharCount (byteArray, 0, byteArray.Length)];






//			Debug.Log (builder.ToString());

//			int count = 0;
//			byte[] byteArray;
//			char[] charArray;
//			byteArray = new byte[stream.Length];
//			while (count < stream.Length)
//				byteArray[count++] = Convert.ToByte (stream.ReadByte ());
//			charArray = new char[charEncoding.GetCharCount (byteArray, 0, count)];
//			charEncoding.GetDecoder ().GetChars (byteArray, 0, count, charArray, 0);


//			string levelData = bf.Serialize (file, itemClass.getItemList ());
//			levelId = serv.newLevel(123, "Level Name", levelData);
		} else {
			Debug.Log ("ItemClass.itemList is empty. Nothing to write.");
		}
//		file.Close ();

//		if (data.getItemDictionary ().Count != 0) {
//			savedState.Clear ();
//			savedState = new Dictionary<string, Vector3> (data.getItemDictionary ());
//		} else {
//			Debug.Log ("Nothing to save");
//		}
	}

	public void loadFile ()
	{

//		if (File.Exists ("Assets/Resources/savedLevel.txt")) {
//			data.wipeItemObjects ();
			wipeItemObjects ();

//			data.clearItemDictionary ();
			itemClass.clearItemList ();


//			TextAsset textFile = new TextAsset();
//			textFile = Resources.Load("savedLevel", typeof(TextAsset)) as TextAsset;
//			StreamReader inp_stm = new StreamReader("Assets/Resources/savedLevel.txt");
//			string inp_ln = "";
			
//			while(!inp_stm.EndOfStream)
//			{
//				inp_ln = inp_stm.ReadLine( );
//				// Do Something with the input. 
//			}

			BinaryFormatter bf = new BinaryFormatter ();
			string tmp = PlayerPrefs.GetString ("levelData", string.Empty);
			MemoryStream memoryStream = new MemoryStream (System.Convert.FromBase64String (tmp));
			List<ItemClass.ItemStruct> savedFile = (List<ItemClass.ItemStruct>)bf.Deserialize (memoryStream);

//			string levelData = "";
//			levelData = serverConnect.getLevel(levelId); //string is level ID

			
//			inp_stm.Close( ); 


//			BinaryFormatter bf = new BinaryFormatter ();
//			FileStream file = File.Open ("Assets/Resources/savedLevel.txt", FileMode.Open);
//			List<ItemClass.ItemStruct> savedFile = (List<ItemClass.ItemStruct>)bf.Deserialize (file);
//			file.Close ();

//			Debug.Log ((List<ItemClass.ItemStruct>)bf.Deserialize);

			//download levelData from server
			//			string levelData = "";
			//			levelData = serverConnect.getLevel(levelId); //string is level ID

			for (int i = 0; i < savedFile.Count; i++) {
				Vector3 pos = new Vector3 (savedFile [i].x, savedFile [i].y, savedFile [i].z);
				string name = savedFile [i].item.Substring (0, savedFile [i].item.IndexOf ('_'));
				tileSelection.placeItems (name, pos);
			}
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
