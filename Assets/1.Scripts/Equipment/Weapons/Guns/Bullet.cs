using UnityEngine;
using System.Collections;

public class Bullet : Gun {
	public bool hasHit;
	public Transform target;
	public Equipment gun;
	
	// Use this for initialization
	protected override void Start() {
		hasHit = false;
		setInitValues();
		//particles.startSpeed = user.GetComponent<Character>().equip.weapon.GetComponent<Weapons>().particles.startSpeed;
	}
	protected override void setInitValues() {
		stats.damage = 1;
		particles.startSpeed = gun.particles.startSpeed;
		particles.Play();
	}
	// Update is called once per frame
	protected override void Update() {
		//target.rotation = new Quaternion(90,0,0,0);
		//target.position = transform.forward *10;
		if(!hasHit){
			Vector3 tarDir = transform.position - target.position;
			Ray ray = new Ray(transform.position, -tarDir);
			RaycastHit hit;
			//Debug.DrawRay(ray);
			Debug.DrawRay(transform.position, -tarDir,Color.green);
			//move fist towards the target location .35f
			transform.position = Vector3.MoveTowards (transform.position, target.position, .55f);
			if (Physics.Raycast (ray, out hit)) {
				//.85f
				if (hit.distance < .35f) {
					if (hit.collider.tag == "Enemy") {
							IDamageable<int> component = (IDamageable<int>) hit.collider.GetComponent( typeof(IDamageable<int>) );
							Enemy enemy = hit.collider.GetComponent<Enemy>();
							if( component != null && enemy != null) {
								enemy.damage(stats.damage);
							}
							hasHit = true;
					}
					if(hit.collider.tag == "Wall"){
						hasHit = true;
					}
				}
			}
		}else{
			///*if(gameObject.tag != "Shotgun1" && gameObject.tag != "CShotgun1")
				//AudioSource.PlayClipAtPoint (sound, transform.position);
			particles.Stop();
			Destroy(this.transform.parent.gameObject);
		}
	}
	/*void OnCollisionEnter(Collision other){
		for(int count = 0; count < collision.contact.list
		ContactPoint contact = collision.contact[0];
		print("Booty");
		if(other.gameObject.tag == "Wall"){
			print("Butt");
			hasHit = true;
		}
	}*/
}
