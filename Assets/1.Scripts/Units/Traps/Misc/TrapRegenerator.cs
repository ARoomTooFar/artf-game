using UnityEngine;
using System.Collections;

public class TrapRegenerator : MonoBehaviour, IDamageable<int, Traps> {

	public bool regenerable;
	public int regenTime;
	public GameObject trapEffects;

	public int maxHealth;
	public int health;

	// Use this for initialization
	void Start () {
		health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void damage(int dmgTaken, Traps atkPosition) {
		damage(dmgTaken);
	}

	public void damage(int dmgTaken) {
		health -= dmgTaken;

		if (health <= 0) {
			die();
		}
	}

	public void die() {
		if (regenerable) {
			trapEffects.SetActive(false);

			this.GetComponent<Collider>().enabled = false;
			
			StartCoroutine(regen());
		}
	}
	public void rez(){
	}
	public void heal(int d){
	}

	private void regenerate() {
		trapEffects.SetActive(true);
		this.GetComponent<Collider>().enabled = true;

		health = maxHealth;
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
