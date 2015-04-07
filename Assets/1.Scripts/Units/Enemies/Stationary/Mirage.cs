using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mirage : StationaryEnemy {

	public int rank = 5;

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
		else this.blink.rank = this.rank; // Put in check later for Rank
	}

	protected override void setInitValues() {
		base.setInitValues();
		stats.maxHealth = 20;
		stats.health = stats.maxHealth;
		stats.armor = 0;
		stats.strength = 20;
		stats.coordination=0;
		stats.speed=4;
		stats.luck=0;
		setAnimHash ();
		
		this.minAtkRadius = 0.0f;
		this.maxAtkRadius = 5.0f;
	}

	//-------------//
	// Transitions //
	//-------------//

	protected override bool isApproaching() {
		// If we don't have a target currently and aren't alerted, automatically assign anyone in range that we can see as our target
		if (this.deathTarget == null) {
			if (aRange.unitsInRange.Count > 0) {
				foreach(Character tars in aRange.unitsInRange) {
					if (this.canSeePlayer(tars.gameObject)) {
						this.alerted = true;
						this.target = tars.gameObject;
						deathTarget = tars.gameObject.GetComponent<Player>();
						deathTarget.BDS.addBuffDebuff (this.mark, this.gameObject);
						break;
					}
				}
				
				if (this.deathTarget == null) {
					return false;
				}
			} else {
				return false;
			}
		}
		
		float distance = this.distanceToPlayer(this.deathTarget.gameObject);
		
		if (distance >= this.maxAtkRadius && this.canSeePlayer (this.deathTarget.gameObject) && !isInAtkAnimation()) {
			return true;
		}
		return false;
	}

	protected override bool isAttacking() {
		if (this.deathTarget != null) {
			float distance = this.distanceToPlayer(this.deathTarget.gameObject);
			if (distance < this.maxAtkRadius && distance >= this.minAtkRadius) {
				this.facing = this.deathTarget.transform.position - this.transform.position;
				this.facing.y = 0.0f;
				this.transform.localRotation = Quaternion.LookRotation(facing);
				return true;
			}
		}
		return false;
	}

	protected override bool isSearching() {
		return this.deathTarget == null && !this.alerted;
	}

	//-------------//

	//---------//
	// Actions //
	//---------//

	protected override void Approach() {

		if (!this.stunned) {
			this.facing = this.deathTarget.transform.position - this.transform.position;
			this.facing.y = 0.0f;
			
			if (this.blink.curCoolDown <= 0) {
				do {
					this.facing.x = Random.value * (this.facing.x == 0 ? (Random.value - 0.5f) : (Mathf.Abs(this.facing.x)/this.facing.x));
					this.facing.z = Random.value * (this.facing.z == 0 ? (Random.value - 0.5f) : (Mathf.Abs(this.facing.z)/this.facing.z));
					this.facing.Normalize();
				} while (this.facing == Vector3.zero);
				
				this.blink.useItem();
			}
		}

	}

	protected override void Search() {
		if (this.deathTarget == null) {
			if (this.lastSeenPosition.HasValue) {
				this.facing = this.lastSeenPosition.Value - this.transform.position;
				this.facing.y = 0.0f;
				StartCoroutine(randomSearch());
				this.lastSeenPosition = null;
			}
		} else {
			this.facing = this.deathTarget.transform.position - this.transform.position;
			this.facing.y = 0.0f;
		}
	}
	//----------//

	//---------------------//
	// Inherited Functions //
	//---------------------//

	public override void die() {
		if (rank > 1) {
			if (this.blink.mirrors.Count > 0) {
				MirageImage imageToBe = this.blink.mirrors[(int)(Random.value * this.blink.mirrors.Count)];
				this.rank--;
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
