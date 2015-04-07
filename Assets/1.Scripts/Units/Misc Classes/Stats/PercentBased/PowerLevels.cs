using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class StatsMultiplier{
	public float health, armor,maxHealth,rezCount;
	public float strength, coordination, speed, luck;
	public float atkSpeed;

	public float healthGrowth, armorGrowth, maxHealthGrowth,rezCountGrowth;
	public float strengthGrowth, coordinationGrowth, speedGrowth, luckGrowth;
	public float atkSpeedGrowth;
	
	public int minAtkRadius, maxAtkRadius;
	public float dmgAmp, dmgRed;
	public float dmgAmpGrowth, dmgRedGrowth;
}

public class PowerLevels{
	private class PowerStage
	{
		public int powerCount;
		public StatsMultiplier mult;
	}
	
	List<PowerStage> stages;
	int powvalue;
	int currentStage;
	Character target;
	Frenzy frenzy;
	
	public PowerLevels(){
		stages = new List<PowerStage>();
		powvalue = 0;
		currentStage = -1;
		frenzy = new Frenzy ();
		frenzy.setDmgBoost (0);
		frenzy.setDmgReduction (0);
		frenzy.setSpeedBoost (0);
		target.BDS.addBuffDebuff (frenzy, target.gameObject);
	}
	
	/*
	public int currentStage(){

	}*/
	
	public PowerLevels(Character c){
		stages = new List<PowerStage>();
		powvalue = 0;
		currentStage = -1;
		target = c;
		frenzy = new Frenzy ();
		frenzy.setDmgBoost (0);
		frenzy.setDmgReduction (0);
		frenzy.setSpeedBoost (0);
		target.BDS.addBuffDebuff (frenzy, target.gameObject);
	}
	
	public void addStage(StatsMultiplier statsbd, int powval){
		if(currentStage == -1) currentStage = 0;
		PowerStage newtier = new PowerStage ();
		newtier.powerCount = powval;
		newtier.mult = statsbd;
		stages.Add (newtier);
	}
	
	public StatsMultiplier getStatsBD(){
		return stages[currentStage].mult;
	}
	
	public void Update(){
		if (currentStage < stages.Count){
			if(powvalue > stages [currentStage].powerCount && currentStage < (stages.Count - 1)){
				currentStage++;
				ApplyRage();
			}
		} else if (powvalue < stages [currentStage].powerCount && currentStage > 0){
			currentStage--;
			ApplyRage();
		}
		
		//		Debug.Log (powvalue + " " + currentStage);
	}
	
	private void ApplyRage(){
		target.BDS.rmvBuffDebuff(frenzy, target.gameObject);
		frenzy.setDmgBoost (stages[currentStage].mult.dmgAmp);
		frenzy.setDmgReduction (stages[currentStage].mult.dmgRed);
		frenzy.setSpeedBoost (stages[currentStage].mult.speed);
		target.BDS.addBuffDebuff (frenzy, target.gameObject);
	}
	
	public void updateRage(int gaar){
		powvalue = gaar;
	}
	
	public void addRage(int gaaar){
		powvalue += gaaar;
		//		Debug.Log (powvalue);
	}
	
	public void resetStage(){
		currentStage = 0;
		powvalue = 0;
	}
	
}