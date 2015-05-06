using UnityEngine;
using System.Collections;

public class CacklebranchPistol : RangedWeapons {
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	protected override void setInitValues() {
		base.setInitValues();
		stats.weapTypeName = "";
		stats.atkSpeed = 2.0f;
		stats.damage = 20 + user.GetComponent<Character>().stats.coordination;

		variance = 10f;
		kick = 0.5f;
		
		spray = user.transform.rotation;
		spray = Quaternion.Euler(new Vector3(user.transform.eulerAngles.x,Random.Range(-(12f-user.stats.coordination)+user.transform.eulerAngles.y,(12f-user.stats.coordination)+user.transform.eulerAngles.y),user.transform.eulerAngles.z));
		
	}
	
	// Update is called once per frame
	protected override void Update () {
		Debug.Log("FIX THE WARNING ON THIS -> public virtual IEnumerator Shoot(int count)");
		//Assets/1.Scripts/Equipment/Weapons/RangedWeapons/MonsterWeapons/CacklebranchPistol.cs(34,36):
		//warning CS0114: `CacklebranchPistol.Shoot(int)' hides inherited member `RangedWeapons.Shoot(int)'.
		//To make the current member override that implementation, add the override keyword. Otherwise add the new keyword
		base.Update();
	}
	
	public override void initAttack() {
		base.initAttack();
	}
	
	public virtual IEnumerator Shoot(int count) {
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
			spray = Quaternion.Euler(spray.eulerAngles.x,(spray.eulerAngles.y+Random.Range(-kick,kick)),spray.eulerAngles.z);
			kick += .2f;
			if(kick >= 5f){
				kick = 2f;
			}
		}
	}
}
