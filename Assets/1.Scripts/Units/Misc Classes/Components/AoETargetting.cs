// AoETargetting class for area of effect things
//     This is used for tracking units within the range of the collider

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class AoETargetting : MonoBehaviour {
	
	public bool affectPlayers;
	public bool affectEnemies;
	public List<Character> unitsInRange;

	public void Start() {
		unitsInRange = new List<Character>();
	}


	// Will send a message to current object and all parents that something entered for instant response (Look at Stun trap)
	//     Take care not to have multiple parents with the unit entered method unless it is what you want
	//     "unitEntered" with a Character paramter is the method name you want for response
	void OnTriggerEnter(Collider other) {
		if ((affectPlayers && other.GetComponent<Player>() != null) || (affectEnemies && other.GetComponent<Enemy>() != null)) {
			Character unit = other.GetComponent<Character>();
			unitsInRange.Add(unit);
			this.gameObject.SendMessageUpwards("unitEntered", unit, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	void OnTriggerExit(Collider other) {
		Character unit = other.GetComponent<Character>();
		
		if (unit != null) {
			unitsInRange.Remove (unit);
			this.gameObject.SendMessageUpwards("unitLeft", unit, SendMessageOptions.DontRequireReceiver);
		}
	}
}