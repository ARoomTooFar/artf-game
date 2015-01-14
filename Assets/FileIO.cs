using UnityEngine;
using System.Collections;
using System;
using System.IO; 
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FileIO : MonoBehaviour
{
		[SerializeField]
		private Button
				Button_Open = null; // assign in the editor
	
		[SerializeField]
		private Button
				Button_Save = null; // assign in the editor

		void Start ()
		{
				Button_Open.onClick.AddListener (() => {
						openFile (); });
				Button_Save.onClick.AddListener (() => {
						saveFile ();});
		}

		public void saveFile ()
		{
				//StartCoroutine (getURL());
//				AppendString ("i like pie: gg" + testnum++ + inputguy.text);
//				inputguy.text = Application.dataPath;
		}

		public void openFile ()
		{
				//Debug.Log("hio");
				//AssetDatabase.Refresh();


//			assetText = asset.text;
//			Debug.Log(assetText);
		}
		

}
