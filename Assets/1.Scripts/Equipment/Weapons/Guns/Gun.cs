using UnityEngine;
using System.Collections;

public class Gun : Weapons {
	public string bullPattern;
	
	
	// Use this for initialization
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	protected override void setInitValues() {
		base.setInitValues();
		stats.atkSpeed = 2.0f;
		stats.damage = 1;
		stats.multHit = 3;
		stats.chgType = 2;
		stats.maxChgTime = 2.0f;
		stats.weapType = 1;
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
				if(stats.chgType == 1){
					GameObject bullet = (GameObject) Instantiate(stats.projectile, player.transform.position, player.transform.rotation);
				}
				//Having issues passing particle.startSpeed to the bullet object..=(
				//bullet.GetComponent<Equipment>().particles.startSpeed = particles.startSpeed;
				if(stats.chgType == 2){
					StartCoroutine(multShoot((int)(stats.curChgDuration/0.4f),stats.atkSpeed *3));
				}
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
	private IEnumerator multShoot(int count, float frequency)
    {
		if(count == 0){
			count = 1;
		}
        for (int i = 0; i < count; i++)
        {
			yield return StartCoroutine(Wait(.08f));
			GameObject bullet = (GameObject) Instantiate(stats.projectile, player.transform.position, player.transform.rotation);
        }
    }
	private IEnumerator Wait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime)
			yield return 0;
	}
}
