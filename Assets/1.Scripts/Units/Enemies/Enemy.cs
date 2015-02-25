// Parent scripts for enemy units

using UnityEngine;
using System.Collections;
using System;


public class Enemy : Character, IStunable {

	//Damage variables
	public bool inGrey;
	public int testDmg;
	public int greyDamage;
	public bool testable;

	//Aggro Variables
	public int[] enemyDmgTable = new int[3];
	public int recentDmgTaken = 0;
	public float dmgTimer = 0f;
	public bool aggro = false;

	private EnemySight enemySight;

	protected override void Awake() {
		opposition = Type.GetType ("Player");
		BDS = new BuffDebuffSystem(this);
		stats = new Stats(this.GetComponent<MonoBehaviour>());
		animator = GetComponent<Animator>();
		facing = Vector3.forward;
		isDead = false;
		gear.equipGear(this, opposition);
		inventory.equipItems(this);
		freeAnim = true;
		setInitValues();
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
		stats.speed=5;
		stats.luck=0;
		inGrey = false;
		greyDamage = 0;
		testDmg = 0;
		testable = true;
		setAnimHash ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(!isDead) {

			animSteInfo = animator.GetCurrentAnimatorStateInfo(0);
			animSteHash = animSteInfo.nameHash;
			actable = (animSteHash == runHash || animSteHash == idleHash) && freeAnim;
			attacking = animSteHash == atkHashStart || animSteHash == atkHashSwing || animSteHash == atkHashEnd ;
				
			if (!isGrounded) {
				falling();
			}
				


			//If aggro'd, will chase, and if not attacked for 5 seconds, will deaggro
			if (aggro == true) 
			{
				fAggro ();
			}
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

	public override void falling(){

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


	// The duration are essentially stun, expand on these later
	public override void pull(float pullDuration) {
		stun();
	}
	
	public override void push(float pushDuration) {
		stun();
	}


	/*//Use for other shit maybe
	public virtual void OnTriggerEnter(Collider other) {
		damage (1);
	}*/
}