using UnityEngine;
using System.Collections;

public class Bullet : Gun {
	public Transform target;
	public Equipment gun;
	
	// Use this for initialization
	protected override void Start() {
		setInitValues();
	}
	protected override void setInitValues() {
		stats.damage = 1;
		// particles.startSpeed = gun.particles.startSpeed;
		particles.Play();
	}

	// Update is called once per frame
	protected override void Update() {
		transform.position = Vector3.MoveTowards (transform.position, target.position, .35f);
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Wall") {
			particles.Stop();
			Destroy(this.transform.parent.gameObject);
		}

		IDamageable<int> component = (IDamageable<int>) other.GetComponent( typeof(IDamageable<int>) );
		Enemy enemy = other.GetComponent<Enemy>();
		if( component != null && enemy != null) {
			enemy.damage(stats.damage);
			particles.Stop();
			Destroy(this.transform.parent.gameObject);
		}
	}
}
