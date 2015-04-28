using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewMirage : NewStationaryEnemy {

	protected List<MirageImage> images;
	protected MarkOfDeath mark;
	public Player deathTarget;
	protected MirageBlink blink;
	
	// Mark of Death
	protected class MarkOfDeath : Stacking {
		
		public MarkOfDeath() {
			name = "MarkOfDeath";
			particleName = "MirageTargetParticle";
		}
		
		protected override void bdEffects(BDData newData) {
			base.bdEffects(newData);
		}
		
		protected override void removeEffects (BDData oldData, GameObject source) {
			base.removeEffects (oldData, source);
		}
		
		public override void purgeBD(Character unit, GameObject source) {
			base.purgeBD (unit, source);
		}
	}
	
	// Use this for initialization
	protected override void Awake () {
		base.Awake();
		
		this.deathTarget = null;
		this.mark = new MarkOfDeath();
	}
	
	protected override void Start() {
		base.Start ();

		this.blink = this.inventory.items[inventory.selected].GetComponent<MirageBlink>();
		if (this.blink == null) Debug.LogWarning ("Mirage does not have MirageBlink equipped");
		else this.blink.tier = this.tier; // Put in check later for tier

		foreach(MirageApproach behaviour in this.animator.GetBehaviours<MirageApproach>()) {
			behaviour.blink = this.blink;
		}
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 50;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 20;
		stats.coordination=0;
		stats.speed=4;
		stats.luck=0;
		setAnimHash ();
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 3.0f;
	}
	
	//-------------//
	// Transitions //
	//-------------//

	//-------------//
	
	//---------//
	// Actions //
	//---------//

	protected override void TargetFunction() {
		if (this.deathTarget != null) {
			target = this.deathTarget.gameObject;
			if (this.deathTarget.isDead) {
				deathTarget.BDS.rmvBuffDebuff(this.mark, this.gameObject);
				this.target = null;
				this.animator.SetBool ("Target", false);
				this.deathTarget = null;
			}
			if (this.canSeePlayer(target)) {
				float distance = Vector3.Distance(this.transform.position, this.target.transform.position);
				this.animator.SetBool ("InAttackRange", distance < this.maxAtkRadius && distance >= this.minAtkRadius);
			}
		} else {
			this.target = null;
			this.animator.SetBool ("Target", false);
			if (aRange.unitsInRange.Count > 0) {
				foreach(Character tars in aRange.unitsInRange) {
					if (tars.isDead) continue; 
					if (this.canSeePlayer(tars.gameObject)) {
						this.alerted = true;
						this.target = tars.gameObject;
						deathTarget = tars.gameObject.GetComponent<Player>();
						deathTarget.BDS.addBuffDebuff (this.mark, this.gameObject);
						this.animator.SetBool ("Target", true);
						break;
					}
				}
			}
		}
	}

	//----------//
	
	//---------------------//
	// Inherited Functions //
	//---------------------//
	
	public override void die() {
		if (tier > 1) {
			if (this.blink.mirrors.Count > 0) {
				MirageImage imageToBe = this.blink.mirrors[(int)(Random.value * this.blink.mirrors.Count)];
				this.tier--;
				this.transform.position = imageToBe.transform.position;
				imageToBe.die ();
			}
		} else {
			foreach (MirageImage im in this.blink.mirrors) {
				Destroy(im.gameObject);
			}
			this.deathTarget.BDS.rmvBuffDebuff(this.mark, this.gameObject);
			base.die ();
		}
		
	}
	
	
	//---------------------//
}
