using UnityEngine;
using System.Collections;

//All conditions need go here
public class cApproach :  ISMCondition {
	public TestSM e;
	public bool test(){
		return e.canSeePlayer ();
	}
	
}
public class cRest :  ISMCondition {
	public TestSM e;
	public bool test(){
		return !(e.canSeePlayer ());
	}
	
}
public class cRetreat :  ISMCondition {
	public TestSM e;
	public bool test(){
		return false;
	}
	
}
public class cAttack :  ISMCondition {
	public TestSM e;
	public bool test(){
		return false;
	}
	
}
public class cSearch :  ISMCondition {
	public TestSM e;
	public bool test(){
		return false;
	}
	
}

//All actions go here
public class actionRest : ISMAction {
	public void action(){
		
	}
}

public class TestSM: Enemy{
	
	//Public variables to tweak in inspector
	public float fov = 110f;
	public bool playerInSight = false;
	public Vector3 targetPosition;
	
	//Private variables for use in player detection
	private NavMeshAgent nav;
	private Animator ani;
	private SphereCollider col;
	private GameObject[] players;
	private GameObject target = null;
	private StateMachine testStateMachine;
	
	//Get players, navmesh and all colliders
	void Awake ()
	{
		nav = GetComponent<NavMeshAgent> ();
		col = GetComponent<SphereCollider> ();
		ani = GetComponent<Animator> ();
		players = GameObject.FindGameObjectsWithTag ("Player");
		testStateMachine = new StateMachine ();

		initStates ();
		testStateMachine.Start ();
	}
	
	protected override void Update()
	{
		base.Update ();
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
		actionRest actionRest = new actionRest ();
		rest.addAction (actionRest);
	}

	public bool canSeePlayer()
	{
		foreach (GameObject p in players) 
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
					return true;
					
					}
				}
			}
		}
	
		return false;
	}

	
	/*void attackPlayer (GameObject player)
	{
		Vector3 distanceToTarget = transform.position - targetPosition;
		if (distanceToTarget.sqrMagnitude > 12) 
		{
			nav.destination = targetPosition;
			ani.SetBool ("Moving", true);
		} else {
			nav.Stop ();
			if (!facingPlayer (player))
				transform.LookAt (targetPosition);
			ani.SetBool ("Moving", false);
			
			if (actable)
				gear.weapon.initAttack();
		}
	}*/

}
