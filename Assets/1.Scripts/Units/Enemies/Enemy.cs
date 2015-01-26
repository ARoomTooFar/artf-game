// Parent scripts for enemy units

using UnityEngine;
using System.Collections;

public class Enemy : Character, IStunable<float>, IForcible<float> {

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
	}
	
	// Update is called once per frame
	protected override void Update () {
		if(!isDead) {
				
			animSteInfo = animator.GetCurrentAnimatorStateInfo(0);
			actable = (animSteInfo.nameHash == runHash || animSteInfo.nameHash == idleHash) && freeAnim;
				
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

	// Change this for other units in the future, ie. Unit that can be stunned and those that can't
	public virtual void stun(float stunDuration) {
		print ("Stunned for " + stunDuration + " seconds");
	}

	// The duration are essentially stun, expand on these later
	public virtual void pull(float pullDuration) {
		stun(pullDuration);
	}
	
	public virtual void push(float pushDuration) {
		stun(pushDuration);
	}


	/*//Use for other shit maybe
	public virtual void OnTriggerEnter(Collider other) {
		damage (1);
	}*/
}