using UnityEngine;
using System.Collections;

public class Traps : MonoBehaviour {
	
	public int damage;

	// Use this for initialization
	protected virtual void Start () {
		setInitValues ();
	}

	protected virtual void setInitValues() {
		damage = 1;
	}

	protected virtual void FixedUpdate() {
	}

	// Update is called once per frame
	protected virtual void Update () {
	
	}
}
