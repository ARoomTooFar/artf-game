using UnityEngine;
using System.Collections;

public class Prop : MonoBehaviour, IDamageable<int, Traps> {

	public GameObject expDeath;
	public int health;

	protected bool justSpawned;

	// Use this for initialization
	protected virtual void Start () {
		this.health = 20;
		this.justSpawned = true;

		this.Invoke("notJustSpawned", 0.5f);
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}

	// Using traps since logic for weapons to damage thtings are already in place
	public virtual void damage(int dmgTaken, Traps striker) {
		this.damage (dmgTaken);
	}
	
	public virtual void damage(int dmgTaken) {
		this.health -= dmgTaken;
		if (this.health <= 0) this.die ();
	}

	protected virtual void notJustSpawned() {
		this.justSpawned = false;
	}

	public virtual void die() {
		Instantiate(expDeath, transform.position, transform.rotation);
		Destroy(this.gameObject);
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Wall") {
			this.die ();
		}

		if (collision.gameObject.tag == "Prop" && !this.justSpawned) {
			this.die ();
		}
	}
}
