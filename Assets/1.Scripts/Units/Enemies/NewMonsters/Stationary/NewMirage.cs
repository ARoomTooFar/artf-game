using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewMirage : NewStationaryEnemy {

	public MirageWeapon leftClaw, rightClaw;

	protected List<MirageImage> images;
	protected MarkOfDeath mark;
	public Player deathTarget;
	protected MirageBlink blink;
	protected BlinkStrike blinkStrike;
	
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

		this.inventory.cycItems();
		this.blinkStrike = this.inventory.items[inventory.selected].GetComponent<BlinkStrike>();
		if (this.blinkStrike == null) Debug.LogWarning ("Mirage does not have BlinkStrike equipped");
		else this.blinkStrike.mirage = this;

		foreach(MirageBlinkBehaviour behaviour in this.animator.GetBehaviours<MirageBlinkBehaviour>()) {
			behaviour.SetVar (this.blink);
		}

		foreach (MirageBlinkStrike behaviour in this.animator.GetBehaviours<MirageBlinkStrike>()) {
			behaviour.SetVar (this.blinkStrike);
		}

		leftClaw.equip (this, opposition);
		rightClaw.equip (this, opposition);
	}

	public override void SetTierData(int tier) {
		tier = 2;

		base.SetTierData (tier);
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 35;
		stats.health = stats.maxHealth;
		stats.armor = 1;
		stats.strength = 8;
		stats.coordination=0;
		stats.speed=4;
		setAnimHash ();
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 3.5f;
	}
	
	//-------------//
	// Transitions //
	//-------------//

	//-------------//
	
	//---------//
	// Actions //
	//---------//
	
	public override void initAttack() {
		this.animator.SetTrigger("Attack");
	}

	protected virtual void LeftClawiderOn() {
		this.leftClaw.collideOn();
	}

	protected virtual void LeftClawiderOff() {
		this.leftClaw.collideOff();
	}

	protected virtual void RightClawiderOn() {
		this.rightClaw.collideOn();
	}

	protected virtual void RightClawiderOff() {
		this.rightClaw.collideOff();
	}

	protected virtual void Blink() {
		this.facing = this.deathTarget.transform.position - this.transform.position;
		this.facing.y = 0.0f;

		do {
			this.facing.x = Random.value * (this.facing.x == 0 ? (Random.value - 0.5f) : (Mathf.Abs(this.facing.x)/this.facing.x));
			this.facing.z = Random.value * (this.facing.z == 0 ? (Random.value - 0.5f) : (Mathf.Abs(this.facing.z)/this.facing.z));
			this.facing.Normalize();
		} while (this.facing == Vector3.zero);
			
		this.blink.useItem();
	}
	
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
				if (im != null) im.die(); //Destroy(im.gameObject);
			}
			this.isDead = true;
			this.deathTarget.BDS.rmvBuffDebuff(this.mark, this.gameObject);
			animator.SetTrigger("Died");
		}
		
	}

	public virtual void Death() {
		base.die ();
	}
	
	//---------------------//
}
