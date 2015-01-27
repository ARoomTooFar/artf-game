using UnityEngine;
using System.Collections;

public class Shot : Projectile {
	public Vector3 facing;
	protected override void Start() {
		base.Start();
	}
	protected override void setInitValues() {
		base.setInitValues();
		damage = 1;
		speed = 5f;
		//facing = new Vector3(transform.rotation.x,transform.rotation.y,transform.rotation.z);
	}

	// Update is called once per frame
	protected override void Update() {
		rigidbody.velocity = facing * speed;
	}
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Wall") {
			particles.Stop();
			Destroy(this.gameObject);
		}

		IDamageable<int> component = (IDamageable<int>) other.GetComponent( typeof(IDamageable<int>) );
		Enemy enemy = other.GetComponent<Enemy>();
		if( component != null && enemy != null) {
			enemy.damage(damage);
			particles.Stop();
			Destroy(this.gameObject);
		}
	}
}
