using UnityEngine;
using System.Collections;

public class TrapRegenerator : MonoBehaviour, IDamageable<int, Traps, GameObject> {

	public bool regenerable;
	public int regenTime;
	public GameObject trapEffects;

	public int maxHealth;
	public int health;

	protected Animator anim;

	// Use this for initialization
	void Start () {
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//------------------//
	// Damage Functions //
	//------------------//

	public virtual void damage(int dmgTaken, Traps atkPosition, GameObject source) {
		this.damage(dmgTaken);
	}

	public virtual void damage(int dmgTaken, Traps atkPosition) {
		this.damage(dmgTaken);
	}

	public virtual void damage(int dmgTaken) {
		this.health -= dmgTaken;

		if (this.health <= 0) this.die();
	}

	public void die() {
		if (regenerable) {
			trapEffects.SetActive(false);

			this.GetComponent<Collider>().enabled = false;
			
			StartCoroutine(regen());
			this.GetComponent<Animator>().SetBool("Killed", true);
		}
	}
	
	//----------------------//
	
	public void rez(){
	}
	public void heal(int d){
	}

	private void regenerate() {
		trapEffects.SetActive(true);
		this.GetComponent<Collider>().enabled = true;

		health = maxHealth;
		this.GetComponent<Animator>().SetBool("Killed", false);
	}

	private IEnumerator regen() {
		float time = 0.0f;
		while (time < regenTime) {
			time += Time.deltaTime;
			yield return null;
		}
		regenerate();
	}
}
