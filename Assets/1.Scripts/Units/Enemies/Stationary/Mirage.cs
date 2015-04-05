using UnityEngine;
using System.Collections;

public class Mirage : StationaryEnemy {

	protected MarkOfDeath mark;
	public Player deathTarget;
	protected Blink blink;

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

		this.blink = this.inventory.items[inventory.selected].GetComponent<Blink>();
		if (this.blink == null) Debug.LogWarning ("Mirage does not have blink equipped");
		else this.blink.cooldown = 2.0f;
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
		this.facing = this.deathTarget.transform.position - this.transform.position;
		this.facing.y = 0.0f;

		if (this.blink.curCoolDown <= 0) {
			this.blink.useItem();
			print("Blink");
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
}
