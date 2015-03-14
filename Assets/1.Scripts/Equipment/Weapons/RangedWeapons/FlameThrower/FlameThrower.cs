using UnityEngine;
using System.Collections;

public class FlameThrower : RangedWeapons {
	public float lastDmgTime, curDuration, maxDuration;
	public Fire currFire, oneFalse, twoFalse;
	protected override void Start () {
		base.Start ();
	}
	protected override void setInitValues() {
		base.setInitValues();
		maxAmmo = 99;
		currAmmo = maxAmmo;
		// Use sword animations for now
		stats.weapType = 2;
		stats.weapTypeName = "chainsaw";
		stats.atkSpeed = 3.0f;
		loadSpeed = 5f;
		stats.damage = 1;
		stats.maxChgTime = 3.0f;
		stats.timeForChgAttack = 0.5f;
		
		stats.chgLevels = 0.5f;

		lastDmgTime = 0.0f;
		maxDuration = 5.0f;
		curDuration = 0.0f;
		// Bull Pattern L originally
		//rifle(L,2) + pistol (L,1)
		variance = 22f;
		kick = 15f;
		stats.debuff = new Burning(stats.damage);

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
	/*protected void fireFlame() {
		Projectile newBullet = ((GameObject)Instantiate(projectile, user.transform.position, spray)).GetComponent<Projectile>();
		//Debug.Log(newBullet == null);
		newBullet.setInitValues(user, opposition, stats.damage, user.luckCheck(), stats.debuff);
	}*/

	protected override IEnumerator bgnCharge() {
		if (user.animator.GetBool("Charging")) particles.Play();
		while (user.animator.GetBool("Charging") && stats.curChgDuration < stats.timeForChgAttack) {
			stats.curChgDuration = Mathf.Clamp(stats.curChgDuration + Time.deltaTime, 0.0f, stats.maxChgTime);
			stats.chgDamage = (int) (stats.curChgDuration/stats.chgLevels);
			particles.startSpeed = stats.chgDamage;
			yield return null;
		}
		attack ();
	}

	protected override void attack() {
		base.attack ();
	}

	protected override void basicAttack() {
		print("Charged Attack; Power level:" + stats.chgDamage);
		user.animator.SetBool("ChargedAttack", false);
		//this.GetComponent<Collider>().enabled = true;
		//Add fire production here
		spray = Quaternion.Euler(new Vector3(user.transform.eulerAngles.x,Random.Range(-(variance-user.stats.coordination)+user.transform.eulerAngles.y,(variance-user.stats.coordination)+user.transform.eulerAngles.y),user.transform.eulerAngles.z));
		currFire = ((GameObject)Instantiate(projectile, user.transform.position, spray)).GetComponent<Fire>();
		currFire.setInitValues(user, opposition, .5f,true, stats.debuff);
		currFire.damage = 1;
		StartCoroutine(atkFinish());
	}

	protected override void chargedAttack() {
		print("Charged Attack; Power level:" + stats.chgDamage);
		user.animator.SetBool("ChargedAttack", true);
		//this.GetComponent<Collider>().enabled = true;
		//Add start of fire here
		StartCoroutine(flaming());
		StartCoroutine(atkFinish());
	}

	// Logic for when we have our chainsaw out and sawing some bitch ass plants
	protected virtual IEnumerator flaming() {
		curDuration = maxDuration;
		lastDmgTime = Time.time;
		spray = Quaternion.Euler(new Vector3(user.transform.eulerAngles.x,Random.Range(-(variance-user.stats.coordination)+user.transform.eulerAngles.y,(variance-user.stats.coordination)+user.transform.eulerAngles.y),user.transform.eulerAngles.z));
		currFire = ((GameObject)Instantiate(projectile, user.transform.position, spray)).GetComponent<Fire>();
		currFire.setInitValues(user, opposition, .6f,true, stats.debuff);
		while(user.animator.GetBool("Charging") && curDuration > 0) {
			stats.curChgDuration = Mathf.Clamp(stats.curChgDuration + Time.deltaTime, 0.0f, stats.maxChgTime);
			stats.chgDamage = (int) (stats.curChgDuration/stats.chgLevels);
			currFire.damage = stats.chgDamage;
			particles.startSpeed = stats.chgDamage;
			curDuration -= Time.deltaTime;
			if (Time.time - lastDmgTime >= 0.6f/stats.curChgDuration) {
				lastDmgTime = Time.time;
			}

			yield return null;
		}
		user.animator.SetBool("ChargedAttack", false);
	}
	
	// When our attack swing finishes, remove colliders, particles, and other stuff
	//     * Consider one more co routine after to check for when our animation is completely done
	protected override IEnumerator atkFinish() {
		while (user.animSteInfo.nameHash != user.atkHashEnd) {
			yield return null;
		}
		
		particles.Stop();
		//this.GetComponent<Collider>().enabled = false;
		//Finish the duration on the fire 
		/*if (chained.Count > 0) {
			foreach(Character meat in chained) {
				meat.BDS.rmvBuffDebuff(debuff, user.gameObject);
			}
			chained.Clear();
			user.BDS.rmvBuffDebuff(debuff, user.gameObject);
		}
		cropping.Clear();*/
		if(currFire != null){
			currFire.StartCoroutine("Wait",.5f);
			currFire = null;
		}
		user.animator.speed = 1.0f;
		//currFire = null;
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
