using UnityEngine;
using System.Collections;
using System;

public class ExplosiveRound : HomingBullet {	

	public GameObject expDeath;
	public AoETargetting aoe;

	// Use this for initialization
	protected override void Start() {
		base.Start();
	}
	
	public override void setInitValues(Character player, Type opposition, int damage, float partSpeed, bool effect, BuffsDebuffs hinder, float durations) {
		if (opposition == typeof(Enemy) || opposition == typeof(Enemy)) this.aoe.affectEnemies = true;
		if (opposition == typeof(Player)) this.aoe.affectPlayers = true;
		base.setInitValues(player, opposition, damage, partSpeed, effect, hinder, duration);
	}
	
	public override void setInitValues(Character player, Type opposition, int damage, float partSpeed, bool effect, BuffsDebuffs hinder, float durations, Character target) {
		this.target = target;
		this.setInitValues(player, opposition, damage, partSpeed, effect, hinder, duration);
	}
	
	// Update is called once per frame
	protected override void Update() {
		base.Update();
	}

	protected virtual void Explode() {
		// Create explosion while removing self
		// BombExplosion eDeath = ((GameObject)Instantiate(expDeath, transform.position, transform.rotation)).GetComponent<BombExplosion>();
		Instantiate(expDeath, transform.position, expDeath.transform.rotation);
		
		// Variables for sight checking
		RaycastHit[] hits;
		bool inSight;
		
		// For all targets that are within our collider at this point, check that they aren't behind a wall and hit them
		foreach(Character suckers in this.aoe.unitsInRange) {//this.targetsInRange) {
			inSight = true;
			hits = Physics.RaycastAll(this.transform.position,
			                          (suckers.transform.position - this.transform.position).normalized,
			                          Vector3.Distance(this.transform.position, suckers.transform.position));
			
			// Check for walls
			foreach(RaycastHit hit in hits) {
				if (hit.transform.tag == "Wall") inSight = false;
			}
			
			if (inSight) this.onHit(suckers); // Only hits units that aren't behind walls (Add in props and other obstacles in future)
		}
	}

	protected override void CollisionWithThing() {
		this.Explode ();
		base.CollisionWithThing ();
	}

	protected override void OnTriggerEnter(Collider other) {
		if (other.tag == "Wall" || other.tag == "Door" || other.tag == "Prop") {
			if (other.tag == "Prop") other.GetComponent<Prop>().damage(damage);
			this.CollisionWithThing();
			return;
		}
		
		IDamageable<int, Transform, GameObject> component = (IDamageable<int, Transform, GameObject>) other.GetComponent( typeof(IDamageable<int, Transform, GameObject>) );
		Character enemy = (Character) other.GetComponent(this.opposition);
		if( component != null && enemy != null) {
			this.CollisionWithThing();
			return;
		}
		
		IDamageable<int, Traps, GameObject> component2 = (IDamageable<int, Traps, GameObject>) other.GetComponent (typeof(IDamageable<int, Traps, GameObject>));
		if (component2 != null) {
			component2.damage(damage);
			this.CollisionWithThing();
		}
	}
}
