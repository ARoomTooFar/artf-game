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

	protected void cleanList() {
		int count = 0;
		while (count < unitsInRange.Count) {
			if (unitsInRange[count] == null) {
				unitsInRange.RemoveAt(count);
				continue;
			}
			count++;
		}
		if (this.unitsInRange.Count == 0) this.CancelInvoke();
	}

	// Will send a message to current object and all parents that something entered for instant response (Look at Stun trap)
	//     Take care not to have multiple parents with the unit entered method unless it is what you want
	//     "unitEntered" with a Character paramter is the method name you want for response
	void OnTriggerEnter(Collider other) {
		if ((affectEnemies && (other.tag == "Enemy")) || (affectPlayers && other.tag.Substring(0, other.tag.Length - 1) == "Player")) {
			Character unit = other.GetComponent<Character>();
			if (unitsInRange.Count == 0 && !unit.invis) this.InvokeRepeating("cleanList", 3.0f, 3.0f);
			unitsInRange.Add(unit);
			this.gameObject.SendMessageUpwards("unitEntered", unit, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	// Same as above, but for when stuff leave the area
	void OnTriggerExit(Collider other) {
		Character unit = other.GetComponent<Character>();
		
		if (unit != null) {
			unitsInRange.Remove (unit);
			if (this.unitsInRange.Count == 0) this.CancelInvoke();
			this.gameObject.SendMessageUpwards("unitLeft", unit, SendMessageOptions.DontRequireReceiver);
		}
	}
}