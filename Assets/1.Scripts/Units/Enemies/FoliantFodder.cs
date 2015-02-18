using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoliantFodder : MonoBehaviour {

	private GameObject[] fodders;

	private StateMachine fodderSM;

	// Use this for initialization
	protected virtual void Start () {
	
	}

	protected virtual void Awake () {
		fodders = GameObject.FindGameObjectsWithTag ("fodder");
		fodderSM = new StateMachine ();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}

	private void initStates(){
		State hive = new State ("hive");
		State attack = new State ("attack");

		fodderSM.initState = hive;

		Transition tHive = new Transition (hive);
		Transition tAttack = new Transition (attack);

		hive.addTransition (tAttack);
		attack.addTransition (tHive);
	}

	public bool isAttacking(Character a){
		return true;
	}

	public bool isHive(Character a){
		return true;
	}
}
