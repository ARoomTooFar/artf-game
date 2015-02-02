using UnityEngine;
using System.Collections;

//All conditions need go here
public class cApproach :  ISMCondition 
{
	public TestSM e;
	public bool test()
	{
		return e.distanceToPlayer(e.giveTarget()) < 500f && e.distanceToPlayer(e.giveTarget()) >= 8.5f && e.canSeePlayer (e.giveTarget());
	}
	
}

public class cRest :  ISMCondition 
{
	public TestSM e;
	public bool test()
	{
		return (e.giveLastSeenPos() == null);
	}
}

public class cRetreat :  ISMCondition 
{
	public TestSM e;
	public bool test()
	{
		return e.distanceToPlayer(e.giveTarget()) <= 5.5f;
	}
	
}

public class cAttack :  ISMCondition 
{
	public TestSM e;
	public bool test()
	{
		return  e.distanceToPlayer(e.giveTarget()) < 8.5f && e.distanceToPlayer(e.giveTarget()) > 5.5f;
	}
	
}

public class cSearch :  ISMCondition 
{
	public TestSM e;
	public bool test()
	{
			return (e.giveLastSeenPos ().HasValue) && !(e.canSeePlayer (e.giveTarget()));

	}
	
}

//All actions go here

//Add patrolling
public class aRest : ISMAction 
{
	public TestSM e;
	public void action()
	{
		e.ani.SetBool ("Moving", false);
		e.nav.Stop ();
	}
}

public class aApproach : ISMAction 
{
	public TestSM e;
	public void action()
	{
		e.ani.SetBool ("Moving", true);
		e.nav.destination = e.giveTarget ().transform.position;
	}
}

public class aAttack : ISMAction 
{
	public TestSM e;
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
public class aSearch : ISMAction 
{
	public TestSM e;
	public void action()
	{
		e.ani.SetBool ("Moving", true);
		e.nav.destination = (Vector3)e.giveLastSeenPos ();
	}
}

//Improve retreat AI
public class aRetreat : ISMAction 
{
	public TestSM e;
	public void action()
	{
		e.ani.SetBool ("Moving", true);
		e.nav.speed = 5;
		e.nav.destination = e.retreatPos;
	}
}

public class TestSM: Enemy{
	
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
		cApproach cApproach = new cApproach();
		cApproach.e = this;
		tApproach.addCondition(cApproach);

		cRest cRest = new cRest ();
		cRest.e = this;
		tRest.addCondition (cRest);

		cAttack cAttack = new cAttack ();
		cAttack.e = this;
		tAttack.addCondition (cAttack);

		cRetreat cRetreat = new cRetreat ();
		cRetreat.e = this;
		tRetreat.addCondition (cRetreat);

		cSearch cSearch = new cSearch ();
		cSearch.e = this;
		tSearch.addCondition (cSearch);


		//Set actions for the states
		aRest aRest = new aRest ();
		aRest.e = this;
		rest.addAction (aRest);

		aSearch aSearch = new aSearch ();
		aSearch.e = this;
		search.addAction (aSearch);

		aAttack aAttack = new aAttack ();
		aAttack.e = this;
		attack.addAction (aAttack);

		aApproach aApproach = new aApproach ();
		aApproach.e = this;
		approach.addAction (aApproach);

		aRetreat aRetreat = new aRetreat ();
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
