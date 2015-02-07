using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//Interface for action classes
public interface ISMAction
{
	void action ();
	
}

//State class holds array of transitions, and the method to access them
public class State 
{
	private string id;
	public string sId()
	{
		return id;
	}

	private List<Transition> transitions = new List<Transition>();
	public List<Transition> getTransitions()
	{
		return transitions;
	}

	public void addTransition(Transition t){
		transitions.Add (t);
	}

	private ISMAction action;
	public void getAction()
	{
		action.action();
	}

	public void addAction(ISMAction act)
	{
		action = act;
	}

	public State(string n){
		id = n;
	}
}

//Interface for condition classes
public interface ISMCondition
{
	bool test ();
}

//Transitions have unique condition and action classes that inherit from the corresponding interfaces
//They have a target state, a condition, and an action. All of these also have methods to access them
public class Transition {

	private ISMCondition condition;
	public bool isTriggered()
	{
		return condition.test ();
	}

	private State targetState;
	public State getTargetState()
	{
		return targetState;
	}

	//Used to set the condition from outside this class
	public void addCondition(ISMCondition cond)
	{
		condition = cond;
		Debug.Log (cond);
	}

	public Transition(State t)
	{
		targetState = t;
	}
}


//The state machine keeps track of the states
public class StateMachine{
	
	public State initState;

	private State[] states;
	private State currState;
	private Transition triggeredTransition;


	// Use this for initialization
	public void Start()
	{
		currState = initState;
	}
	
	// Update is called once per frame
	public void Update () 
	{
		triggeredTransition = null;

		Debug.Log (currState.sId());

		List<Transition> transList = currState.getTransitions ();
		foreach (Transition t in transList) 
		{
			if(t.isTriggered())
			{
				triggeredTransition = t;
				break;
			}
		}

		if (triggeredTransition != null) 
		{
			currState = triggeredTransition.getTargetState();
		}

		currState.getAction ();
	}
}
