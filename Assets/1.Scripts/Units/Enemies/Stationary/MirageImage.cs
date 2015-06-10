using UnityEngine;
using System.Collections;

// Inherits from mirage just to get enemy component
public class MirageImage : Mirage {
	
	public MirageBlink spawnedBy;
	public Mirage user;

	public int hitsToKill;

	protected override void Awake () {
		this.animator = this.GetComponent<Animator>();
		this.BDS = new BuffDebuffSystem(this);
		this.rb = this.GetComponent<Rigidbody>();
		
		
	}

	// Use this for initialization
	protected override void Start () {
	
	}
	
	public virtual void SetData(MirageBlink spawnedBy, Mirage user, int hitsToKill) {
		this.spawnedBy = spawnedBy;
		this.user = user;
		this.hitsToKill = hitsToKill;
		GameObject hr = Instantiate (user.healthRing, this.transform.position, Quaternion.identity) as GameObject;
		RectTransform hrt = hr.GetComponent<RectTransform>();
		hr.transform.SetParent(this.transform);
		hrt.position = hrt.transform.position + new Vector3(0f, 0.75f, 0f);
		hr.GetComponent<EnemyHealthRingController>().enemy = this.user.GetComponent<Enemy>();
	}
	
	protected override void SetTierData(int tier) {
		tier = -1;
		base.SetTierData (tier);
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

	protected override void Death() {
		spawnedBy.mirrors.Remove(this);
		Destroy(this.gameObject);
	}
}
