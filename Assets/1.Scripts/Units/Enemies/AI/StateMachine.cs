using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate bool AICondTest( Character agent );
public delegate void AIAction( Character agent );

//State class holds array of transitions, and the method to access them
public class State 
{
	protected string _id;
	public string id {
		get{ return _id ;}
	}

	protected AIAction action;
	protected AIAction exitAction;

	protected Character agent;

	protected List<Transition> transitions = new List<Transition>();

	public State(string n){
		_id = n;
	}

	public List<Transition> getTransitions() {
		return transitions;
	}

	public void addTransition(Transition t){
		transitions.Add (t);
	}

	public void removeTransition(Transition t) {
		if (transitions.Contains(t)) transitions.Remove(t);
		else Debug.LogWarning ("State " + _id + " in unit " + agent.name + " does not contain a transition to " + t.targetState.id);
	}

	public void getAction()
	{
		action(agent);
	}

	public void onExit(){
		if(exitAction != null){
			exitAction (agent);
		}
	}

	public void addAction(AIAction act, Character a)
	{
		action = act;
		agent = a;
	}

	public void addExitAction(AIAction act)
	{
		exitAction = act;
	}
}

//Transitions have unique condition and action classes that inherit from the corresponding interfaces
//They have a target state, a condition, and an action. All of these also have methods to access them
public class Transition {

	protected AICondTest condition;
	protected Character agent;

	protected State _targetState;
	public State targetState {
		get {return _targetState;}
	}

	public Transition(State t)
	{
		_targetState = t;
	}

	public bool isTriggered()
	{
		return condition(agent);
	}

	//Used to set the condition from outside this class
	public void addCondition(AICondTest cond, Character a)
	{
		condition = cond;
		agent = a;
	}
}


//The state machine keeps track of the states
public class StateMachine {
	
	public State initState;
	public Dictionary<string, State> states;

	// private State[] states;
	private State currState;
	private Transition triggeredTransition;


	public StateMachine() {
		states = new Dictionary<string, State>();
	}

	// Use this for initialization
	public void Start()
	{
		currState = initState;
	}
	
	// Update is called once per frame
	public void Update () 
	{
		triggeredTransition = null;

		// Debug.Log (currState.sId());

		List<Transition> transList = currState.getTransitions ();
		foreach (Transition t in transList) 
		{
			if(t.isTriggered())
			{
				triggeredTransition = t;
				currState.onExit();
				break;
			}
		}

		if (triggeredTransition != null) 
		{
			currState = triggeredTransition.targetState;

		}

		currState.getAction ();
	}
}
