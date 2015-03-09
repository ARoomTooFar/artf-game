using UnityEngine;
using System.Collections;
using System;

public class Fire : Projectile {	
	// Use this for initialization
	public float duration;
	public float subDuration;
	public float adjuster;
	public ParticleAnimator part1;
	public ParticleAnimator part2;
	public ParticleAnimator part3;
	public bool falseStop;
	//public ParticleCollisionEvent[] collisionEvents;
	protected override void Start() {
		base.Start();
		adjuster = 3f;
		//collisionEvents = new ParticleCollisionEvent[16];
	}

	public override void setInitValues(Character player, Type ene, float partSpeed,bool effect,BuffsDebuffs hinder) {
		user = player;
		opposition = ene;

		transform.Rotate(Vector3.right * 90);
		moving = true;
		damage = 0;
		speed = 0.1f;
		castEffect = true;
		debuff = hinder;
		duration = partSpeed*.75f;
		if(duration < .5f){
			duration = .5f;
		}
		falseStop = !effect;
		subDuration = duration/5;
		if(particles !=null){
			particles.startSpeed = partSpeed;
			particles.Play();
		}
		StartCoroutine(Wait(duration));
		if(falseStop){
			StartCoroutine(stopWait(duration/2));
		}
		StartCoroutine(growWait(subDuration));
	}
	private IEnumerator stopWait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime){
			//testable = true;
			yield return 0;
		}
		moving = false;
	}
	private IEnumerator Wait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime){
			//testable = true;
			yield return 0;
		}
		Destroy(gameObject);
	}
	private IEnumerator growWait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime){
			yield return 0;
		}
		GetComponent<CapsuleCollider>().height += 1f;
		adjuster -= .25f;
		GetComponent<CapsuleCollider>().center -= new Vector3(0,adjuster,0);
		//GetComponent<CapsuleCollider>().transform.position = Vector3.MoveTowards (GetComponent<CapsuleCollider>().transform.position, -target.position, -speed);
		part1.sizeGrow *= 2.5f;
		part2.sizeGrow *= 2.5f;
		part3.sizeGrow *= 2.5f;
		StartCoroutine(growWait(duration));
	}

	// Update is called once per frame
	protected override void Update() {
		base.Update();
		
	}
}
