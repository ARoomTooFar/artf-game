using UnityEngine;
using System.Collections;

public class Bullet : Weapons {
	public bool hasHit;
	public Transform target;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		hasHit = false;
		//particles.startSpeed = user.GetComponent<Character>().equip.weapon.GetComponent<Weapons>().particles.startSpeed;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		//target.rotation = new Quaternion(90,0,0,0);
		//target.position = transform.forward *10;
		if(!hasHit){
			Vector3 tarDir = target.position - transform.position;
			Ray ray = new Ray(transform.position, tarDir);
			RaycastHit hit;
			//Debug.DrawRay(transform.position, targetDir,Color.green);
			//move fist towards the target location .35f
			transform.position = Vector3.MoveTowards (transform.position, target.position, .35f);
			if (Physics.Raycast (ray, out hit)) {
				//.85f
				if (hit.distance < .35f) {
					if (hit.collider.tag == "Wall" || hit.collider.tag == "Enemy") {
							hasHit = true;
					}
				}
			}
		}else{
			///*if(gameObject.tag != "Shotgun1" && gameObject.tag != "CShotgun1")
				//AudioSource.PlayClipAtPoint (sound, transform.position);

			Destroy(this.transform.parent.gameObject);
		}
	}
}
