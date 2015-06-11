using UnityEngine;
using System.Collections;

public class GOSTimer : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		GSManager gsm = GameObject.Find("GSManager").GetComponent<GSManager>();
		gsm.LoadScene ("MainMenu");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}