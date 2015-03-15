using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bushman : MobileEnemy {
	
	private bool inFrenzy;
	private float frenzy_counter;
	private float frenzy_growth;
	private PowerLevels powlvs;
	private Frenzy frenzy;
	private int tier;
	private float health;
	GameObject expDeath;
	
	protected override void Awake () {
		base.Awake ();
		inFrenzy = false;
		frenzy_counter = 0;
		frenzy_growth = 2;
		powlvs = new PowerLevels (this);
		StatsMultiplier stage0 = new StatsMultiplier (); stage0.dmgAmp = 0f; stage0.dmgRed = 0f; stage0.speed = 0f;
		StatsMultiplier stage1 = new StatsMultiplier (); stage1.dmgAmp = 0.2f; stage1.dmgRed = -0.1f; stage1.speed = 0.1f;
		StatsMultiplier stage2 = new StatsMultiplier (); stage2.dmgAmp = 0.3f; stage2.dmgRed = -0.25f; stage2.speed = 0.2f;
		StatsMultiplier stage3 = new StatsMultiplier (); stage3.dmgAmp = 0.36f; stage3.dmgRed = -0.3f; stage3.speed = 0.25f;
		StatsMultiplier stage4 = new StatsMultiplier (); stage4.dmgAmp = 0.5f; stage4.dmgRed = -0.35f; stage4.speed = 0.4f;
		StatsMultiplier stage5 = new StatsMultiplier (); stage5.dmgAmp = 0.75f; stage5.dmgRed = -0.4f; stage5.speed = 0.6f;
		StatsMultiplier stage6 = new StatsMultiplier (); stage6.dmgAmp = 1.0f; stage6.dmgRed = -0.8f; stage6.speed = 1.2f;
		powlvs.addStage(stage0, 0);
		powlvs.addStage(stage1, 10);
		powlvs.addStage(stage2, 20);
		powlvs.addStage(stage3, 25);
		powlvs.addStage(stage4, 36);
		powlvs.addStage(stage5, 45);
		powlvs.addStage(stage6, 60);
		setInitValues();
		health = stats.health;
	}

	void setTier(int tier){
		this.tier = tier;
	}

	protected override void Start() {
		base.Start ();
	}
	
	protected override void Update() {
		if(health > stats.health){
			health = stats.health;
			powlvs.addRage(Mathf.CeilToInt((float)(stats.maxHealth - stats.health)/stats.maxHealth * 150));
		}
//		Debug.Log (stats.health);
		powlvs.Update();
		base.Update ();
	}

	/*
	public override void die() {
		Debug.Log("IsDead");
		base.die();
		stats.health = 0;
		Renderer[] rs = GetComponentsInChildren<Renderer>();
		Explosion eDeath = ((GameObject)Instantiate(expDeath, transform.position, transform.rotation)).GetComponent<Explosion>();
		eDeath.setInitValues(this, true);
		foreach (Renderer r in rs) {
			r.enabled = false;
		}
	}*/

	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 60;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 10;
		stats.coordination=0;
		stats.speed=6;
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
