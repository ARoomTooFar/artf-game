using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate bool AICondTest( Character agent );
public delegate void AIAction( Character agent );

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

	private AIAction action;
	private Character agent;
	public void getAction()
	{
		action(agent);
	}

	public void addAction(AIAction act, Character a)
	{
		action = act;
		agent = a;
	}

	public State(string n){
		id = n;
	}
}

//Transitions have unique condition and action classes that inherit from the corresponding interfaces
//They have a target state, a condition, and an action. All of these also have methods to access them
public class Transition {

	private AICondTest condition;
	private Character agent;

	public bool isTriggered()
	{
		return condition(agent);
	}

	private State targetState;
	public State getTargetState()
	{
		return targetState;
	}

	//Used to set the condition from outside this class
	public void addCondition(AICondTest cond, Character a)
	{
		condition = cond;
		agent = a;
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

//		Debug.Log (currState.sId());

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
