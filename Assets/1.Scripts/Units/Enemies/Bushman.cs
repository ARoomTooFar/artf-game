using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bushman : MobileEnemy {
	
	bool inFrenzy;
	float frenzy_counter;
	float frenzy_growth;
	PowerLevels tiers;
	Frenzy frenzy;

	
	protected override void Awake () {
		base.Awake ();
		inFrenzy = false;
		frenzy_counter = 0;
		frenzy_growth = 2;
		tiers = new PowerLevels ();
		StatsMultiplier tier0 = new StatsMultiplier (); tier0.dmgAmp = 0f; tier0.dmgRed = 0f; tier0.speed = 0f;
		StatsMultiplier tier1 = new StatsMultiplier (); tier1.dmgAmp = 0.2f; tier1.dmgRed = -0.1f; tier1.speed = 0.1f;
		StatsMultiplier tier2 = new StatsMultiplier (); tier2.dmgAmp = 0.3f; tier2.dmgRed = -0.25f; tier2.speed = 0.2f;
		StatsMultiplier tier3 = new StatsMultiplier (); tier3.dmgAmp = 0.36f; tier3.dmgRed = -0.3f; tier3.speed = 0.25f;
		StatsMultiplier tier4 = new StatsMultiplier (); tier4.dmgAmp = 0.5f; tier4.dmgRed = -0.35f; tier4.speed = 0.4f;
		StatsMultiplier tier5 = new StatsMultiplier (); tier5.dmgAmp = 0.75f; tier5.dmgRed = -0.4f; tier5.speed = 0.6f;
		StatsMultiplier tier6 = new StatsMultiplier (); tier6.dmgAmp = 1.0f; tier6.dmgRed = -0.8f; tier6.speed = 1.2f;
		tiers.addTier(tier0, 0);
		tiers.addTier(tier1, 10);
		tiers.addTier(tier2, 20);
		tiers.addTier(tier3, 25);
		tiers.addTier(tier4, 36);
		tiers.addTier(tier5, 45);
		tiers.addTier(tier6, 60);
		frenzy = new Frenzy ();
		BDS.addBuffDebuff (frenzy, this.gameObject, 20);
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
