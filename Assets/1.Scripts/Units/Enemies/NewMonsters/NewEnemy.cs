// Parent scripts for enemy units

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class NewEnemy : NewCharacter {

	protected float lastSawTargetCount;

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
	protected GameObject MusicPlayer;

	public Vector3 targetDir;
	public Vector3 resetpos;

	
	protected int layerMask = 1 << 9;
	
	protected float aggroTimer = 5.0f;

	// bool sparksDone = true;
	GameObject sparks = null;
	
	public MonsterLoot monsterLoot;
	
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

		MusicPlayer = GameObject.Find ("MusicPlayer");
	}
	
	// Update is called once per frame
	protected override void Update () {
		if (stats.isDead) return;
		base.Update();
		this.TargetFunction();
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
		setAnimHash ();
	}

	// Things that are tier specific should be set here
	public virtual void SetTierData(int tier) {
		this.tier = tier;
		this.animator.SetInteger("Tier", this.tier);
		monsterLoot = this.gameObject.AddComponent<MonsterLoot>();
	}

	//-----------//
	// Functions //
	//-----------//

	protected virtual void TargetFunction() {
		target = aggroT.GetTopAggro ();
		if (target != null) {
			this.animator.SetBool ("Target", true);
			if (this.canSeePlayer(target)) {
				this.lastSawTargetCount = 0.0f;
				float distance = Vector3.Distance(this.transform.position, this.target.transform.position);
				this.animator.SetBool ("InAttackRange", distance < this.maxAtkRadius && distance >= this.minAtkRadius);
			} else {
				this.lastSawTargetCount += Time.deltaTime;
				this.target = null;
				this.animator.SetBool ("Target", false);
				if (this.lastSawTargetCount > this.aggroTimer) {
					this.aggroT.RemoveUnit(this.aggroT.GetTopAggro());
					this.lastSawTargetCount = 2;
				}
			}
		} else {
			this.animator.SetBool ("Target", false);
			if (aRange.unitsInRange.Count > 0) {
				foreach(Character tars in aRange.unitsInRange) {
					if (this.canSeePlayer(tars.gameObject) && !tars.isDead) {
						aggroT.AddAggro(tars.gameObject, 1);
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


	protected float distanceToPlayer(GameObject p) {
		if (p == null) return 0.0f;
		float distance = Vector3.Distance(this.transform.position, p.transform.position);
		//Debug.Log (distance);
		return distance;
	}

	public virtual bool canSeePlayer(GameObject p) {
		if (p == null) {
			this.animator.SetBool("CanSeeTarget", false);
			return false;
		}
		
		// Check angle of forward direction vector against the vector of enemy position relative to player position
		Vector3 direction = p.transform.position - transform.position;
		direction.y = 0.0f;
		float angle = Vector3.Angle(direction, this.facing);

		float dis = Vector3.Distance(this.transform.position, p.transform.position);

		if (angle < fov) {
			RaycastHit hit;
			if (Physics.Raycast (transform.position + transform.up, direction.normalized, out hit, dis, layerMask)) {
				this.animator.SetBool("CanSeeTarget", false);
				return false;
			} else {
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
		
		if (this.target != null && this.actable) {
			newFacing = this.target.transform.position - this.transform.position;
			newFacing.y = 0.0f;
			if (newFacing != Vector3.zero) {
				this.facing = newFacing.normalized;
				this.transform.localRotation = Quaternion.LookRotation(facing);	
			}
		}
	}
	
	//----------------------//
	
	
	//-------------------------------//
	// Character Inherited Functions //
	//-------------------------------//
	
	public override void damage(int dmgTaken, Transform atkPosition, GameObject source) {
		if (this.invincible) return;
		this.damage(dmgTaken, atkPosition);
		aggroT.AddAggro(source, dmgTaken);
		isHit = true;

		//particle effects
		if(sparks == null){
			sparks = Instantiate(Resources.Load("Sparks"), transform.position, Quaternion.identity) as GameObject;
			Material particleMat = Resources.Load("Materials/EnemySparks", typeof(Material)) as Material;
			sparks.GetComponent<ParticleRenderer>().material = particleMat;
			Destroy (sparks, 1);
		}
	}
	
	public override void damage(int dmgTaken, Transform atkPosition) {
		base.damage(dmgTaken, atkPosition);
		isHit = true;


		//particle effects
		if(sparks == null){
			sparks = Instantiate(Resources.Load("Sparks"), transform.position, Quaternion.identity) as GameObject;
			Material particleMat = Resources.Load("Materials/EnemySparks", typeof(Material)) as Material;
			sparks.GetComponent<ParticleRenderer>().material = particleMat;
			Destroy (sparks, 1);
		}
	}
	
	public override void damage(int dmgTaken) {
		base.damage(dmgTaken);

		//particle effects
		if(sparks == null){
			sparks = Instantiate(Resources.Load("Sparks"), transform.position, Quaternion.identity) as GameObject;
			Material particleMat = Resources.Load("Materials/EnemySparks", typeof(Material)) as Material;
			sparks.GetComponent<ParticleRenderer>().material = particleMat;
			Destroy (sparks, 1);
		}
	}
	
	public override void die() {
		monsterLoot.lootMonster();
		base.die ();
		Destroy (gameObject);
	}
	
	//-------------------------------//
	
	//-----------------//
	// Aggro Functions //
	//-----------------//

	public virtual void playerVanished(GameObject dead){
		if (aggroT != null) {
			aggroT.RemoveUnit(dead);
			target = null;
		}
	}
	
	public virtual void taunted(GameObject taunter){
		if (aggroT != null){
			aggroT.AddAggro(taunter,aggroT.GetAggro()*2);
		}
	}

	//---------------//
}