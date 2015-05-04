// Applies knockback to unit towards the Vector3 supplied
//     If no direction is supplied it will instead find the direction based on source of debuff and affected unit

using UnityEngine;
using System.Collections;

public class Knockback : Override {

	private float speed;
	private Vector3 direction;

	public Knockback() {
		name = "Knockback";
		this.direction = Vector3.zero;
		this.speed = 5.0f;
	}

	public Knockback(float speed) {
		name = "Knockback";
		this.direction = Vector3.zero;

		if (speed <= 0) Debug.LogWarning("Knockback speed cannot be less than or equal to zero");
		else this.speed = speed;
	}

	public Knockback(Vector3 direction) {
		name = "Knockback";
		this.direction = direction;
		this.speed = 5.0f;
	}

	public Knockback(Vector3 direction, float speed) {
		name = "Knockback";
		this.direction = direction;
		
		if (speed <= 0) Debug.LogWarning("Knockback speed cannot be less than or equal to zero");
		else this.speed = speed;
	}

	protected override void bdEffects(BDData newData) {
		base.bdEffects(newData);

		Vector3 pushBackDir = this.direction;

		if (pushBackDir == Vector3.zero) {
			pushBackDir = newData.unit.transform.position - newData.source.transform.position;
		}
		pushBackDir.y = 0.0f;
		newData.unit.knockback(pushBackDir, this.speed);
		//newData.unit.actable = false;
	}
	
	protected override void removeEffects (BDData oldData, GameObject source) {
		base.removeEffects (oldData, source);
		//Debug.Log ("GotHere");
		//oldData.unit.actable = true;
		oldData.unit.stabled();
	}
	
	public override void purgeBD(Character unit, GameObject source) {
		base.purgeBD (unit, source);
		//Debug.Log ("GotHere");
		//unit.actable = true;
		unit.stabled();
	}
	
	public override bool isBetter(BuffsDebuffs newBD, float newDuration, float timeLeft) {
		return true;
	}
}
