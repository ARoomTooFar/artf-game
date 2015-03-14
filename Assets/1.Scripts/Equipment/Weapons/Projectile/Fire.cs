using UnityEngine;
using System.Collections;
using System;

public class Fire : Projectile {	
	// Use this for initialization
	public float duration;
	public float subDuration;
	public int adjuster;
	public ParticleAnimator part1;
	public ParticleAnimator part2;
	public ParticleAnimator part3;
	public bool falseStop;
	//public ParticleCollisionEvent[] collisionEvents;
	protected override void Start() {
		base.Start();
		//collisionEvents = new ParticleCollisionEvent[16];
	}

	public override void setInitValues(Character player, Type ene, float partSpeed,bool effect,BuffsDebuffs hinder) {
		user = player;
		opposition = ene;

		transform.Rotate(Vector3.right * 90);
		moving = true;
		damage = (int)partSpeed;
		speed = 0.1f;
		castEffect = true;
		debuff = hinder;
		StartCoroutine("growWait",1f);
		if(partSpeed == .5f){
			StartCoroutine("Wait",.6f);
		}
		adjuster = 1;
		GetComponent<CapsuleCollider>().enabled = false;
		/*duration = partSpeed*.75f;
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
		}*/
		//StartCoroutine(growWait(subDuration));
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
	public void extinguish(){
		Destroy(gameObject);
	}
	private IEnumerator growWait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime){
			if(timer <= duration/2 + .05f && timer >= duration/2 -.05f){
				GetComponent<CapsuleCollider>().enabled = true;
			}
			yield return 0;
		}
		GetComponent<CapsuleCollider>().height += 4f;
		adjuster++;
		//adjuster -= .25f;
		if(GetComponent<CapsuleCollider>().height >= 6.5f){
			GetComponent<CapsuleCollider>().center += new Vector3(0,2,0);
		}
		//GetComponent<CapsuleCollider>().center -= new Vector3(0,adjuster,0);
		//GetComponent<CapsuleCollider>().transform.position = Vector3.MoveTowards (GetComponent<CapsuleCollider>().transform.position, -target.position, -speed);
		part1.sizeGrow *= 2.5f;
		part2.sizeGrow *= 2.5f;
		//part3.sizeGrow *= 2.5f;
		StartCoroutine("growWait",1f);
	}

	// Update is called once per frame
	protected override void Update() {
		base.Update();
		GetComponent<CapsuleCollider>().center -= new Vector3(0,.075f,0);
	}
}
