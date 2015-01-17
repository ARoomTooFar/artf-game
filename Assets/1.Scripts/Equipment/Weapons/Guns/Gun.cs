using UnityEngine;
using System.Collections;

public class Gun : Weapons {
	//L = line, S = Shotty/Sprayheavy
	public char bullPattern;
	//for spray of shotgun
	public Quaternion spray;
	
	// Use this for initialization
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
		bullPattern = 'L';
		//bullPattern = 'S';
		//if(bullPattern == 'S'){
			spray = player.transform.rotation;
			spray = Quaternion.Euler(new Vector3(player.transform.eulerAngles.x,Random.Range(-(12f-player.stats.coordination)+player.transform.eulerAngles.y,(12f-player.stats.coordination)+player.transform.eulerAngles.y),player.transform.eulerAngles.z));
		//}
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	public override void initAttack() {
		base.initAttack();
	}

	public override void attack() {
		if (!Input.GetKey(player.controls.attack) && stats.curChgAtkTime != -1) {
			stats.curChgAtkTime = -1;
			//spray = Quaternion.Euler(new Vector3(player.transform.eulerAngles.x,Random.Range(-(12f-player.stats.coordination)+player.transform.eulerAngles.y,(12f-player.stats.coordination)+player.transform.eulerAngles.y),player.transform.eulerAngles.z));
			//GameObject bullet = (GameObject) Instantiate(stats.projectile, player.transform.position, spray);
			StartCoroutine(multShoot((int)(stats.curChgDuration/stats.chgLevels)));
			print("Charge Attack power level:" + (int)(stats.curChgDuration/stats.chgLevels));
		} else if (stats.curChgAtkTime == 0 && player.animSteInfo.normalizedTime > stats.colStart) {
			stats.curChgAtkTime = Time.time;
			particles.startSpeed = 0;
			particles.Play();
		} else if (stats.curChgAtkTime != -1 && player.animSteInfo.normalizedTime > stats.colStart) {
			stats.curChgDuration = Mathf.Clamp(Time.time - stats.curChgAtkTime, 0.0f, stats.maxChgTime);
			particles.startSpeed = (int)(stats.curChgDuration/stats.chgLevels);
			//StartCoroutine(multShoot((int)(stats.curChgDuration/0.4f)));
		}
		
		if (player.animSteInfo.normalizedTime > stats.colEnd) {
			particles.Stop();
		}
	}
	private IEnumerator multShoot(int count)
    {
		if(count == 0){
			count = 1;
		}
		//High cap for basic is 12f variance, low cap for shotty is 22f
		if(bullPattern=='L'){
			for (int i = 0; i < count; i++)
			{
				yield return StartCoroutine(Wait(.08f));
				spray = Quaternion.Euler(new Vector3(player.transform.eulerAngles.x,Random.Range(-(22f-player.stats.coordination)+player.transform.eulerAngles.y,(22f-player.stats.coordination)+player.transform.eulerAngles.y),player.transform.eulerAngles.z));
				GameObject bullet = (GameObject) Instantiate(stats.projectile, player.transform.position, spray);
			}
		}
		//High cap for shotty is 27f variance, low cap for shotty is 47f
		if(bullPattern=='S'){
			for (int i = 0; i < count*3; i++)
			{
				yield return StartCoroutine(Wait(.02f));
				spray = Quaternion.Euler(new Vector3(player.transform.eulerAngles.x,Random.Range(-(47f-player.stats.coordination*1.5f)+player.transform.eulerAngles.y,(47f-player.stats.coordination*1.5f)+player.transform.eulerAngles.y),player.transform.eulerAngles.z));
				GameObject bullet = (GameObject) Instantiate(stats.projectile, player.transform.position, spray);
			}
		}
    }
	private IEnumerator Wait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime)
			yield return 0;
	}
}
