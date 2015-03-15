using UnityEngine;
using System.Collections;

public class Prop : Wall, IDamageable<int, Character> {
	public GameObject expDeath;
	public int health;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public virtual void damage(int dmgTaken, Character striker) {
		//if(striker.ge
	}
	
	public virtual void damage(int dmgTaken) {
		/*if (!invincible) {
			print("UGH!" + dmgTaken);
			stats.health -= greyTest(dmgTaken);
			UI.greyBar.current = greyDamage+stats.health;
			if (stats.health <= 0) {
				
				die();
			}
			UI.hpBar.current = stats.health;
		}*/
	}

	public virtual void die() {
		/*base.die();
		Renderer[] rs = GetComponentsInChildren<Renderer>();
		Explosion eDeath = ((GameObject)Instantiate(expDeath, transform.position, transform.rotation)).GetComponent<Explosion>();
		eDeath.setInitValues(this, true);
		foreach (Renderer r in rs) {
			r.enabled = false;
		}*/
	}
}
