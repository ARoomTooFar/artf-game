using UnityEngine;
using System.Collections;

public class TestSMRanged: Enemy{
	
	//Public variables to tweak in inspector
	public float fov = 110f;
	public bool playerInSight = false;
	public NavMeshAgent nav;
	public Animator ani;
	public Vector3 retreatPos;
	
	//Private variables for use in player detection
	private SphereCollider col;
	private GameObject[] players;
	private GameObject target;
	
	private Vector3? lastSeenPosition = null;
	private float posTimer = 0f;
	
	private StateMachine testStateMachine;
	
	//Get players, navmesh and all colliders
	void Awake ()
	{
		nav = GetComponent<NavMeshAgent> ();
		col = GetComponent<SphereCollider> ();
		ani = GetComponent<Animator> ();
		players = GameObject.FindGameObjectsWithTag ("Player");
		retreatPos = transform.position;
		
		//Placeholder for more advanced aggro where target may change
		target = players [1];
		
		//State machine initialization
		testStateMachine = new StateMachine ();
		initStates ();
		testStateMachine.Start ();
	}
	
	protected override void Update()
	{
		base.Update ();
		Debug.Log (lastSeenPosition.HasValue);
		if (lastSeenPosition.HasValue && !canSeePlayer (target)) {
			posTimer += Time.deltaTime;
		} else if (canSeePlayer (target)){
			posTimer = 0f;
		}
		if(posTimer > 10f)
		{
			posTimer = 0f;
			lastSeenPosition = null;
		}
		
		
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
		TestSMRanged agent = (TestSMRanged)a;
		return agent.distanceToPlayer(agent.giveTarget()) < 700f 
			&& agent.distanceToPlayer(agent.giveTarget()) >= 8.5f && agent.canSeePlayer (agent.giveTarget());
	}
	
	public bool isResting(Character a)
	{
		TestSMRanged agent = (TestSMRanged)a;
		return (agent.giveLastSeenPos() == null);
	}
	
	public bool isRetreating(Character a)
	{
		TestSMRanged agent = (TestSMRanged)a;
		return agent.distanceToPlayer(agent.giveTarget()) <= 20f;
	}
	
	public bool isAttacking(Character a)
	{
		TestSMRanged agent = (TestSMRanged)a;
		return  agent.distanceToPlayer(agent.giveTarget()) < 100f && agent.distanceToPlayer(agent.giveTarget()) > 20f;
	}
	
	public bool isSearching(Character a)
	{
		TestSMRanged agent = (TestSMRanged)a;
		return (agent.giveLastSeenPos ().HasValue) && !(agent.canSeePlayer (agent.giveTarget()));
		
	}
	
	public void Approach(Character a)
	{
		TestSMRanged agent = (TestSMRanged)a;
		agent.ani.SetBool ("Moving", true);
		agent.nav.destination = agent.giveTarget ().transform.position;
	}
	
	public void Attack(Character a)
	{
		TestSMRanged agent = (TestSMRanged)a;
		agent.ani.SetBool ("Moving", false);
		agent.nav.destination = agent.transform.position;
		if (!agent.canSeePlayer (agent.giveTarget()))
			agent.transform.LookAt (agent.giveTarget().transform.position);
		if (agent.actable)
			agent.gear.weapon.initAttack();
	}
	
	
	//Improve search function to have multiple goals (scanning room, reaching last known position of target)
	public void Search(Character a)
	{
		TestSMRanged agent = (TestSMRanged)a;
		agent.ani.SetBool ("Moving", true);
		agent.nav.destination = (Vector3)agent.giveLastSeenPos ();
	}
	
	
	//Improve retreat AI
	public void Retreat(Character a)
	{
		TestSMRanged agent = (TestSMRanged)a;
		agent.ani.SetBool ("Moving", true);
		agent.nav.speed = 5;
		agent.nav.destination = agent.retreatPos;
	}
	
	public void Rest(Character a)
	{
		TestSMRanged agent = (TestSMRanged)a;
		agent.ani.SetBool ("Moving", false);
		agent.nav.Stop ();
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
