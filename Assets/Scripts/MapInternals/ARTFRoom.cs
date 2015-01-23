using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ARTFRoom {
	
	private Vector3 LLposition = new Vector3 ();
	
	public Vector3 LLPosition {
		get {
			return LLposition;
		}
	}
	
	private Vector3 URposition = new Vector3 ();
	
	public Vector3 URPosition {
		get {
			return URposition;
		}
	}
	
	public string SaveString{
		get{ return LLposition.toCSV () + "," + URposition.toCSV ();}
	}
	
	/*
	 * Constructor
	 */
	public ARTFRoom (Vector3 llpos, Vector3 urpos) {
		this.LLposition = llpos;
		this.URposition = urpos;
	}
}