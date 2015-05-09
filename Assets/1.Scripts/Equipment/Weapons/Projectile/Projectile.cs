// Projectile parent class
//     For now, it just goes straight

using UnityEngine;
using System.Collections;
using System;

public class Projectile : MonoBehaviour {
	public ParticleSystem particles;

	protected Rigidbody rb;
	protected int damage;
	protected float speed, duration;
	protected bool castEffect;
	protected Character user;
	protected BuffsDebuffs debuff;
	protected Type opposition;

	// Use this for initialization
	protected virtual void Start() {
	}

	public virtual void setInitValues(Character player, Type ene, int damage, float partSpeed, bool effect, BuffsDebuffs hinder, float duration) {
		this.rb = this.GetComponent<Rigidbody> ();

		this.user = player;
		this.opposition = ene;
		this.damage = damage;
		this.speed = 40f;
		this.castEffect = effect;
		this.debuff = hinder;
		this.duration = duration;
		this.particles.startSpeed = partSpeed;
		this.particles.Play();
		this.rb.velocity = this.user.facing * this.speed;

		transform.Rotate(Vector3.right * 90);

		this.rb.isKinematic = false;
	}

	// Update is called once per frame
	protected virtual void Update() {

	}
	
	protected virtual void onHit(Character enemy) {
		if(this.castEffect && this.debuff != null){
			enemy.BDS.addBuffDebuff(this.debuff, this.user.gameObject, this.duration);
		}
		enemy.damage(damage, user.transform, user.gameObject);
	}

	protected virtual void CollisionWithThing() {
		this.particles.Stop();
		Destroy(this.gameObject);
	}

	protected virtual void OnTriggerEnter(Collider other) {
		if (other.tag == "Wall" || other.tag == "Door" || other.tag == "Prop") {
			if (other.tag == "Prop") other.GetComponent<Prop>().damage(damage);
			this.CollisionWithThing();
			return;
		}

		IDamageable<int, Transform, GameObject> component = (IDamageable<int, Transform, GameObject>) other.GetComponent( typeof(IDamageable<int, Transform, GameObject>) );
		Character enemy = (Character) other.GetComponent(this.opposition);
		if( component != null && enemy != null) {
			onHit(enemy);
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
