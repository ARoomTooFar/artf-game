// EnergyProjectile parent class
//     Things that don't last till they collide

using UnityEngine;
using System.Collections;
using System;

public class EnergyProjectile : MonoBehaviour {
	public int damage;
	public float speed;
	protected Character user;
	
	public Rigidbody rb;
	protected Type opposition;
	protected float lifeTime, curLifeTime;
	
	// Use this for initialization
	protected virtual void Start() {
		
	}

	public virtual void setInitValues(Character user, Type opposition, int dmg, bool effect, BuffsDebuffs hinder) {
		this.user = user;
		this.opposition = opposition;
		this.rb = this.GetComponent<Rigidbody> ();

		transform.Rotate(Vector3.up * 180);

		damage = dmg;
		speed = 30f;
		lifeTime = dmg/20f;
		curLifeTime = 0.0f;
		
		this.rb.velocity = this.user.facing * this.speed;
		this.rb.isKinematic = false;
	}

	// Update is called once per frame
	protected virtual void Update() {
		if (this.curLifeTime >= this.lifeTime) {
			Destroy(this.gameObject);
		}

		this.curLifeTime += Time.deltaTime;
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Wall") {
			Destroy(this.gameObject);
		}
		
		IDamageable<int, Transform, GameObject> component = (IDamageable<int, Transform, GameObject>) other.GetComponent( typeof(IDamageable<int, Transform, GameObject>) );
		Character enemy = opposition == null ? null : (Character) other.GetComponent(opposition);
		if( component != null && enemy != null) {
			enemy.damage(damage, user.transform, user.gameObject);
		}
	}
}
