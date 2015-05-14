// AoETargetting class for area of effect things
//     This is used for tracking units within the range of the collider

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public class CVTargeting : MonoBehaviour {
	
	public List<NewCableVine> unitsInRange;
	public List<ContactPoint> bondagepoints;
	
	public void Start() {
		unitsInRange = new List<NewCableVine>();
		bondagepoints = new List<ContactPoint> ();
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
}