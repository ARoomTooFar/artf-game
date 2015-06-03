using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Hook : ChargeItem {

	public GameObject origin;
	public int hookDist;
	
	private float hookSpeed;
	private Vector3 initialPos;
	private Collider col;
	private Renderer ren;
	private Rigidbody rb;
	private ParticleSystem particle;
	private bool hitWall;
	private Stun debuff;
	private Enemy ene;
	
	// Use this for initialization
	protected override void Start () {
		base.Start();
		
		this.debuff = new Stun();
		
		this.col = this.GetComponent<Collider>();
		this.col.isTrigger = true;
		this.col.enabled = false;
		
		this.ren = this.GetComponent<Renderer>();
		this.ren.enabled = false;
		
		this.rb = this.GetComponent<Rigidbody>();
		this.rb.isKinematic = true;
		
		this.particle = this.GetComponent<ParticleSystem>();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		
		hookSpeed = 30f;
		cooldown = 5.0f;
		hookDist = 1;
		maxChgTime = 3.0f;
		hitWall = false;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	// Called when character with an this item selected uses their item key
	public override void useItem() {
		this.transform.position = this.origin.transform.position;
		// this.ren.enabled = true;
		this.particle.simulationSpace = ParticleSystemSimulationSpace.Local;
		InvokeRepeating("ParticleEmit", 0f, 0.25f);
		this.ene = null;
		base.useItem ();
	}
	
	
	public override void deactivateItem() {
		this.rb.isKinematic = false;
		base.deactivateItem();
	}
	
	protected override void animDone() {
		this.user.animationLock = false;
		this.col.enabled = false;
		this.hitWall = false;
		this.ene = null;
		// this.ren.enabled = false;
		this.rb.velocity = Vector3.zero;
		this.rb.isKinematic = true;
		
		base.animDone ();
	}
	
	protected override void chgDone() {
		// user.animator.SetTrigger("Charge Forward");
		
		this.col.enabled = true;
		user.animationLock = true;
		StartCoroutine(hookFunc((hookDist + curChgTime) * 0.1f));
	}
	
	private IEnumerator hookFunc(float chgTime) {
		yield return StartCoroutine(hookTimeFunc(chgTime));
		this.col.enabled = false;
		if (hitWall) yield return StartCoroutine (this.PullUser());
		else yield return StartCoroutine (this.HookRetract());
		CancelInvoke();
		animDone();
	}
	
	
	
	// Timer and velocity changing thing
	private IEnumerator hookTimeFunc(float chgTime) {
		CancelInvoke();
		this.particle.Clear();
		this.particle.simulationSpace = ParticleSystemSimulationSpace.World;
		InvokeRepeating("ParticleEmit", 0.001f, 0.001f);
		for (float timer = 0; timer <= chgTime; timer += Time.deltaTime) {
			
			if (hitWall || this.ene != null) {
				this.rb.velocity = Vector3.zero;
				break;
			}

			this.rb.velocity = user.facing.normalized * hookSpeed;
			yield return 0;
		}
	}
	
	private void ParticleEmit() {
		this.particle.Emit(400);
	}
	
	private IEnumerator HookRetract() {
		while (Vector3.Distance(this.transform.position, this.origin.transform.position) > 0.5f) {
			this.rb.velocity = (this.origin.transform.position - this.transform.position).normalized * hookSpeed;
			
			
			if (ene != null) {
				ene.transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
				ene.BDS.addBuffDebuff(debuff, this.user.gameObject, 0.1f);
			}
			
			yield return 0;
		}
	}
	
	private IEnumerator PullUser() {
		while (Vector3.Distance(this.transform.position, this.origin.transform.position) > 1f) {
			this.user.rb.velocity = (this.transform.position - this.origin.transform.position).normalized * hookSpeed;
			yield return 0;
		}
	}
	
	protected void OnTriggerEnter (Collider other) {
		if (other.tag == "Wall") {
			hitWall = true;
		}

		if(other.tag == "Enemy") {
			this.ene = other.GetComponent<Enemy>();
		}
	}
}
