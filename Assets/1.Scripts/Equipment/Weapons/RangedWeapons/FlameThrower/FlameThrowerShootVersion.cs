using UnityEngine;
using System.Collections;

public class FlameThrowerShootVersion : RangedWeapons {

	protected override void Start () {
		base.Start ();
	}
	protected override void setInitValues() {
		base.setInitValues();
		maxAmmo = 99;
		currAmmo = maxAmmo;
		// Use sword animations for now
		stats.weapType = 0;
		stats.weapTypeName = "sword";
		loadSpeed = 5f;
		stats.atkSpeed = 2.0f;
		stats.damage = 1;
		stats.maxChgTime = 2;
		// Bull Pattern L originally
		//rifle(L,2) + pistol (L,1)
		variance = 22f;
		kick = 15f;
		stats.debuff = new Burning(stats.damage);

		spray = user.transform.rotation;
		spray = Quaternion.Euler(new Vector3(user.transform.eulerAngles.x,Random.Range(-(12f-user.stats.coordination)+user.transform.eulerAngles.y,(12f-user.stats.coordination)+user.transform.eulerAngles.y),user.transform.eulerAngles.z));

	}
	protected void fireFlame(bool activated) {
		Fire newBullet = ((GameObject)Instantiate(projectile, user.transform.position, spray)).GetComponent<Fire>();
		newBullet.setInitValues(user, opposition, particles.startSpeed,activated, stats.debuff);
		if(activated){
			newBullet.damage = (int)particles.startSpeed;
		}else{
			//newBullet.duration = particles.startSpeed/2;
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	public override void initAttack() {
		base.initAttack();
	}
	/*protected void fireFlame() {
		Projectile newBullet = ((GameObject)Instantiate(projectile, user.transform.position, spray)).GetComponent<Projectile>();
		//Debug.Log(newBullet == null);
		newBullet.setInitValues(user, opposition, stats.damage, user.luckCheck(), stats.debuff);
	}*/

	protected override IEnumerator Shoot(int count)
	{
		if(!reload){
			variance = 22f;

			if(count == 0){
				count = 1;
			}

		//High cap for basic is 12f variance, low cap for shotty is 22f
			spray = Quaternion.Euler(new Vector3(user.transform.eulerAngles.x,Random.Range(-(variance-user.stats.coordination)+user.transform.eulerAngles.y,(variance-user.stats.coordination)+user.transform.eulerAngles.y),user.transform.eulerAngles.z));
				StartCoroutine(makeSound(action,playSound,action.length));
				fireFlame(true);
				currAmmo--;
				if(currAmmo<=0){
					reload = true;
					StartCoroutine(loadAmmo());
				}
				yield return StartCoroutine(Wait(.05f));
				spray = Quaternion.Euler(spray.eulerAngles.x,(spray.eulerAngles.y-kick),spray.eulerAngles.z);
				fireFlame(false);
				currAmmo--;
				if(currAmmo<=0){
					reload = true;
					StartCoroutine(loadAmmo());
				}
				spray = Quaternion.Euler(spray.eulerAngles.x,(spray.eulerAngles.y+kick*2),spray.eulerAngles.z);
				fireFlame(false);
				currAmmo--;
				if(currAmmo<=0){
					reload = true;
					StartCoroutine(loadAmmo());
				}
				kick = 15f;
			}
	}
}
