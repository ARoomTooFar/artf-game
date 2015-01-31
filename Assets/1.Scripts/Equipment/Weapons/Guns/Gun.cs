using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gun : Weapons {
	//L = line, S = Shotty/Sprayheavy
	public char bullPattern;
	//for inaccuracy
	public Quaternion spray;
	public float variance;
	public float kick;
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	protected override void setInitValues() {
		base.setInitValues();
		stats.atkSpeed = 2.0f;
		stats.damage = 1;
		stats.maxChgTime = 2.0f;
		stats.weapType = 1;
		
		bullPattern = 'S';
		if(bullPattern == 'L'){
			//rifle(L,2) + pistol (L,1)
			variance = 22f;
			kick = 2f;
		}
		if(bullPattern == 'M'){
			//machine gun
			variance = 32f;
			kick = 5f;
		}
		if(bullPattern == 'S'){
			//shotty
			variance = 47f;
		}
		spray = user.transform.rotation;
		spray = Quaternion.Euler(new Vector3(user.transform.eulerAngles.x,Random.Range(-(12f-user.stats.coordination)+user.transform.eulerAngles.y,(12f-user.stats.coordination)+user.transform.eulerAngles.y),user.transform.eulerAngles.z));
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

	protected override void attack() {
		if (!Input.GetKey(user.controls.attack) && stats.curChgAtkTime != -1) {
			stats.curChgAtkTime = -1;
			StartCoroutine(Shoot((int)(stats.curChgDuration/stats.chgLevels)));
			print("Charge Attack power level:" + (int)(stats.curChgDuration/stats.chgLevels));
		} else if (stats.curChgAtkTime == 0 && user.animSteInfo.normalizedTime > stats.colStart) {
			StartCoroutine(makeSound(charge,playSound,charge.length));
			stats.curChgAtkTime = Time.time;
			particles.startSpeed = 0;
			particles.Play();
		} else if (stats.curChgAtkTime != -1 && user.animSteInfo.normalizedTime > stats.colStart) {
			stats.curChgDuration = Mathf.Clamp(Time.time - stats.curChgAtkTime, 0.0f, stats.maxChgTime);
			particles.startSpeed = (int)(stats.curChgDuration/stats.chgLevels);
		}
		
		if (user.animSteInfo.normalizedTime > stats.colEnd) {
			particles.Stop();
		}
	}
	private IEnumerator Shoot(int count)
    {
		if(bullPattern == 'L'){
			variance = 22f;
		}
		if(bullPattern == 'S'){
			variance = 47f;
		}
		if(count == 0){
			count = 1;
		}
		//High cap for basic is 12f variance, low cap for shotty is 22f
		if(bullPattern=='L'){
			spray = Quaternion.Euler(new Vector3(user.transform.eulerAngles.x,Random.Range(-(variance-user.stats.coordination)+user.transform.eulerAngles.y,(variance-user.stats.coordination)+user.transform.eulerAngles.y),user.transform.eulerAngles.z));
			for (int i = 0; i < count; i++)
			{
				StartCoroutine(makeSound(action,playSound,action.length));
				yield return StartCoroutine(Wait(.08f));
				Instantiate(stats.projectile, user.transform.position, spray);
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
		if(bullPattern == 'M'){//Glitchy
			spray = Quaternion.Euler(new Vector3(user.transform.eulerAngles.x,Random.Range(-(variance-user.stats.coordination)+user.transform.eulerAngles.y,(variance-user.stats.coordination)+user.transform.eulerAngles.y),user.transform.eulerAngles.z));
			StartCoroutine(makeSound(action,playSound,action.length));
			for (int i = 0; i < count*count*count; i++)
			{
				yield return StartCoroutine(Wait(.01f));
				Instantiate(stats.projectile, user.transform.position, spray);
				/*shots.Add(bullet);
				foreach (Shot bull in shots){
					bull.facing = spray.eulerAngles;
				}*/
				//variance -= 1;
				spray = Quaternion.Euler(spray.eulerAngles.x,(spray.eulerAngles.y+Random.Range(-kick,kick)),spray.eulerAngles.z);
				kick += .2f;
				if(kick >= 10f){
					kick = 5f;
				}
			}
		}
		//High cap for shotty is 27f variance, low cap for shotty is 47f
		if(bullPattern=='S'){
			StartCoroutine(makeSound(action,playSound,action.length));
			for (int i = 0; i < count*(int)Random.Range(3,5); i++)
			{
				yield return StartCoroutine(Wait(.02f));
				spray = Quaternion.Euler(new Vector3(user.transform.eulerAngles.x,Random.Range(-(variance-user.stats.coordination*1.5f)+user.transform.eulerAngles.y,(variance-user.stats.coordination*1.5f)+user.transform.eulerAngles.y),user.transform.eulerAngles.z));
				Instantiate(stats.projectile, user.transform.position, spray);
				/*shots.Add(bullet);
				foreach (Shot bull in shots){
					bull.facing = spray.eulerAngles;
				}
				shots.Clear();*/
				variance += 2;
			}
		}
    }
	private IEnumerator Wait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime)
			yield return 0;
	}
}
