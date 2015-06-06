using UnityEngine;
using System.Collections;

public class Prop : MonoBehaviour, IDamageable<int, Traps, GameObject> {

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

	//------------------//
	// Damage Functions //
	//------------------//

	public virtual void damage(int dmgTaken, Traps atkPosition, GameObject source) {
		this.damage (dmgTaken);
	}

	// Using traps since logic for weapons to damage thtings are already in place
	public virtual void damage(int dmgTaken, Traps atkPosition) {
		this.damage (dmgTaken);
	}
	
	public virtual void damage(int dmgTaken) {
		this.health -= dmgTaken;
		if (this.health <= 0) this.die ();
	}

	public virtual void die() {
		if(transform.parent !=null){
			Destroy(transform.parent.gameObject);
		}
		Destroy(this.gameObject);
		// Instantiate(expDeath, transform.position, transform.rotation);
	}
	
	//--------------------//

	protected virtual void notJustSpawned() {
		this.justSpawned = false;
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
