using UnityEngine;
using System.Collections;


//Interface for action classes
interface ISMAction
{
	void action ();
}

//State class holds array of transitions, and the method to access them
public class State 
{
	private Transition[] transitions;
	public Transition[] getTransitions()
	{
		return transitions;
	}

	private ISMAction action;
	public void getAction()
	{
		action.action();
	}
}

//Interface for condition classes
interface ISMCondition
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

}


//The state machine keeps track of the states
public class StateMachine : MonoBehaviour {

	private State[] states;
	private State initState;
	private State currState;
	private Transition triggeredTransition;


	// Use this for initialization
	void Start () 
	{
		currState = initState;
	}
	
	// Update is called once per frame
	void Update () 
	{
		triggeredTransition = null;

		Transition[] transList = currState.getTransitions ();
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
