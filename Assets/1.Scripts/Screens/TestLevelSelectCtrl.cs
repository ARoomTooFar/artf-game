using UnityEngine;
using System.Collections;

public class TestLevelSelectCtrl : MonoBehaviour {
	private string test = "1,3,2,4,5";

	// Use this for initialization
	void Start () {
		test.Split (',');
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
