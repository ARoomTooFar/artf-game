using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bushman : MobileEnemy {
	
	bool inFrenzy;
	float frenzy_counter;
	float frenzy_growth;
	PowerLevels tiers;

	
	protected override void Awake () {
		base.Awake ();
		inFrenzy = false;
		frenzy_counter = 0;
		frenzy_growth = 2;
		tiers = new PowerLevels ();
		StatsMultiplier tier0 = new StatsMultiplier (); 
		StatsMultiplier tier1 = new StatsMultiplier ();
		StatsMultiplier tier2 = new StatsMultiplier ();
		StatsMultiplier tier3 = new StatsMultiplier ();
		StatsMultiplier tier4 = new StatsMultiplier ();
		StatsMultiplier tier5 = new StatsMultiplier ();
		StatsMultiplier tier6 = new StatsMultiplier ();
	}

	
	protected override void Start() {
		base.Start ();
	}
	
	protected override void Update() {
		base.Update ();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 140;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=9;
		stats.luck=0;
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 5.0f;
	}

	/*
	protected bool isFlared(){
		return flared;
	}*/

	protected bool isFrenzy(){
		return inFrenzy;
	}

	/*
	protected bool isFlared(Character a){
		Bushman bushman = (Bushman) a;
		return bushman.isFlared();
	}*/

	protected bool isFrenzy(Character a){
		Bushman bushman = (Bushman) a;
		return bushman.isFrenzy ();
	}

	protected void Frenzy(){

	}
}
