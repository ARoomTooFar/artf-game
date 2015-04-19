// Parent scripts for enemy units

using UnityEngine;
using System.Collections;
using System;

public class NewEnemy : Character {
	
	//Aggro Variables
	public float dmgTimer = 0f;
	public bool aggro = false;
	
	//Is this unit part of the hive mind?
	public bool swarmBool = false;
	//Object which holds hivemind aggrotable
	public Swarm swarm;
	
	public int tier;
	public AoETargetting aRange;
	protected StateMachine sM;
	
	protected float fov = 150f;
	protected float lineofsight = 15f;
	public float maxAtkRadius, minAtkRadius;
	

	// Variables for use in player detection
	protected bool alerted = false;
	public GameObject target;
	public Vector3? lastSeenPosition = null;
	protected float lastSeenSet = 0.0f;
	protected AggroTable aggroT;
	protected bool targetchanged;

	public Vector3 targetDir;
	public Vector3 resetpos;
	
	
	protected int layerMask = 1 << 9;
	
	protected float aggroTimer = 7.0f;
	
	void OnEnable()
	{
		Player.OnDeath += playerDied;
	}
	
	
	void OnDisable()
	{
		Player.OnDeath -= playerDied;
	}
	
	protected override void Awake() {
		base.Awake();
		opposition = Type.GetType ("Player");
		
		facing = Vector3.back;
		targetchanged = false;
		
		// aRange.opposition = this.opposition;
		aRange.affectPlayers = true;
	}
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		
		//Uses swarm aggro table if this unit swarms
		if(swarmBool){
			aggroT = swarm.aggroTable;
		} else {
			aggroT = new AggroTable();
		}

		if (this.testing) {
			this.SetTierData(0);
		}

		foreach(EnemyBehaviour behaviour in this.animator.GetBehaviours<EnemyBehaviour>()) {
			behaviour.SetVar(this.GetComponent<NewEnemy>());
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
		if (!stats.isDead) {
			isGrounded = Physics.Raycast (transform.position, -Vector3.up, minGroundDistance);
			
			animSteInfo = animator.GetCurrentAnimatorStateInfo (0);
			animSteHash = animSteInfo.fullPathHash;
			actable = (animSteHash == runHash || animSteHash == idleHash) && freeAnim;
			this.animator.SetBool("Actable", this.actable);
			attacking = animSteHash == atkHashStart || animSteHash == atkHashSwing || animSteHash == atkHashEnd;
			this.animator.SetBool("IsInAttackAnimation", this.attacking || this.animSteHash == this.atkHashChgSwing || this.animSteHash == this.atkHashCharge);
			
			
			//If aggro'd, will chase, and if not attacked for 5 seconds, will deaggro
			//If we want a deaggro function, change the if statement to require broken line of sight to target
			/*if (aggro == true) {
				fAggro ();
			}*/
			
			
			if (isGrounded) {
				movementAnimation ();
			} else {
				falling ();
			}

			this.TargetFunction();
		}
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
		setAnimHash ();
	}

	// Things that are tier specific should be set here
	public virtual void SetTierData(int tier) {
		this.tier = tier;
		this.animator.SetInteger("Tier", this.tier);
	}

	//-----------//
	// Functions //
	//-----------//

	protected virtual void TargetFunction() {
		if (target != null) {
			target = aggroT.getTarget ();
			if (this.canSeePlayer(target)) {
				float distance = Vector3.Distance(this.transform.position, this.target.transform.position);
				this.animator.SetBool ("InAttackRange", distance < this.maxAtkRadius && distance >= this.minAtkRadius);
			} else {
				this.target = null;
				this.animator.SetBool ("Target", false);
			}
		} else {
			if (aRange.unitsInRange.Count > 0) {
				foreach(Character tars in aRange.unitsInRange) {
					if (this.canSeePlayer(tars.gameObject) && !tars.isDead) {
						target = tars.gameObject;
						this.animator.SetBool("Target", true);
						this.alerted = true;
						this.animator.SetBool("Alerted", true);
						break;
					}
				}
			}
		}
	}

	//-----------//


	//-----------------------//
	// Calculation Functions //
	//-----------------------//
	
	public virtual bool canSeePlayer(GameObject p) {
		if (p == null) {
			this.animator.SetBool("CanSeeTarget", false);
			return false;
		}
		
		// Check angle of forward direction vector against the vector of enemy position relative to player position
		Vector3 direction = p.transform.position - transform.position;
		float angle = Vector3.Angle(direction, this.facing);

		float dis = Vector3.Distance(this.transform.position, p.transform.position);

		if (angle < fov) {
			RaycastHit hit;
			if (Physics.Raycast (transform.position + transform.up, direction.normalized, out hit, dis, layerMask)) {
				this.animator.SetBool("CanSeeTarget", false);
				return false;
			} else {
				aggroT.add(p,1);
				lastSeenPosition = p.transform.position;
				this.animator.SetBool ("HasLastSeenPosition", true);
				this.lastSeenSet = 3.0f;
				alerted = true;
				this.animator.SetBool("Alerted", true);
				this.animator.SetBool("CanSeeTarget", true);
				return true;
				
			}
		}
		this.animator.SetBool("CanSeeTarget", false);
		return false;
	}
	
	// Will change units facing to be towards their target. If new facing is zero it doesn't changes
	//     Move to ultilties if we find more uses for this outside of AI
	public virtual void getFacingTowardsTarget() {
		Vector3 newFacing = Vector3.zero;
		
		if (this.target != null) {
			newFacing = this.target.transform.position - this.transform.position;
			newFacing.y = 0.0f;
			if (newFacing != Vector3.zero) this.facing = newFacing.normalized;
		}
	}
	
	//----------------------//
	
	
	//-------------------------------//
	// Character Inherited Functions //
	//-------------------------------//
	
	public override void damage(int dmgTaken, Character striker) {
		base.damage(dmgTaken, striker);
		
		if (aggro == false) {
			aggro = true;
			dmgTimer = 0f;
		}		
		
		// aggroT.add(striker.gameObject, dmgTaken); // This is causing the the AI to stop attacking and only approach and search for a target once they get damaged
	}
	
	public override void damage(int dmgTaken) {
		//aggro is on and timer reset if attacked
		if (aggro == false) {
			aggro = true;
			dmgTimer = 0f;
		}
		
		base.damage(dmgTaken);
	}
	
	public override void die() {
		base.die ();
		Destroy (gameObject);
	}
	
	//-------------------------------//
	
	//-----------------//
	// Aggro Functions //
	//-----------------//
	
	public virtual void fAggro(){
		if (dmgTimer < 5f)
		{
			dmgTimer += Time.deltaTime;
		}
		else if (dmgTimer >= 5f)
		{
			resetAggro ();
			dmgTimer = 0f;
		}
	}
	
	public virtual void resetAggro(){
		dmgTimer = 0f;
		aggro = false;
		target = null;
		this.animator.SetBool ("Target", false);
	}
	
	public virtual void playerDied(GameObject dead){
		if (aggroT != null) {
			aggroT.deletePlayer(dead);
		}
	}
	
	//---------------//
}