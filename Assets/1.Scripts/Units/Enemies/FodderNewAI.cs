using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class FodderNewAI: MobileEnemy {
	
	protected override void Awake () {
		base.Awake ();
	}
	
	protected override void Start() {
		base.Start ();
	}
	
	protected override void Update() {
		base.Update ();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 40;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=7;
		stats.luck=0;
		
		this.minAtkRadius = 20.0f;
		this.maxAtkRadius = 100.0f;
	}

	protected override void initStates() {
		
		// Initialize all states
		State rest = new State("rest");
		State approach = new State("approach");
		State attack = new State ("attack");
		State atkAnimation = new State ("attackAnimation");
		
		
		// Add all the states to the state machine
		sM.states.Add (rest.id, rest);
		sM.states.Add (approach.id, approach);
		sM.states.Add (attack.id, attack);
		sM.states.Add (atkAnimation.id, atkAnimation);
		
		
		// Set initial state for the State Machine of this unit
		sM.initState = rest;
		
		
		// Initialize all transitions
		Transition tRest = new Transition(rest);
		Transition tApproach = new Transition (approach);
		Transition tAttack = new Transition (attack);
		Transition tAtkAnimation = new Transition(atkAnimation);
		
		
		// Set conditions for the transitions
		tApproach.addCondition(isApproaching, this);
		tRest.addCondition (isResting, this);
		tAttack.addCondition (isAttacking, this);
		tAtkAnimation.addCondition (isInAtkAnimation, this);
		
		
		// Set actions for the states
		rest.addAction (Rest, this);
		approach.addAction (Approach, this);
		attack.addAction (Attack, this);
		atkAnimation.addAction (AtkAnimation, this);
		
		
		// Set the transitions for the states
		rest.addTransition (tApproach);
		approach.addTransition (tAttack);
		attack.addTransition (tAtkAnimation);
		attack.addTransition (tRest);
		atkAnimation.addTransition (tApproach);
		atkAnimation.addTransition (tAttack);
	}
}
