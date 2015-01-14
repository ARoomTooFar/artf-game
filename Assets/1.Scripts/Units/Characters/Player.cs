// Character class
// Base class that our heroes will inherit from

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Player : Character {
	public bool inGrey;
	public int testDmg;
	public int greyDamage;
	public bool testable;
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	protected override void setInitValues() {
		base.setInitValues();
		//Testing with base 0-10 on stats with 10 being 100/cap%
		stats.maxHealth = 40;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 0;
		stats.coordination=0;
		stats.speed=0;
		stats.luck=0;
		inGrey = false;
		greyDamage = 0;
		testDmg = 0;
		testable = true;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if(Input.GetKeyDown("space")&&testable) {
				testable = false;
				damage(testDmg);
				StartCoroutine(Wait(.5f));
		}
	}
	
	public override void damage(int dmgTaken) {
		if (!invincible) {
			print("UGH!" + dmgTaken);
			stats.health -= greyTest(dmgTaken);
			
		}
	}
	public virtual int greyTest(int damage){
		if(((greyDamage + damage) > stats.health) && ((greyDamage + damage) < stats.maxHealth)){
			stats.health = 0;
			stats.isDead = true;
			return 0;
		}
		if(((greyDamage + damage) >= stats.maxHealth) && stats.health == stats.maxHealth){
			stats.health = 1;
			greyDamage = stats.maxHealth - 1;
			inGrey = true;
			return 0;
		}		
		if(damage > (stats.maxHealth/20)){
			//print("Got Here"+(stats.maxHealth/20)+":"+damage);
			int tempDmg = greyDamage;
			if(inGrey){
				greyDamage = damage - stats.maxHealth/20;
				//print("Grey!:"+tempDmg);
				inGrey = true;
				StopCoroutine("RegenWait");
				if(inGrey &&!stats.isDead){
					StartCoroutine("RegenWait");
				}
				//print("True!WGAT:"+(stats.maxHealth/20 + tempDmg));
				return stats.health/20 + tempDmg;
			}else{
				inGrey = true;
				greyDamage = damage - stats.maxHealth/20;
				//print("Grey!:"+(damage - stats.maxHealth/20));
				//print("True!NGAT:"+stats.maxHealth/20);
				StopCoroutine("RegenWait");
				if(inGrey &&!stats.isDead){
					StartCoroutine("RegenWait");
				}
				return stats.maxHealth/20;
			}
		}
		if(inGrey){
			inGrey = false;
			//print("True!WGBT:"+(damage + greyDamage));
			return damage + greyDamage;
		}
		else{
			inGrey = false;
			//print("True!NG:"+damage);
			return damage;
		}
	}
	private IEnumerator Wait(float duration){
		for (float timer = 0; timer < duration; timer += Time.deltaTime){
			testable = true;
			yield return 0;
		}
	}
	private IEnumerator RegenWait(){
		yield return new WaitForSeconds(1);
		if(inGrey && !stats.isDead){
			print("Healed Grey and True");
			greyDamage--;
			if(greyDamage > 0){
				StartCoroutine("RegenWait");
			}
		}
		yield return 0;
	}
}