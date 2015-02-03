using UnityEngine;
using System.Collections;

//All conditions need go here
public class cApproachR :  ISMCondition 
{
	public TestSMRanged e;
	public bool test()
	{
		return e.distanceToPlayer(e.giveTarget()) < 700f && e.distanceToPlayer(e.giveTarget()) >= 100f && e.canSeePlayer (e.giveTarget());
	}
	
}

//Stuck in rest mode when search is over
public class cRestR :  ISMCondition 
{
	public TestSMRanged e;
	public bool test()
	{
		return (e.giveLastSeenPos() == null);
	}
}

public class cRetreatR :  ISMCondition 
{
	public TestSMRanged e;
	public bool test()
	{
		return e.distanceToPlayer(e.giveTarget()) <= 20f;
	}
	
}

public class cAttackR :  ISMCondition 
{
	public TestSMRanged e;
	public bool test()
	{
		return  e.distanceToPlayer(e.giveTarget()) < 100f && e.distanceToPlayer(e.giveTarget()) > 20f;
	}
	
}

public class cSearchR :  ISMCondition 
{
	public TestSMRanged e;
	public bool test()
	{
		return (e.giveLastSeenPos ().HasValue) && !(e.canSeePlayer (e.giveTarget()));
		
	}
	
}

//All actions go here

//Add patrolling
public class aRestR : ISMAction 
{
	public TestSMRanged e;
	public void action()
	{
		e.ani.SetBool ("Moving", false);
		e.nav.Stop ();
	}
}

public class aApproachR : ISMAction 
{
	public TestSMRanged e;
	public void action()
	{
		e.ani.SetBool ("Moving", true);
		e.nav.destination = e.giveTarget ().transform.position;
	}
}

public class aAttackR : ISMAction 
{
	public TestSMRanged e;
	public void action()
	{
		e.ani.SetBool ("Moving", false);
		e.nav.destination = e.transform.position;
		if (!e.canSeePlayer (e.giveTarget()))
			e.transform.LookAt (e.giveTarget().transform.position);
		if (e.actable)
			e.gear.weapon.initAttack();
	}
}


//Improve search function to have multiple goals (scanning room, reaching last known position of target)
public class aSearchR : ISMAction 
{
	public TestSMRanged e;
	public void action()
	{
		e.ani.SetBool ("Moving", true);
		e.nav.destination = (Vector3)e.giveLastSeenPos ();
	}
}

//Improve retreat AI
public class aRetreatR : ISMAction 
{
	public TestSMRanged e;
	public void action()
	{
		e.ani.SetBool ("Moving", true);
		e.nav.speed = 5;
		e.nav.destination = e.retreatPos;
	}
}

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
		cApproachR cApproach = new cApproachR();
		cApproach.e = this;
		tApproach.addCondition(cApproach);
		
		cRestR cRest = new cRestR ();
		cRest.e = this;
		tRest.addCondition (cRest);
		
		cAttackR cAttack = new cAttackR ();
		cAttack.e = this;
		tAttack.addCondition (cAttack);
		
		cRetreatR cRetreat = new cRetreatR ();
		cRetreat.e = this;
		tRetreat.addCondition (cRetreat);
		
		cSearchR cSearch = new cSearchR ();
		cSearch.e = this;
		tSearch.addCondition (cSearch);
		
		
		//Set actions for the states
		aRestR aRest = new aRestR ();
		aRest.e = this;
		rest.addAction (aRest);
		
		aSearchR aSearch = new aSearchR ();
		aSearch.e = this;
		search.addAction (aSearch);
		
		aAttackR aAttack = new aAttackR ();
		aAttack.e = this;
		attack.addAction (aAttack);
		
		aApproachR aApproach = new aApproachR ();
		aApproach.e = this;
		approach.addAction (aApproach);
		
		aRetreatR aRetreat = new aRetreatR ();
		aRetreat.e = this;
		retreat.addAction (aRetreat);
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
