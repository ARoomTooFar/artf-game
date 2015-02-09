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
		
		stats.atkSpeed = 2.0f;
		stats.damage = 1;
		stats.maxChgTime = 2.0f;
		
		// Originally bull pattern S
		//shotty
		variance = 47f;

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
		variance = 47f;

		if(count == 0){
			count = 1;
		}

		//High cap for shotty is 27f variance, low cap for shotty is 47f

		StartCoroutine(makeSound(action,playSound,action.length));
		for (int i = 0; i < count*(int)Random.Range(3,5); i++) {
			yield return 0;
			spray = Quaternion.Euler(new Vector3(user.transform.eulerAngles.x,Random.Range(-(variance-user.stats.coordination*1.5f)+user.transform.eulerAngles.y,(variance-user.stats.coordination*1.5f)+user.transform.eulerAngles.y),user.transform.eulerAngles.z));

			fireProjectile();

			/*shots.Add(bullet);
			foreach (Shot bull in shots){
				bull.facing = spray.eulerAngles;
			}
			shots.Clear();*/
			variance += 2;
		}
	}
}
