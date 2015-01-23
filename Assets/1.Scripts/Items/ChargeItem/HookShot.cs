using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HookShot : ChargeItem {

	public int hookDist;
	public float hookSpeed;

	[Range(0.0f, 2.0f)]
	public float chgLag;

	[Range(0.5f, 3.0f)]
	public float stunDuration;

	public GameObject target;
	public GameObject start;
	public Character foe;
	public bool hasHit;
	public bool hitEnd;
	

	private Collider collider;

	// Use this for initialization
	protected override void Start () {
		base.Start();

		collider = GetComponent<Collider>();
		collider.enabled = false;
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		cooldown = 1.0f;
		hookDist = 10;
		maxChgTime = 2.0f;
		hookSpeed = .2f;
		hitEnd = false;
	}
	
	// Update is called once per frame
	protected override void Update() {
		base.Update();
		if(hitEnd && hasHit){
			start.GetComponent<Collider>().enabled = false;
			renderer.enabled = false;
		}
		
		if(!hasHit && !start.GetComponent<Collider>().enabled){
			transform.position = Vector3.MoveTowards (transform.position, target.transform.position, hookSpeed);
		}else if(hasHit && start.GetComponent<Collider>().enabled){
			transform.position = Vector3.MoveTowards (transform.position, start.transform.position, hookSpeed);
			if(foe != null && !hitEnd) {
				foe.transform.position = transform.position;
			}
		}
	}
	
	public override void useItem() {
		base.useItem ();
		// player.animator.SetTrigger("Charging Charge"); Once we have the animation for it
	}

	public override void deactivateItem() {
		base.deactivateItem();
	}

	protected override void chgDone() {
		renderer.enabled = true;
		collider.enabled = true;
		hitEnd = false;
		hasHit = false;
		start.GetComponent<Collider>().enabled = false;
		StartCoroutine("hookFunc",((hookDist + curChgTime) * 0.1f));
	}

	protected override void animDone() {
		float tempStun = stunDuration;
		if(foe != null) {
			((IForcible<float>)foe.GetComponent(typeof(IForcible<float>))).pull(tempStun);
		}
		//player.freeAnim = true;
		start.GetComponent<Collider>().enabled = true;
		hitEnd = false;
		hasHit = true;
		foe = null;

		base.animDone ();
	}

	private IEnumerator hookFunc(float hookTime) {
		player.freeAnim = false;
		yield return StartCoroutine(hookTimeFunc((hookDist + curChgTime) * 0.1f));
		
		animDone();
	}
	
	// Timer and velocity changing thing
	private IEnumerator hookTimeFunc(float hookTime) {
		for (float timer = 0; timer <= hookTime; timer += Time.deltaTime) {

			if (hitEnd) {
				hasHit = true;
				yield break;
			}
			if(!hasHit && !start.GetComponent<Collider>().enabled){
				transform.position = Vector3.MoveTowards (transform.position, target.transform.position, hookSpeed);
			}else{
				transform.position = Vector3.MoveTowards (transform.position, start.transform.position, hookSpeed);
				if(foe != null && !hitEnd) {
					foe.transform.position = transform.position;
				}
			}
			yield return 0;
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Wall") {
			hasHit = true;
			curChgTime = -1.0f;
		}
		//hasHit = true;
		IForcible<float> component = (IForcible<float>) other.GetComponent( typeof(IForcible<float>) );
		Character enemy = other.GetComponent<Character>();
		if( component != null && enemy != null) {
			start.GetComponent<Collider>().enabled = true;
			hasHit = true;
			foe = enemy;
			curChgTime = -1.0f;
		}
		if(other.tag == "HStart"){
			foe = null;
			hitEnd = true;
			player.freeAnim = true;
		}
	}
}
