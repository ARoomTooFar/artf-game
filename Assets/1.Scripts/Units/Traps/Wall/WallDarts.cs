using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallDarts : Traps {

	public Animator animator;

	private Knockback debuff;
	protected AoETargetting aoe;
	protected ParticleSystem darts;
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		darts = GetComponent<ParticleSystem> ();
		aoe = this.GetComponent<AoETargetting>();
		aoe.affectEnemies = true;
		aoe.affectPlayers = true;
		debuff = new Knockback();
	}
	
	protected override void setInitValues() {
		base.setInitValues ();
		
		damage = 1;
	}
	
	protected override void FixedUpdate() {
		base.FixedUpdate();
	}
	
	// Update is called once per framea
	protected override void Update () {
		base.Update ();
	}

	public virtual void fireDarts() {
		darts.Emit (50);
	}


	public void unitEntered(Character entered) {
		this.animator.SetBool ("Fire", true);//this.fireDarts();
	}

	void OnParticleCollision(GameObject other) {
		IDamageable<int, Transform, GameObject> component = (IDamageable<int, Transform, GameObject>) other.GetComponent( typeof(IDamageable<int, Transform, GameObject>) );
		IForcible<Vector3, float> component2 = (IForcible<Vector3, float>) other.GetComponent( typeof(IForcible<Vector3, float>) );
		Character enemy = other.GetComponent<Character>();
		if( component != null && enemy != null) {
			enemy.damage(damage);

			if (component2 != null) {
				enemy.BDS.addBuffDebuff(debuff, this.transform.parent.gameObject, .5f);
			}
		}
	}

	void OnEnable() {
		if (aoe != null && aoe.unitsInRange != null) aoe.unitsInRange.Clear();
	}
}
