using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate bool AICondTest();
public delegate void AIAction();

//State class holds array of transitions, and the method to access them
public class State 
{
	protected string _id;
	public string id {
		get{ return _id ;}
	}

	protected AIAction enterAction;
	protected AIAction action;
	protected AIAction exitAction;

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
		else Debug.LogWarning ("State " + _id + " does not contain a transition to " + t.targetState.id);
	}

	public void onEnter() {
		if (enterAction != null) {
			enterAction();
		}
	}

	public void getAction() {
		action();
	}

	public void onExit(){
		if(exitAction != null){
			exitAction ();
		}
	}

	public void addEnterAction(AIAction act) {
		enterAction = act;
	}

	public void addAction(AIAction act) {
		action = act;
	}

	public void addExitAction(AIAction act) {
		exitAction = act;
	}
}

//Transitions have unique condition and action classes that inherit from the corresponding interfaces
//They have a target state, a condition, and an action. All of these also have methods to access them
public class Transition {

	protected AICondTest condition;

	protected State _targetState;
	public State targetState {
		get {return _targetState;}
	}

	public Transition(State t) {
		_targetState = t;
	}

	public bool isTriggered() {
		return condition();
	}

	//Used to set the condition from outside this class
	public void addCondition(AICondTest cond) {
		condition = cond;
	}
}


//The state machine keeps track of the states
public class StateMachine {
	
	public State initState;
	public Dictionary<string, State> states;
	public Dictionary<string, Transition> transitions;

	// private State[] states;
	private State currState;
	private Transition triggeredTransition;


	public StateMachine() {
		states = new Dictionary<string, State>();
		Debug.Log ("SM created");
		transitions = new Dictionary<string, Transition>();
	}

	// Use this for initialization
	public void Start() {
		currState = initState;
	}
	
	// Update is called once per frame
	public void Update () {
		triggeredTransition = null;

//		Debug.Log (currState.id);

		List<Transition> transList = currState.getTransitions ();

		foreach (Transition t in transList) {
			if(t.isTriggered()) {
				//Debug.Log (t.targetState.id);
				triggeredTransition = t;
				currState.onExit();
				break;
			}
		}

		if (triggeredTransition != null) {
			currState = triggeredTransition.targetState;
			currState.onEnter ();
		}

		currState.getAction ();
	}
}
