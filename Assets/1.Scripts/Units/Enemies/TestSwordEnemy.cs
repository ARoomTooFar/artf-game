using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TestSwordEnemy: Enemy {

	public bool alerted = false;
	public float fov = 150f;
	float lineofsight = 15f;
	// float aggro_radius = 500f;
	float attack_radius = 6.0f;
	public Vector3? lastSeenPosition = null;
	public bool inPursuit = false;
	public int waypointIndex = 0;

	// Waypoints for patrolling
	public List<Transform> patrolWP = new List<Transform>();

	// Nav Mesh Positions
	public Vector3 resetpos;
	public Vector3 retreatPos;

	public AggroRange aRange;

	// Protected Variables
	protected NavMeshAgent nav;
	protected AggroTable aggroT;
	protected StateMachine sM;
	
	// Variables for use in player detection
	public GameObject target;
	public Vector3 lastTargetPosition;
	private GameObject[] players;

	private float posTimer = 0f;
	public float aggroTimer = 10f;
	
	//-------------------//
	// Primary Functions //
	//-------------------//

	// Get players, navmesh and all colliders
	protected override void Awake ()
	{
		base.Awake ();
		facing = Vector3.back;
		aggroT = new AggroTable();
		nav = GetComponent<NavMeshAgent> ();

		retreatPos = transform.position;
		resetpos = transform.position;
		patrolWP.Add (transform);
		
		//Placeholder for more advanced aggro where target may change
		// players = GameObject.FindGameObjectsWithTag ("Player");
		// target = players [0];
		// lastTargetPosition = target.transform.position;

		aRange.opposition = this.opposition;

		//State machine initialization
		sM = new StateMachine ();
		initStates ();
		sM.Start ();
	}

	protected override void Update()
	{
		base.Update ();
		if (target != null) target = aggroT.getTarget ();
		if(target == null || !lastSeenPosition.HasValue){
			sM.Update();
			return;
		}
		bool iseeyou = canSeePlayer (target);
		if (target && lastSeenPosition.HasValue && !iseeyou) {
			posTimer += Time.deltaTime;
		} else if (target && iseeyou){
			posTimer = 0f;
		}
		if(posTimer > aggroTimer)
		{
			posTimer = 0f;
			lastSeenPosition = null;
			alerted = false;
			target = null;
		}
		
		//Speed updates from stats now, fix navigation to not overshoot like it does
		nav.speed = stats.speed;
		
		sM.Update ();
	}
	
	// Initializes states, transitions and actions
	public void initStates(){
		
		// Initialize all states
		State rest = new State("rest");
		State approach = new State("approach");


		// Add all the states to the state machine
		sM.states.Add (rest.id, rest);
		sM.states.Add (approach.id, approach);
		
		// Set initial state for the State Machine of this unit
		sM.initState = rest;


		// Initialize all transitions
		Transition tRest = new Transition(rest);
		Transition tApproach = new Transition (approach);


		// Set conditions for the transitions
		tApproach.addCondition(isApproaching, this);
		tRest.addCondition (isResting, this);


		// Set actions for the states
		rest.addAction (Rest, this);
		approach.addAction (Approach, this);


		// Set the transitions for the states
		rest.addTransition (tApproach);

		
		/*
		State search = new State ("search");
		State attack = new State ("attack");
		State retreat = new State ("retreat");

		

		Transition tAttack = new Transition (attack);
		Transition tRetreat = new Transition (retreat);
		Transition tSearch = new Transition (search);

		approach.addTransition (tSearch);
		approach.addTransition (tAttack);
		attack.addTransition (tRetreat);
		attack.addTransition (tApproach);
		attack.addTransition (tSearch);
		retreat.addTransition (tRest);
		retreat.addTransition (tAttack);
		search.addTransition (tRest);
		search.addTransition (tApproach);
		search.addTransition (tAttack);
		

		tAttack.addCondition (isAttacking, this);
		tRetreat.addCondition (isRetreating, this);
		tSearch.addCondition (isSearching, this);
		
		

		search.addAction (Search, this);
		attack.addAction (Attack, this);
		retreat.addAction (Retreat, this);
		*/
	}

	protected override void setInitValues() {
		base.setInitValues();
		//Testing with base 0-10 on stats with 10 being 100/cap%
		stats.maxHealth = 40;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=9;
		stats.luck=0;
		testDmg = 0;
		//testable = true;
		
	}
	
	//----------------------//

	//----------------------//
	// Transition Functions //
	//----------------------//

	public bool isApproaching(Character a) {
		nav.speed = stats.speed * stats.spdManip.speedPercent;
		TestSwordEnemy agent = (TestSwordEnemy)a;

		if (target == null) {
			if (aRange.inRange.Count > 0) {
				target = aRange.inRange[0].gameObject;
			} else {
				return false;
			}
		}

		float distance = agent.distanceToPlayer(agent.target);

		// if (distance < aggro_radius && distance >= attack_radius && (agent.canSeePlayer (agent.target) || agent.inPursuit)) {
		if (distance >= attack_radius && (agent.canSeePlayer (agent.target) || agent.inPursuit)) {
			agent.inPursuit = true;
			agent.alerted = true;
			return true;
		}
		return false;
	}
	
	public bool isResting(Character a) {
		TestSwordEnemy agent = (TestSwordEnemy)a;
		return (agent.lastSeenPosition == null);
	}

	//---------------------//


	//------------------//
	// Action Functions //
	//------------------//

	public void Rest(Character a) {
		TestSwordEnemy agent = (TestSwordEnemy)a;
		agent.inPursuit = false;
		// Patrol (a);
	}

	public void Patrol(Character a) {
		nav.speed = (stats.speed * stats.spdManip.speedPercent)/2;
		TestSwordEnemy agent = (TestSwordEnemy)a;
		agent.animator.SetBool ("Moving", true);
		
		if (agent.nav.remainingDistance < agent.nav.stoppingDistance) {
			if(agent.waypointIndex <= agent.patrolWP.Count - 1){
				agent.waypointIndex++;
				
			}
			else agent.waypointIndex = 0;
			
		}
		
		// if(agent.patrolWP.Count > 0) agent.nav.destination = agent.patrolWP[waypointIndex].position;
	}

	public void Approach(Character a) {
		TestSwordEnemy agent = (TestSwordEnemy)a;
		this.facing = agent.target.transform.position - agent.transform.position;
		this.facing.y = 0.0f;
		this.rigidbody.velocity = this.facing.normalized * stats.speed * stats.spdManip.speedPercent;
	}
	
	//------------------//


	//-----------------------//
	// Calculation Functions //
	//-----------------------//

	public bool canSeePlayer(GameObject p) {
		// Check angle of forward direction vector against the vector of enemy position relative to player position
		Vector3 direction = p.transform.position - transform.position;
		float angle = Vector3.Angle(direction, this.facing);
		
		if (angle < fov) {
			RaycastHit hit;
			if (Physics.Raycast (transform.position + transform.up, direction.normalized, out hit, lineofsight)) {
				
				if (hit.collider.gameObject == p) {
					aggroT.add(p,1);
					// Debug.Log(p.name);
					lastSeenPosition = p.transform.position;
					alerted = true;
					return true;
				}
			}
		}
		
		return false;
	}

	public float distanceToPlayer(GameObject p) {
		if (p == null) return 0.0f;
		Vector3 distance = p.transform.position - transform.position;
		return distance.sqrMagnitude;
	}

	//-----------------//

	//Public variables to tweak in inspector

	/*
	public float patrolSpeed = 2f;
	public float approachSpeed = 5f;
	public float reactionTime = 5f;			// Time buffer between player sighting and giving chase
	public float patrolWaitTime = 1f;		// Time wait when reaching the patrol way point
	
	public AggroRange awareness;

	
	
	public bool playerInSight = false;
	
	private StateMachine testStateMachine;*/

	/*
	
	public bool Alerted(){
		return alerted;
	}
	
	public bool canFeel(){
		return inPursuit;
	}

	
	public bool isRetreating(Character a)
	{
		TestSwordEnemy agent = (TestSwordEnemy)a;
		float distance = agent.distanceToPlayer(agent.giveTarget());
		return distance <= 5.5f;
	}
	
	public bool isAttacking(Character a)
	{
		TestSwordEnemy agent = (TestSwordEnemy)a;
		float distance = agent.distanceToPlayer(agent.giveTarget());
		return distance < attack_radius && distance > 5.5f;
	}
	
	public bool isSearching(Character a)
	{
		TestSwordEnemy agent = (TestSwordEnemy)a;
		return (agent.giveLastSeenPos ().HasValue) && !(agent.canSeePlayer (agent.giveTarget()));
		
	}
	
	public void Attack(Character a)
	{
		TestSwordEnemy agent = (TestSwordEnemy)a;
		agent.alerted = true;
		agent.animator.SetBool ("Moving", false);
		agent.nav.destination = agent.transform.position;
		if (!agent.canSeePlayer (agent.giveTarget()))
			agent.transform.LookAt (agent.giveTarget().transform.position);
		
		if (agent.actable && !attacking){
			agent.gear.weapon.initAttack();
		}
	}
	
	
	//Improve search function to have multiple goals (scanning room, reaching last known position of target)
	public void Search(Character a)
	{
		TestSwordEnemy agent = (TestSwordEnemy)a;
		agent.inPursuit = true;
		agent.alerted = true;
		agent.animator.SetBool ("Moving", true);
		agent.nav.destination = (Vector3)agent.giveLastSeenPos ();
	}
	
	
	//Improve retreat AI
	public void Retreat(Character a)
	{
		TestSwordEnemy agent = (TestSwordEnemy)a;
		agent.inPursuit = true;;
		agent.alerted = true;
		agent.animator.SetBool ("Moving", true);
		agent.nav.speed = 5;
		agent.nav.destination = agent.retreatPos;
	}


	public override void damage(int dmgTaken, Character striker) {
		base.damage(dmgTaken, striker);
		aggroT.add (striker.gameObject, dmgTaken);
	}

	

	
	public Vector3? giveLastSeenPos()
	{
		return lastSeenPosition;
	}
	
	public GameObject giveTarget()
	{
		return target;
	}
	*/



//----------------------------------------------------------------------------------------------------//


	/*
	//Public variables to tweak in inspector
	public float fov = 150f;
	public float patrolSpeed = 2f;
	public float approachSpeed = 5f;
	public float reactionTime = 5f;			// Time buffer between player sighting and giving chase
	public float patrolWaitTime = 1f;		// Time wait when reaching the patrol way point
	public float aggroTimer = 10f;
	public int waypointIndex = 0;
	public bool alerted = false;
	public bool inPursuit = false;
	public Vector3 resetpos;
	public Vector3 lastTargetPosition;
	float aggro_radius = 500f;
	float attack_radius = 8.5f;
	float lineofsight = 15f;

	public AggroRange awareness;
	
	public GameObject target;

	// Waypoints for patrolling
	public List<Transform> patrolWP = new List<Transform>();
	
	
	public bool playerInSight = false;
	public NavMeshAgent nav;
	public Vector3 retreatPos;
	
	//Private variables for use in player detection
	private GameObject[] players;
	private AggroTable aggroT;
	
	public Vector3? lastSeenPosition = null;
	private float posTimer = 0f;
	
	private StateMachine testStateMachine;
	
	//Get players, navmesh and all colliders
	protected override void Awake ()
	{
		base.Awake ();
		aggroT = new AggroTable();
		nav = GetComponent<NavMeshAgent> ();
		//animator = GetComponent<Animator> ();
		players = GameObject.FindGameObjectsWithTag ("Player");
		retreatPos = transform.position;
		resetpos = transform.position;
		patrolWP.Add (transform);
		
		//Placeholder for more advanced aggro where target may change
		
		target = players [0];
		lastTargetPosition = target.transform.position;
		
		//State machine initialization
		testStateMachine = new StateMachine ();
		initStates ();
		testStateMachine.Start ();
	}
	
	protected override void Update()
	{
		base.Update ();
		target = aggroT.getTarget ();
		if(target == null || !lastSeenPosition.HasValue){
			testStateMachine.Update();
			return;
		}
		bool iseeyou = canSeePlayer (target);
		// if(target != null) Debug.Log (target.name);
		if (target && lastSeenPosition.HasValue && !iseeyou) {
			posTimer += Time.deltaTime;
		} else if (target && iseeyou){
			posTimer = 0f;
		}
		if(posTimer > aggroTimer)
		{
			posTimer = 0f;
			lastSeenPosition = null;
			alerted = false;
			target = null;
		}
		
		//Speed updates from stats now, fix navigation to not overshoot like it does
		nav.speed = stats.speed;
		
		testStateMachine.Update ();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		//Testing with base 0-10 on stats with 10 being 100/cap%
		stats.maxHealth = 40;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=10;
		stats.luck=0;
		inGrey = false;
		greyDamage = 0;
		testDmg = 0;
		//testable = true;
		
	}
	
	public override void damage(int dmgTaken, Character striker) {
		base.damage(dmgTaken, striker);
		aggroT.add (striker.gameObject, dmgTaken);
	}
	
	
	//Initializes states, transitions and actions
	public void initStates(){
		
		//Initialize all states
		State rest = new State("rest");
		State approach = new State("approach");
		State search = new State ("search");
		State attack = new State ("attack");
		State retreat = new State ("retreat");
		
		
		//Set initial state for the SM
		testStateMachine.initState = rest;
		
		
		//Initialize all transitions
		Transition tRest = new Transition(rest);
		Transition tApproach = new Transition (approach);
		Transition tAttack = new Transition (attack);
		Transition tRetreat = new Transition (retreat);
		Transition tSearch = new Transition (search);
		
		
		//Set the transitions for the states
		rest.addTransition (tApproach);
		approach.addTransition (tSearch);
		approach.addTransition (tAttack);
		attack.addTransition (tRetreat);
		attack.addTransition (tApproach);
		attack.addTransition (tSearch);
		retreat.addTransition (tRest);
		retreat.addTransition (tAttack);
		search.addTransition (tRest);
		search.addTransition (tApproach);
		search.addTransition (tAttack);
		
		
		//Set conditions for the transitions
		tApproach.addCondition(isApproaching, this);
		tRest.addCondition (isResting, this);
		tAttack.addCondition (isAttacking, this);
		tRetreat.addCondition (isRetreating, this);
		tSearch.addCondition (isSearching, this);
		
		
		//Set actions for the states
		rest.addAction (Rest, this);
		search.addAction (Search, this);
		attack.addAction (Attack, this);
		approach.addAction (Approach, this);
		retreat.addAction (Retreat, this);
	}
	
	public bool Alerted(){
		return alerted;
	}
	
	public bool canFeel(){
		return inPursuit;
	}
	
	public bool isApproaching(Character a)
	{
		nav.speed = approachSpeed;
		TestSwordEnemy agent = (TestSwordEnemy)a;
		float distance = agent.distanceToPlayer(agent.giveTarget());
		return (distance < aggro_radius 
		        && distance >= attack_radius && (agent.canSeePlayer (agent.giveTarget()) || agent.canFeel()) );
	}
	
	public bool isResting(Character a)
	{
		TestSwordEnemy agent = (TestSwordEnemy)a;
		return (agent.giveLastSeenPos() == null);
	}
	
	public bool isRetreating(Character a)
	{
		TestSwordEnemy agent = (TestSwordEnemy)a;
		float distance = agent.distanceToPlayer(agent.giveTarget());
		return distance <= 5.5f;
	}
	
	public bool isAttacking(Character a)
	{
		TestSwordEnemy agent = (TestSwordEnemy)a;
		float distance = agent.distanceToPlayer(agent.giveTarget());
		return distance < attack_radius && distance > 5.5f;
	}
	
	public bool isSearching(Character a)
	{
		TestSwordEnemy agent = (TestSwordEnemy)a;
		return (agent.giveLastSeenPos ().HasValue) && !(agent.canSeePlayer (agent.giveTarget()));
		
	}
	
	public void Approach(Character a)
	{
		TestSwordEnemy agent = (TestSwordEnemy)a;
		agent.inPursuit = true;
		agent.alerted = true;
		agent.animator.SetBool ("Moving", true);
		agent.nav.destination = agent.giveTarget ().transform.position;
		//if (inventory.items[4].curCoolDown <= 0) inventory.items[4].useItem();
	}
	
	public void Attack(Character a)
	{
		TestSwordEnemy agent = (TestSwordEnemy)a;
		agent.alerted = true;
		agent.animator.SetBool ("Moving", false);
		agent.nav.destination = agent.transform.position;
		if (!agent.canSeePlayer (agent.giveTarget()))
			agent.transform.LookAt (agent.giveTarget().transform.position);
		
		if (agent.actable && !attacking){
			agent.gear.weapon.initAttack();
		}
	}
	
	
	//Improve search function to have multiple goals (scanning room, reaching last known position of target)
	public void Search(Character a)
	{
		TestSwordEnemy agent = (TestSwordEnemy)a;
		agent.inPursuit = true;
		agent.alerted = true;
		agent.animator.SetBool ("Moving", true);
		agent.nav.destination = (Vector3)agent.giveLastSeenPos ();
	}
	
	
	//Improve retreat AI
	public void Retreat(Character a)
	{
		TestSwordEnemy agent = (TestSwordEnemy)a;
		agent.inPursuit = true;;
		agent.alerted = true;
		agent.animator.SetBool ("Moving", true);
		agent.nav.speed = 5;
		agent.nav.destination = agent.retreatPos;
	}
	
	public void Rest(Character a)
	{
		TestSwordEnemy agent = (TestSwordEnemy)a;
		agent.inPursuit = false;
		//		agent.animator.SetBool ("Moving", false);
		Patrol (a);
	}
	
	public void Patrol(Character a){
		nav.speed = patrolSpeed;
		TestSwordEnemy agent = (TestSwordEnemy)a;
		agent.animator.SetBool ("Moving", true);
		
		if (agent.nav.remainingDistance < agent.nav.stoppingDistance) {
			if(agent.waypointIndex <= agent.patrolWP.Count - 1){
				agent.waypointIndex++;
				
			}
			else agent.waypointIndex = 0;
			
		}
		
		//		if(agent.patrolWP.Count > 0) agent.nav.destination = agent.patrolWP[waypointIndex].position;
	}
	
	public bool canSeePlayer(GameObject p)
	{
		// Check angle of forward direction vector against the vector of enemy position relative to player position
		Vector3 direction = p.transform.position - transform.position;
		float angle = Vector3.Angle (direction, transform.forward);
		
		if (angle < fov) 
		{
			RaycastHit hit;
			if (Physics.Raycast (transform.position + transform.up, direction.normalized, out hit, lineofsight)) 
			{
				
				if (hit.collider.gameObject == p) 
				{
					aggroT.add(p,1);
					// Debug.Log(p.name);
					lastSeenPosition = p.transform.position;
					alerted = true;
					return true;
					
				}
			}
		}
		
		
		return false;
	}

	
	public float distanceToPlayer(GameObject p)
	{
		if (p == null)
			return 0.0f;
		Vector3 distance = p.transform.position - transform.position;
		return distance.sqrMagnitude;
	}
	
	public Vector3? giveLastSeenPos()
	{
		return lastSeenPosition;
	}
	
	public GameObject giveTarget()
	{
		return target;
	}
	*/
}
