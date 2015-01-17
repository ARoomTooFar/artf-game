// Parent script for AI controlled enemies

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour, IFallable, IDamageable<int> {
	
	public float speed = 5.0f;
	public float gravity = 50.0f;
	public bool isGrounded = false;
	public bool actable = true; // Boolean to show if a unit can act or is stuck in an animation
	
	public Vector3 facing; // Direction unit is facing
	public Vector3 curFacing; // A better facing var, will change and combine in future
	
	public float minGroundDistance; // How far this unit should be from the ground when standing up
	
	public Controls controls;
	public Stats stats;
	public CharItems charItems;
	
	public bool freeAnim;
	
	public bool invincible = false;
	
	// Animation variables
	public Animator animator;
	public AnimatorStateInfo animSteInfo;
	protected int idleHash, runHash, atkHash;
	
	// Use this for initialization
	protected virtual void Start () {
		animator = GetComponent<Animator>();
		facing = curFacing = Vector3.forward;
		freeAnim = true;
		setInitValues();
		setAnimHash();
	}
	protected virtual void setInitValues() {
	}
	
	// Gets hash code for animations (Faster than using string name when running)
	protected virtual void setAnimHash() {
		idleHash = Animator.StringToHash ("Base Layer.idle");
		runHash = Animator.StringToHash ("Base Layer.run");
		atkHash = Animator.StringToHash ("Base Layer.attack");
	}
	
	protected virtual void FixedUpdate() {
		
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		isGrounded = Physics.Raycast (transform.position, -Vector3.up, minGroundDistance);
		
		animSteInfo = animator.GetCurrentAnimatorStateInfo(0);
		actable = (animSteInfo.nameHash == runHash || animSteInfo.nameHash == idleHash) && freeAnim;
		
		if (!isGrounded) {
			falling();
		}
	}


	//-------------------------------------------//


	//----------------------------------//
	// Falling Interface Implementation //
	//----------------------------------//w
	
	public virtual void falling() {
		// fake gravity
		// Animation make it so rigidbody gravity works oddly due to some gravity weight
		// Seems like Unity Pro is needed to change that, so unless we get it, this will suffice 
		rigidbody.velocity = new Vector3 (0.0f, -gravity, 0.0f);
	}
	
	//----------------------------------//

	
	
	//---------------------------------//
	// Damage Interface Implementation //
	//---------------------------------//
	
	public virtual void damage(int dmgTaken) {
		if (!invincible) {
			stats.health -= dmgTaken;
		}
	}
	
	//----------------------------------//
}