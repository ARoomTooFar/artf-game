// Like AoE targetting but for moving away from walls and units

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class FlockDar : MonoBehaviour {

	public List<Collider> objectsInRange;

	protected Enemy user;
	
	public void Start() {}
	
	public virtual void InstantiateFlockingDar(Enemy user) {
		this.user = user;
		this.objectsInRange = new List<Collider>();
	}
	
	
	protected void CleanList() {
		int count = 0;
		while (count < this.objectsInRange.Count) {
			if (this.objectsInRange[count] == null) {
				this.objectsInRange.RemoveAt(count);
				continue;
			}
			count++;
		}
		if (this.objectsInRange.Count == 0) this.CancelInvoke();
	}
	
	protected virtual void OnTriggerEnter(Collider other) {
		if (other.tag == "Wall" || other.tag == "Enemy") {
			if (this.objectsInRange.Count == 0) this.InvokeRepeating("CleanList", 3.0f, 3.0f);
			this.objectsInRange.Add(other);
		} else if (other.GetComponent<Player>() != null) {
			if (other.gameObject != this.user.target) {
				if (this.objectsInRange.Count == 0) this.InvokeRepeating("CleanList", 3.0f, 3.0f);
				this.objectsInRange.Add(other);
			}
		}
	}
	
	// Same as above, but for when stuff leave the area
	protected virtual void OnTriggerExit(Collider other) {
		this.objectsInRange.Remove (other);
		if (this.objectsInRange.Count == 0) this.CancelInvoke();
	}
}