using UnityEngine;
using System.Collections;

public class Pistol : RangedWeapons {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	protected override void setInitValues() {
		base.setInitValues();
		maxAmmo = 20;
		currAmmo = maxAmmo;
		// Use sword animations for now
		stats.weapType = 0;
		stats.weapTypeName = "sword";
		loadSpeed = 2.5f;
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
		if(!reload){
			variance = 22f;

			if(count == 0){
				count = 1;
			}

		//High cap for basic is 12f variance, low cap for shotty is 22f
			spray = Quaternion.Euler(new Vector3(user.transform.eulerAngles.x,Random.Range(-(variance-user.stats.coordination)+user.transform.eulerAngles.y,(variance-user.stats.coordination)+user.transform.eulerAngles.y),user.transform.eulerAngles.z));
			for (int i = 0; i < count; i++) {
				
				StartCoroutine(makeSound(action,playSound,action.length));
				yield return StartCoroutine(Wait(.08f));
				fireProjectile();
				currAmmo--;
				if(currAmmo<=0){
					reload = true;
					StartCoroutine(loadAmmo());
				}
				spray = Quaternion.Euler(spray.eulerAngles.x,(spray.eulerAngles.y+Random.Range(-kick,kick)),spray.eulerAngles.z);
				kick += .2f;
				if(kick >= 5f){
					kick = 2f;
				}
			}
		}
	}
}
