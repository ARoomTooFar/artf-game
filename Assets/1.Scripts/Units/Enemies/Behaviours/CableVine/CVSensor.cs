using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CVSensor : MonoBehaviour {

	private bool hooked;
	public List<Player> playersHooked;
	private bool walled;

	void Start () {
		playersHooked = new List<Player> ();
	}

	void OnTriggerEnter(Collider other) {

		Player p = other.GetComponent<Player> ();

		if (p != null) {
			hooked = true;
			playersHooked.Add(p);
			return;
		}

		Wall w = other.GetComponent<Wall> ();

		if (w != null) {
			walled = true;
		}
	}

	void OnTriggerExit(Collider other) {
		playersHooked.Remove (other.GetComponent<Player> ());
		if(playersHooked.Count == 0) hooked = false;
		Wall w = other.GetComponent<Wall> ();
		if (w != null) {
			walled = false;
		}
		
	}

	public bool Hooked() {
		return hooked;
	}

	public bool Walled() {
		return walled;
	}


}
