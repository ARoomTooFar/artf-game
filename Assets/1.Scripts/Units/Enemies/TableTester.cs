using UnityEngine;
using System.Collections;

public class TableTester : MonoBehaviour {

	private AggroTable test;
	private GameObject p1;
	private GameObject p2;
	private GameObject p3;
	private GameObject p4;

	// Use this for initialization
	void Start () 
	{
		test = new AggroTable ();
		test.add (p1, 10);
		test.add (p2, 4);
		test.add (p3, 21);
		test.add (p4, 14);
		test.add (p3, 20);
		test.printOrder ();
	}
	
	// Update is called once per frame
	void Update () {

	}
}
