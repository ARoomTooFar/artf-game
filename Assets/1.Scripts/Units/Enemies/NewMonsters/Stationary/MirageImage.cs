using UnityEngine;
using System.Collections;

// Inherits from mirage just to get enemy component
public class MirageImage : NewMirage {
	
	public MirageBlink spawnedBy;
	public NewMirage user;

	public int hitsToKill;

	protected override void Awake () {
		this.animator = this.GetComponent<Animator>();
		this.BDS = new BuffDebuffSystem(this);
		this.rb = this.GetComponent<Rigidbody>();
	}

	// Use this for initialization
	protected override void Start () {
	
	}
	
	// Update is called once per frame
	protected override void Update () {

		if (this.user.deathTarget == null) return;
		Vector3 facing = this.user.deathTarget.transform.position - this.transform.position;
		facing.y = 0.0f;
		this.transform.localRotation = Quaternion.LookRotation(facing);
	}

	public override void damage(int dmgTaken, Transform atkPosition, GameObject source) {
		this.damage(dmgTaken);
	}

	public override void damage(int dmgTaken, Transform atkPosition) {
		this.damage(dmgTaken);
	}
	
	public override void damage(int dmgTaken) {
		if (--this.hitsToKill <= 0) {
			this.die(); 
		}
	}

	public override void die() {
		this.animator.SetTrigger("IllusionOut");
	}

	public virtual void Death() {
		spawnedBy.mirrors.Remove(this);
		Destroy(this.gameObject);
	}
}
