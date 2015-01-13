using UnityEngine;
using System.Collections;

public class Gun : Weapons {
	//L = line, S = Shotty/Sprayheavy
	public char bullPattern;
	//for spray of shotgun
	public Transform spray;
	
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
			spray = player.transform;
			spray.rotation = Quaternion.Euler(new Vector3(player.transform.eulerAngles.x,Random.Range(-22f+player.transform.eulerAngles.y,22f+player.transform.eulerAngles.y),player.transform.eulerAngles.z));
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
			if(stats.weapType == 1){
				spray.rotation = Quaternion.Euler(new Vector3(player.transform.eulerAngles.x,Random.Range(-2f+player.transform.eulerAngles.y,2f+player.transform.eulerAngles.y),player.transform.eulerAngles.z));
				GameObject bullet = (GameObject) Instantiate(stats.projectile, player.transform.position, player.transform.rotation);

				//Having issues passing particle.startSpeed to the bullet object..=(
				//bullet.GetComponent<Equipment>().particles.startSpeed = particles.startSpeed;
				//if(stats.chgType == 2){
				StartCoroutine(multShoot((int)(stats.curChgDuration/0.4f)));
				//}
			}
			print("Charge Attack power level:" + (int)(stats.curChgDuration/0.4f));
		} else if (stats.curChgAtkTime == 0 && player.animSteInfo.normalizedTime > stats.colStart) {
			stats.curChgAtkTime = Time.time;
			particles.startSpeed = 0;
			particles.Play();
		} else if (stats.curChgAtkTime != -1 && player.animSteInfo.normalizedTime > stats.colStart) {
			stats.curChgDuration = Mathf.Clamp(Time.time - stats.curChgAtkTime, 0.0f, stats.maxChgTime);
			particles.startSpeed = (int)(stats.curChgDuration/0.4f);
		}
		
		if (player.animSteInfo.normalizedTime > stats.colEnd) {
			particles.Stop();
		}
	}
	private IEnumerator multShoot(int count)
    {
		if(count == 0 || count == 1){
			count = 1;
			spray.rotation = Quaternion.Euler(new Vector3(player.transform.eulerAngles.x,Random.Range(-2f+player.transform.eulerAngles.y,2f+player.transform.eulerAngles.y),player.transform.eulerAngles.z));
			GameObject bullet = (GameObject) Instantiate(stats.projectile, player.transform.position, player.transform.rotation);
		}
		if(bullPattern=='L'){
			for (int i = 1; i < count; i++)
			{
				yield return StartCoroutine(Wait(.08f));
				spray.rotation = Quaternion.Euler(new Vector3(player.transform.eulerAngles.x,Random.Range(-2f+player.transform.eulerAngles.y,2f+player.transform.eulerAngles.y),player.transform.eulerAngles.z));
				GameObject bullet = (GameObject) Instantiate(stats.projectile, player.transform.position, spray.rotation);
			}
		}
		if(bullPattern=='S'){
			for (int i = 1; i < count*2; i++)
			{
				yield return StartCoroutine(Wait(.02f));
				spray.rotation = Quaternion.Euler(new Vector3(player.transform.eulerAngles.x,Random.Range(-12f+player.transform.eulerAngles.y,12f+player.transform.eulerAngles.y),player.transform.eulerAngles.z));
				GameObject bullet = (GameObject) Instantiate(stats.projectile, player.transform.position, spray.rotation);
			}
		}
    }
	private IEnumerator Wait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime)
			yield return 0;
	}
}
