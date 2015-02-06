using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	public int damage;
	public float speed;
	private Character user;
	public ParticleSystem particles;
	public Transform target;
	// Use this for initialization
	protected virtual void Start() {

	}
	public virtual void setInitValues(Character player, float partSpeed) {
		user = player;

		transform.Rotate(Vector3.right * 90);

		damage = 1;
		speed = 0.5f;

		particles.startSpeed = partSpeed;
		particles.Play();
	}

	// Update is called once per frame
	protected virtual void Update() {
		transform.position = Vector3.MoveTowards (transform.position, target.position, speed);
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Wall") {
			particles.Stop();
			Destroy(gameObject);
		}

		IDamageable<int, Character> component = (IDamageable<int, Character>) other.GetComponent( typeof(IDamageable<int, Character>) );
		Enemy enemy = other.GetComponent<Enemy>();
		if( component != null && enemy != null) {
			enemy.damage(damage, user);
			particles.Stop();
			Destroy(gameObject);
		}
	}
}
