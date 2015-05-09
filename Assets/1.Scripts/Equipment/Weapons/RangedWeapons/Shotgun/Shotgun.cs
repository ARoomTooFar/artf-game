using UnityEngine;
using System.Collections;

public class Shotgun : RangedWeapons {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	protected override void setInitValues() {
		base.setInitValues();
		// Use sword animations for now
		stats.weapType = 0;
		stats.weapTypeName = "sword";
		stats.atkSpeed = 1.0f;
		stats.damage = 15 + user.GetComponent<Character>().stats.coordination;
		stats.maxChgTime = 5;
		
		// Originally bull pattern S
		//shotty
		variance = 25f;

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
		//High cap for shotty is 27f variance, low cap for shotty is 47f
		float origVariance = variance;
		StartCoroutine(makeSound(action,playSound,action.length));
		for (int i = 0; i < count*(int)Random.Range(3,5); i++) {
			yield return 0;
			spray = Quaternion.Euler(new Vector3(user.transform.eulerAngles.x,Random.Range(-(variance-user.stats.coordination*1.5f)+user.transform.eulerAngles.y,(variance-user.stats.coordination*1.5f)+user.transform.eulerAngles.y),user.transform.eulerAngles.z));
			fireProjectile();
			variance += 2;
		}
		variance = origVariance;
	}
}
