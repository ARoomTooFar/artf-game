// Parent scripts for enemy units

using UnityEngine;
using System.Collections;
using System;

public class Enemy : Character, IStunable {

	//Damage variables
	public int testDmg;
	public bool testable;

	//Aggro Variables
	public int[] enemyDmgTable = new int[3];
	public int recentDmgTaken = 0;
	public float dmgTimer = 0f;
	public bool aggro = false;

	// private EnemySight enemySight;


	protected override void Awake() {
		base.Awake();
		opposition = Type.GetType ("Player");
	}

	// Use this for initialization
	protected override void Start () {
		base.Start();
		isGrounded = true;
	}

	protected override void setInitValues() {
		base.setInitValues();
		//Testing with base 0-10 on stats with 10 being 100/cap%
		stats.maxHealth = 40;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=4;
		stats.luck=0;
		testDmg = 0;
		testable = true;
		setAnimHash ();
	}

	protected virtual void initStates() {
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(!isDead) {

			animSteInfo = animator.GetCurrentAnimatorStateInfo(0);
			animSteHash = animSteInfo.nameHash;
			actable = (animSteHash == runHash || animSteHash == idleHash) && freeAnim;
			attacking = animSteHash == atkHashStart || animSteHash == atkHashSwing || animSteHash == atkHashEnd ;
				


			//If aggro'd, will chase, and if not attacked for 5 seconds, will deaggro
			if (aggro == true) 
			{
				fAggro ();
			}
			}

			if (isGrounded) {
				movementAnimation();
			} else {
				falling();
			}
	}

	public override void damage(int dmgTaken, Character striker) {
		base.damage(dmgTaken, striker);
		print ("Fuck: " + dmgTaken + " Damage taken");
	}

	public override void damage(int dmgTaken) {
		//aggro is on and timer reset if attacked
		if (aggro == false) {
			aggro = true;
			dmgTimer = 0f;
		}

		base.damage(dmgTaken);
	}

	public virtual void fAggro(){
		if (dmgTimer < 5f)
		{
			dmgTimer += Time.deltaTime;
		}
		else if (dmgTimer >= 5f)
		{
			resetAggro ();
		}
	}

	public virtual void resetAggro(){
		dmgTimer = 0f;
		aggro = false;
	}




}