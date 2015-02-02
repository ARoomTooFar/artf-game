using UnityEngine;
using System.Collections;

public class Pistol : RangedWeapons {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	protected override void setInitValues() {
		base.setInitValues();

		// Use sword animations for now
		stats.weapType = 0;
		stats.weapTypeName = "sword";

		stats.atkSpeed = 2.0f;
		stats.damage = 1;
		stats.maxChgTime = 2.0f;
		
		// Bull Pattern L originally
		//rifle(L,2) + pistol (L,1)
		variance = 22f;
		kick = 2f;

		spray = user.transform.rotation;
		spray = Quaternion.Euler(new Vector3(user.transform.eulerAngles.x,Random.Range(-(12f-user.stats.coordination)+user.transform.eulerAngles.y,(12f-user.stats.coordination)+user.transform.eulerAngles.y),user.transform.eulerAngles.z));

	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	public override void initAttack() {
		base.initAttack();
	}

	protected override IEnumerator Shoot(int count)
	{
		variance = 22f;

		if(count == 0){
			count = 1;
		}

		//High cap for basic is 12f variance, low cap for shotty is 22f
		spray = Quaternion.Euler(new Vector3(user.transform.eulerAngles.x,Random.Range(-(variance-user.stats.coordination)+user.transform.eulerAngles.y,(variance-user.stats.coordination)+user.transform.eulerAngles.y),user.transform.eulerAngles.z));
		for (int i = 0; i < count; i++) {
			StartCoroutine(makeSound(action,playSound,action.length));
			yield return StartCoroutine(Wait(.08f));
				
			//Instantiate(projectile, user.transform.position, spray);
				
			bullet = (GameObject) Instantiate(projectile, user.transform.position, spray);
			//bullet.transform.parent = gameObject.transform;
			bullet.GetComponentInChildren<Bullet>().damage = 1;
			bullet.GetComponentInChildren<Bullet>().speed = .5f;
			bullet.GetComponentInChildren<Bullet>().particles.startSpeed = particles.startSpeed;
			bullet.GetComponentInChildren<Bullet>().player = user;
			/*shots.Add(bullet);
			foreach (Shot bull in shots){
				bull.facing = spray.eulerAngles;
			}*/
			//variance -= 1;
			if(stats.weapType == 1){
				spray = Quaternion.Euler(spray.eulerAngles.x,(spray.eulerAngles.y+Random.Range(-kick,kick)),spray.eulerAngles.z);
			}
			if(stats.weapType == 2){
				spray = Quaternion.Euler(spray.eulerAngles.x,(spray.eulerAngles.y+Random.Range(-kick,kick)),spray.eulerAngles.z);
			}
			kick += .2f;
			if(kick >= 5f){
				kick = 2f;
			}
		}
	}
}
