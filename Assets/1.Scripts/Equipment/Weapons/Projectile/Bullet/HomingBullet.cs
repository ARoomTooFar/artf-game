using UnityEngine;
using System.Collections;
using System;

public class HomingBullet : Bullet {

	protected Character target = null;

	// Use this for initialization
	protected override void Start() {
		base.Start();
	}
	
	public virtual void setInitValues(Character player, Type opposition, int damage, float partSpeed, bool effect, BuffsDebuffs hinder, float durations, Character target) {
		this.target = target;
		base.setInitValues (player, opposition, damage, partSpeed, effect, hinder, duration);
	}
	
	// Update is called once per frame
	protected override void Update() {
		base.Update();
		
		if (target == null) return;
		
		Vector3 tarPos = target.transform.position;
		float angle = Vector2.Angle(new Vector2(tarPos.x - this.transform.position.x, tarPos.z - this.transform.position.z), new Vector2(this.rb.velocity.x, this.rb.velocity.z));
		
		Vector3 cross = Vector3.Cross(new Vector2(tarPos.x - this.transform.position.x, tarPos.z - this.transform.position.z), new Vector2(this.rb.velocity.x, this.rb.velocity.z));
		
		if (cross.z < 0) angle = 360 - angle;
		
		Quaternion turnAngle = Quaternion.AngleAxis(angle, Vector3.up);
		
		this.transform.rotation = this.transform.rotation * turnAngle;
		this.rb.velocity = turnAngle * this.rb.velocity;
	}
}
