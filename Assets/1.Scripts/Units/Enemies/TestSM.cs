using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TestSM: Enemy{
	
	//Public variables to tweak in inspector
	public float fov = 110f;
	public float patrolSpeed = 2f;
	public float approachSpeed = 5f;
	public float reactionTime = 5f;			// Time buffer between player sighting and giving chase
	public float patrolWaitTime = 1f;		// Time wait when reaching the patrol way point
	public float aggroTimer = 10f;
	public int waypointIndex = 0;
	public Vector3 resetpos;

	private GameObject target;

	// Waypoints for patrolling
	public List<Transform> patrolWP = new List<Transform>();


	public bool playerInSight = false;
	public NavMeshAgent nav;
	public Vector3 retreatPos;
	
	//Private variables for use in player detection
	private SphereCollider col;
	private GameObject[] players;

	private Vector3? lastSeenPosition = null;
	private float posTimer = 0f;

	private StateMachine testStateMachine;
	
	//Get players, navmesh and all colliders
	protected override void Awake ()
	{
		base.Awake ();
		nav = GetComponent<NavMeshAgent> ();
		col = GetComponent<SphereCollider> ();
		//animator = GetComponent<Animator> ();
		players = GameObject.FindGameObjectsWithTag ("Player");
		retreatPos = transform.position;
		resetpos = transform.position;
		patrolWP.Add (transform);

		//Placeholder for more advanced aggro where target may change
		target = players [0];

		//State machine initialization
		testStateMachine = new StateMachine ();
		initStates ();
		testStateMachine.Start ();
	}
	
	protected override void Update()
	{
		base.Update ();
		if (lastSeenPosition.HasValue && !canSeePlayer (target)) {
			posTimer += Time.deltaTime;
		} else if (canSeePlayer (target)){
			posTimer = 0f;
		}
		if(posTimer > aggroTimer)
		{
			posTimer = 0f;
			lastSeenPosition = null;
		}

		animSteInfo = animator.GetCurrentAnimatorStateInfo(0);
		actable = (animSteInfo.nameHash == runHash || animSteInfo.nameHash == idleHash) && freeAnim;
		

		testStateMachine.Update ();
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

	public bool isApproaching(Character a)
	{
		nav.speed = approachSpeed;
		TestSM agent = (TestSM)a;
		return agent.distanceToPlayer(agent.giveTarget()) < 500f 
			&& agent.distanceToPlayer(agent.giveTarget()) >= 8.5f && agent.canSeePlayer (agent.giveTarget());
	}

	public bool isResting(Character a)
	{
		TestSM agent = (TestSM)a;
		return (agent.giveLastSeenPos() == null);
	}

	public bool isRetreating(Character a)
	{
		TestSM agent = (TestSM)a;
		return agent.distanceToPlayer(agent.giveTarget()) <= 5.5f;
	}

	public bool isAttacking(Character a)
	{
		TestSM agent = (TestSM)a;
		return  agent.distanceToPlayer(agent.giveTarget()) < 8.5f && agent.distanceToPlayer(agent.giveTarget()) > 5.5f;
	}

	public bool isSearching(Character a)
	{
		TestSM agent = (TestSM)a;
		return (agent.giveLastSeenPos ().HasValue) && !(agent.canSeePlayer (agent.giveTarget()));
		
	}

	public void Approach(Character a)
	{
		TestSM agent = (TestSM)a;
		agent.animator.SetBool ("Moving", true);
		agent.nav.destination = agent.giveTarget ().transform.position;
	}

	public void Attack(Character a)
	{
		TestSM agent = (TestSM)a;
		agent.animator.SetBool ("Moving", false);
		agent.nav.destination = agent.transform.position;
		if (!agent.canSeePlayer (agent.giveTarget()))
			agent.transform.LookAt (agent.giveTarget().transform.position);
		if (agent.actable){
		/*******************
			//Should be causing damage, is only triggering attack animation
		******************/
			agent.gear.weapon.initAttack();
		}
	}


//Improve search function to have multiple goals (scanning room, reaching last known position of target)
	public void Search(Character a)
	{
		TestSM agent = (TestSM)a;
		agent.animator.SetBool ("Moving", true);
		agent.nav.destination = (Vector3)agent.giveLastSeenPos ();
	}


//Improve retreat AI
	public void Retreat(Character a)
	{
		TestSM agent = (TestSM)a;
		agent.animator.SetBool ("Moving", true);
		agent.nav.speed = 5;
		agent.nav.destination = agent.retreatPos;
	}

	public void Rest(Character a)
	{
		TestSM agent = (TestSM)a;
//		agent.animator.SetBool ("Moving", false);
		Patrol (a);
	}

	public void Patrol(Character a){
		nav.speed = patrolSpeed;
		TestSM agent = (TestSM)a;
		agent.animator.SetBool ("Moving", true);

		if (agent.nav.remainingDistance < agent.nav.stoppingDistance) {
			if(agent.waypointIndex <= agent.patrolWP.Count - 1){
				agent.waypointIndex++;

			}
			else agent.waypointIndex = 0;

		}

		agent.nav.destination = agent.patrolWP[waypointIndex].position;
	}

	public bool canSeePlayer(GameObject p)
	{

			// Check angle of forward direction vector against the vector of enemy position relative to player position
			Vector3 direction = p.transform.position - transform.position;
			float angle = Vector3.Angle (direction, transform.forward);
		
			if (angle < fov * 0.5f) 
			{
				RaycastHit hit;
				if (Physics.Raycast (transform.position + transform.up, direction.normalized, out hit, col.radius)) 
				{
				
					if (hit.collider.gameObject == p) 
					{
						lastSeenPosition = p.transform.position;
						return true;
					
					}
				}
			}

	
		return false;
	}

	public float distanceToPlayer(GameObject p)
	{
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

}
