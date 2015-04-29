using UnityEngine;
using System.Collections;

public class AssaultRifle : RangedWeapons {
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	protected override void setInitValues() {
		base.setInitValues();
		maxAmmo = 40;
		currAmmo = maxAmmo;
		stats.weapType = 0;
		stats.weapTypeName = "sword";
		
		stats.atkSpeed = 2.0f;
		stats.damage = 5 + user.GetComponent<Character>().stats.coordination;
		stats.maxChgTime = 2.0f;
		
		// Bull Pattern M originally
		//machine gun
		variance = 15f;
		kick = 5f;

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
	
	protected override IEnumerator Shoot(int count) {
		if(!reload || !needReload){
			if(count == 0){
				count = 2;
			}

			spray = Quaternion.Euler(new Vector3(user.transform.eulerAngles.x,Random.Range(-(variance-user.stats.coordination)+user.transform.eulerAngles.y,(variance-user.stats.coordination)+user.transform.eulerAngles.y),user.transform.eulerAngles.z));
			StartCoroutine(makeSound(action,playSound,action.length));
			for (int i = 0; i < count*count; i++) {
				yield return StartCoroutine(Wait(.01f));
				fireProjectile();
				currAmmo--;
				if(currAmmo<=0 && needReload){
					reload = true;
					StartCoroutine(loadAmmo());
				}
				spray = Quaternion.Euler(spray.eulerAngles.x,(spray.eulerAngles.y+Random.Range(-kick,kick)),spray.eulerAngles.z);
				kick += .2f;
				if(kick >= 6f){
					kick = 5f;
				}
			}
		}
		
	}
}
