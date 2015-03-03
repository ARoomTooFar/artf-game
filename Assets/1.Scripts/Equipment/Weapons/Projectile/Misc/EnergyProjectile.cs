// EnergyProjectile parent class
//     Things that don't last till they collide

using UnityEngine;
using System.Collections;
using System;

public class EnergyProjectile : MonoBehaviour {
	public int damage;
	public float speed;
	private Character user;
	public Transform target;
	
	protected Type opposition;
	protected float lifeTime, curLifeTime;
	
	// Use this for initialization
	protected virtual void Start() {
		
	}

	public virtual void setInitValues(Character player, Type ene, int dmg,bool effect,BuffsDebuffs hinder) {
		user = player;
		opposition = ene;

		transform.Rotate(Vector3.up * 180);

		damage = dmg;
		speed = 1.0f;
		lifeTime = dmg/100.0f;
		curLifeTime = 0.0f;
	}
	
	// Update is called once per frame
	protected virtual void Update() {
		transform.position = Vector3.MoveTowards (transform.position, target.position, speed);

		if (curLifeTime >= lifeTime) {
			Destroy(gameObject);
		}

		curLifeTime += Time.deltaTime;
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Wall") {
			Destroy(gameObject);
		}
		
		IDamageable<int, Character> component = (IDamageable<int, Character>) other.GetComponent( typeof(IDamageable<int, Character>) );
		Character enemy = (Character) other.GetComponent(opposition);
		if( component != null && enemy != null) {
			enemy.damage(damage, user);
		}
	}
}
