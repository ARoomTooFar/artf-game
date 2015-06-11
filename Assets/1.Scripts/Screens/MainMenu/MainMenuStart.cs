using UnityEngine;
using System.Collections;

public class MainMenuStart : MonoBehaviour {
	
	void Start () {
		Debug.Log ("GAME START");
		GSManager gsManager = GameObject.Find ("/GSManager").GetComponent<GSManager>();
		gsManager.ClearData ();
	}
}
