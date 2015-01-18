using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HookShot : Item {

	public int hookDist;
	[Range(1, 4)]
	public int hookSpeed;

	[Range(0.0f, 2.0f)]
	public float chgLag;

	[Range(0.5f, 3.0f)]
	public float stunDuration;

	public float curhookTime;
	private float maxhookTime;
	public Transform target;
	public Transform start;
	public Enemy foe;
	private bool hasHit;
	private bool hitWall;
	

	private Collider collider;

	// Use this for initialization
	protected override void Start () {
		base.Start();

		collider = GetComponent<Collider>();
		collider.enabled = false;
	}
	
	protected override void setInitValues() {
		cooldown = 5.0f;
		hookDist = 10;
		curhookTime = -1.0f;
		maxhookTime = 3.0f;

		hitWall = false;
	}
	
	// Update is called once per frame
	protected override void Update() {
		base.Update();

		if (curhookTime >= 0.0f) {
			curhookTime = Mathf.Clamp(curhookTime + Time.deltaTime, 0.0f, maxhookTime);
		}
		if(!hasHit && collider.enabled){
			transform.position = Vector3.MoveTowards (transform.position, target.position, .35f);
		}else{
			transform.position = Vector3.MoveTowards (transform.position, start.position, .35f);
			if(foe != null) {
				foe.transform.position = transform.position;
			}
		}
	}
	
	public override void useItem() {
		// player.animator.SetTrigger("Charging Charge"); Once we have the animation for it
		
		curhookTime = 0.0f;
	}

	public override void deactivateItem() {
		if (curhookTime >= 0.0f) {
			base.deactivateItem();
			// player.animator.SetTrigger("Charge Forward");

			collider.enabled = true;
			//player.freeAnim = false;
			StartCoroutine(hookFunc((hookDist + curhookTime) * 0.1f));
		}
	}
	private IEnumerator hookFunc(float hookTime) {
		yield return StartCoroutine(hookTimeFunc(hookTime));
		//yield return StartCoroutine(chgLagTime());

		float tempStun = stunDuration;
		if(foe != null) {
			foe.stun(tempStun);
		}
		curCoolDown = cooldown + (curhookTime * 3);
		player.freeAnim = true;
		curhookTime = -1.0f;
		collider.enabled = false;
		hitWall = false;
		hasHit = false;
		foe = null;
	}
	
	// Timer and velocity changing thing
	private IEnumerator hookTimeFunc(float hookTime) {
		for (float timer = 0; timer <= hookTime; timer += Time.deltaTime) {

			if (hitWall) {
				hasHit = true;
				yield break;
			}
			if(!hasHit){
				transform.position = Vector3.MoveTowards (transform.position, target.position, .35f);
			}else{
				transform.position = Vector3.MoveTowards (transform.position, start.position, .35f);
			}
			yield return 0;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Wall") {
			hasHit = true;
			//particles.Stop();
			//Destroy(this.transform.parent.gameObject);
		}
		hasHit = true;
		IDamageable<int> component = (IDamageable<int>) other.GetComponent( typeof(IDamageable<int>) );
		Enemy enemy = other.GetComponent<Enemy>();
		if( component != null && enemy != null) {
			hasHit = true;
			foe = enemy;
			//particles.Stop();
			//Destroy(this.transform.parent.gameObject);
		}
	}
}
