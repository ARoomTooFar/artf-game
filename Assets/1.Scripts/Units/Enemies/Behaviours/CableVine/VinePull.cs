using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class VinePull : Approach {

	private Transform transform;
	public Stun stun;
	public MeleeWeapons weap;
	public CVSensor feeler;
	public GameObject tether;
	public GameObject MyMawPos;
	private Player p;
	bool onCoolDown;
	float currTime, waitUntil;
	protected int layerMask = 1 << 9;

	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		feeler = unit.GetComponentInChildren<CVSensor> ();
		tether = feeler.gameObject;
		p = unit.target.GetComponent<Player> ();
		stun = new Stun ();
		p.mash_threshold = 4;
		p.mash_value = 0;
		onCoolDown = false;
		transform = unit.transform.Find ("CVFeeler");
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
	}


	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		if (onCoolDown) {
			currTime += Time.deltaTime;
			if(currTime >= waitUntil){
				onCoolDown = false;
				p.mash_value = 0;
			}
			return;
		}

		if (p.break_free) {
			onCoolDown = true;
			p.BDS.rmvBuffDebuff(stun, unit.gameObject);
			p.mash_value = 0;
			currTime = Time.time;
			waitUntil = Time.time + 2.0f;
			return;
		}

		unit.facing = unit.target.transform.position - unit.transform.position;
		unit.facing.y = 0.0f;


		if (!feeler.Hooked ()) {
			Vector3 direction = unit.target.transform.position - unit.transform.position;
			float dis = Vector3.Distance(unit.transform.position, unit.target.transform.position);

			RaycastHit hit;
			transform.localRotation = Quaternion.LookRotation(unit.facing, Vector3.forward);

			if (Physics.Raycast (unit.transform.position + unit.transform.up, direction.normalized, out hit, dis, layerMask)) {
				if(hit.distance > 10){
					if(transform.localScale.y < 1){
						transform.localScale += new Vector3(0, 0.01f, 0);
					}
				}
			}

		} else if (MyMawPos != null) {
			unit.target.transform.position = unit.target.transform.position - pullVelocity (MyMawPos.transform.position);
			if (unit.actable && !unit.attacking){
				unit.gear.weapon.initAttack();
				p.damage(1);
				p.BDS.addBuffDebuff (stun, unit.gameObject, 4.0f);
			}
		} else { 
			unit.target.transform.position = unit.target.transform.position - pullVelocity (unit.transform.position);
			if (unit.actable && !unit.attacking){
				unit.gear.weapon.initAttack();
				p.damage(1);
				p.BDS.addBuffDebuff (stun, unit.gameObject, 4.0f);
			}

		}


	}

	private void retract() {
		if(unit.transform.localScale.x > 0.5) unit.transform.localScale -= new Vector3(0.01f, 0, 0);
		if(unit.transform.localScale.z > 0.5) unit.transform.localScale -= new Vector3(0, 0, 0.01f);
		//tether.transform.position = unit.facing - new Vector3((tether.transform.localScale.y)/2.0f, 0, 0);
	}

	private Vector3 pullVelocity(Vector3 destination){
		Vector3 dir = unit.target.transform.position - destination;
		float time =  dir.magnitude/((NewCableVine)unit).pull_velocity;
		Vector3 velocity = new Vector3 ();
		velocity.x = dir.x / time;
		velocity.y = dir.y / time;
		velocity.z = dir.z / time;
		return velocity;
	}

	private Vector3 pullVelocity(float pull_velocity){
		float time = unit.facing.magnitude/pull_velocity;
		Vector3 velocity = new Vector3 ();
		velocity.x = unit.facing.x / time;
		velocity.y = unit.facing.y / time;
		velocity.z = unit.facing.z / time;
		return velocity;
	}

	private IEnumerator Wait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime){
			yield return 0;
		}
	}


}
