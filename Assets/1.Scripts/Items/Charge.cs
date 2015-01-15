// Charge Item
// Issues with current implementation
//     - Checking for walls to stop uses current collider
//     - If monster is too big for collder to catch it may bug out


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Charge : Item {
	
	public int chgDist;
	[Range(1, 4)]
	public int chargeSpeed;

	[Range(0.0f, 2.0f)]
	public float chgLag;

	[Range(0.5f, 3.0f)]
	public float stunDuration;

	public float curChgTime;
	private float maxChgTime;

	public List<Enemy> enemies;
	
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
		chgDist = 1;
		curChgTime = -1.0f;
		maxChgTime = 3.0f;

		hitWall = false;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();

		if (curChgTime >= 0.0f) {
			curChgTime = Mathf.Clamp(curChgTime + Time.deltaTime, 0.0f, maxChgTime);
		}

		//if (enemies.Count > 0) {
			foreach(Enemy ene in enemies) {
				ene.transform.position = transform.position;
			}
		// }
	}
	
	// Called when character with an this item selected uses their item key
	public override void useItem() {
		// player.animator.SetTrigger("Charging Charge"); Once we have the animation for it
		
		curChgTime = 0.0f;
	}

	public override void deactivateItem() {
		if (curChgTime >= 0.0f) {
			base.deactivateItem();
			// player.animator.SetTrigger("Charge Forward");

			collider.enabled = true;
			player.freeAnim = false;
			StartCoroutine(chargeFunc((chgDist + curChgTime) * 0.1f));
		}
	}
	
	// Once we have animation, we can base the timing/checks on animations instead if we choose/need to
	private IEnumerator chargeFunc(float chgTime) {
		yield return StartCoroutine(chgTimeFunc(chgTime));
		yield return StartCoroutine(chgLagTime());

		float tempStun = stunDuration * (hitWall ? 2 : 1);
		foreach(Enemy ene in enemies) {
			ene.stun(tempStun);
		}
		curCoolDown = cooldown + (curChgTime * 3);
		player.freeAnim = true;
		curChgTime = -1.0f;
		collider.enabled = false;
		hitWall = false;
		enemies.Clear();
	}
	
	// Timer and velocity changing thing
	private IEnumerator chgTimeFunc(float chgTime) {
		for (float timer = 0; timer <= chgTime; timer += Time.deltaTime) {

			if (hitWall) {
				player.rigidbody.velocity = Vector3.zero;
				yield break;
			}

			player.rigidbody.velocity = player.curFacing.normalized * player.stats.speed * 1.5f * chargeSpeed;
			yield return 0;
		}
	}
	
	private IEnumerator chgLagTime() {
		for (float timer = 0; timer < chgLag; timer += Time.deltaTime) {
			player.rigidbody.velocity = Vector3.zero;
			yield return 0;
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Wall") {
			hitWall = true;
		}

		Enemy enemy = other.GetComponent<Enemy>();
		if(enemy != null) {
			enemies.Add (enemy);
		}
	}
}
