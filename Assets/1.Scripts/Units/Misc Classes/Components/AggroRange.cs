// An object with a sphere collider component on enemies
//     Acts as aggro radius check for detecting and aggroing enemies

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AggroRange : MonoBehaviour {

	// public float radius;
	public TestSwordEnemy TSE;

	public List<Character> inRange;
	public Type opposition;

	// Use this for initialization
	void Start () {
		inRange = new List<Character>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		Character enemy = (Character) other.GetComponent(opposition);
		if (enemy != null) {
			inRange.Add(enemy); 
		}
	}

	void OnTriggerExit(Collider other) {
		Character enemy = (Character) other.GetComponent(opposition);
		if (enemy != null) {
			inRange.Remove(enemy);
		}
	}
}
