using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallDarts : Traps {

	public Animator animator;

	private Knockback debuff;
	protected AoETargetting aoe;
	protected ParticleSystem darts;

	
	public AudioClip hurt, victory, failure;
	public bool playSound;
	
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
		
		damage = 10;
	}
	
	protected override void FixedUpdate() {
		base.FixedUpdate();
	}
	
	// Update is called once per framea
	protected override void Update () {
		base.Update ();
	}

	//Used to set sound effects
	public virtual void setSounds(AudioClip hurt, AudioClip victory, AudioClip failure, bool playSound){
		this.hurt = hurt;
		this.victory = victory;
		this.failure = failure;
		this.playSound = playSound;
	}

	public virtual void fireDarts() {
		darts.Emit (50);
	}

	public virtual void deathSound(){
		StartCoroutine (makeSound (failure, playSound, failure.length));
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

	public virtual IEnumerator makeSound(AudioClip sound, bool play, float duration){
		AudioSource.PlayClipAtPoint (sound, transform.position, 0.5f);
		play = false;
		yield return new WaitForSeconds (duration);
		play = true;
	}
}
