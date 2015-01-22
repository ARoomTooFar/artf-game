using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HookShot : Item {

	public int hookDist;
	public float hookSpeed;

	[Range(0.0f, 2.0f)]
	public float chgLag;

	[Range(0.5f, 3.0f)]
	public float stunDuration;

	public float curhookTime;
	private float maxhookTime;
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
		cooldown = 1.0f;
		hookDist = 10;
		curhookTime = -1.0f;
		maxhookTime = 2.0f;
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
		if (curhookTime >= 0.0f) {
			curhookTime = Mathf.Clamp(curhookTime + Time.deltaTime, 0.0f, maxhookTime);
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
		// player.animator.SetTrigger("Charging Charge"); Once we have the animation for it
		
		curhookTime = 0.0f;
	}

	public override void deactivateItem() {
		if (curhookTime >= 0.0f) {
			base.deactivateItem();
			renderer.enabled = true;
			collider.enabled = true;
			hitEnd = false;
			hasHit = false;
			start.GetComponent<Collider>().enabled = false;
			StartCoroutine("hookFunc",((hookDist + curhookTime) * 0.1f));
		}
	}
	private IEnumerator hookFunc(float hookTime) {
		player.freeAnim = false;
		yield return StartCoroutine(hookTimeFunc((hookDist + curhookTime) * 0.1f));

		float tempStun = stunDuration;
		if(foe != null) {
			((IForcible<float>)foe.GetComponent(typeof(IForcible<float>))).pull(tempStun);
		}
		curCoolDown = cooldown + (curhookTime * 3);
		//player.freeAnim = true;
		curhookTime = -1.0f;
		start.GetComponent<Collider>().enabled = true;
		hitEnd = false;
		hasHit = true;
		foe = null;
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
			curhookTime = -1.0f;
		}
		//hasHit = true;
		IForcible<float> component = (IForcible<float>) other.GetComponent( typeof(IForcible<float>) );
		Character enemy = other.GetComponent<Character>();
		if( component != null && enemy != null) {
			start.GetComponent<Collider>().enabled = true;
			hasHit = true;
			foe = enemy;
			curhookTime = -1.0f;
		}
		if(other.tag == "HStart"){
			foe = null;
			hitEnd = true;
			player.freeAnim = true;
		}
	}
}
