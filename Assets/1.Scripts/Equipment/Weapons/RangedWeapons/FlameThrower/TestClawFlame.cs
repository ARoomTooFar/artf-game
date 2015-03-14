using UnityEngine;
using System.Collections;

public class TestClawFlame : FlameThrower {
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Used for setting sword stats for each equipment piece
	protected override void setInitValues() {
		base.setInitValues();
		//stats.atkSpeed = 2.0f;
		//stats.damage = 1;
		//stats.maxChgTime = 2.0f;
		
		//spray = user.transform.rotation;
		//spray = Quaternion.Euler(new Vector3(user.transform.eulerAngles.x,Random.Range(-(12f-user.stats.coordination)+user.transform.eulerAngles.y,(12f-user.stats.coordination)+user.transform.eulerAngles.y),user.transform.eulerAngles.z));
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	protected override IEnumerator flaming() {
		curDuration = maxDuration;
		lastDmgTime = Time.time;
		currFire = ((GameObject)Instantiate(projectile, user.transform.position, spray)).GetComponent<Fire>();
		currFire.setInitValues(user, opposition, .6f,true, stats.debuff);
		while(user.animator.GetBool("Charging") && curDuration > 0) {
			stats.curChgDuration = Mathf.Clamp(stats.curChgDuration + Time.deltaTime, 0.0f, stats.maxChgTime);
			if(stats.curChgDuration >= 1.6f && stats.curChgDuration <= 1.7f){
				//I like the effect without the pause, but hey, I'll demo it another time <3
				if(oneFalse ==null){
					spray = Quaternion.Euler(spray.eulerAngles.x,(spray.eulerAngles.y-kick),spray.eulerAngles.z);
					oneFalse = ((GameObject)Instantiate(projectile, user.transform.position, spray)).GetComponent<Fire>();
					oneFalse.setInitValues(user, opposition, .6f,true, stats.debuff);
					oneFalse.damage = 0;
				}
				if(twoFalse ==null){
					spray = Quaternion.Euler(spray.eulerAngles.x,(spray.eulerAngles.y+kick*2),spray.eulerAngles.z);
					twoFalse = ((GameObject)Instantiate(projectile, user.transform.position, spray)).GetComponent<Fire>();
					twoFalse.setInitValues(user, opposition, .6f,true, stats.debuff);
					twoFalse.damage = 0;
				}
			}
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
	
	public override void initAttack() {
		base.initAttack();
	}
}
