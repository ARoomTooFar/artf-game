// Roll Item
// http://unitypatterns.com/scripting-with-coroutines/

using UnityEngine;
using System.Collections;

public class Roll : QuickItem {

	// private int rollInt; Once settled
	// private float rollSpeed; Once settled

	[Range(1, 4)]
	public int rollInt;
	[Range(1, 4)]
	public int rollSpeed;
	[Range(0.1f, 0.85f)]
	public float iFrameTime;

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}

	protected override void setInitValues() {
		cooldown = 5.0f;
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	// Called when character with an this item selected uses their item key
	public override void useItem() {
		base.useItem ();
		// player.animator.SetTrigger("Roll"); Once we have the animation for it
		player.freeAnim = false;

		StartCoroutine(rollFunc(rollInt * 0.1f));
	}

	protected override void animDone() {
		player.freeAnim = true;
		base.animDone();
	}

	// Once we have animation, we can base the timing/checks on animations instead if we choose/need to
	private IEnumerator rollFunc(float rollTime) {
		yield return StartCoroutine(rollTimeFunc(rollTime));
		yield return StartCoroutine(rollLagTime());

		animDone();
	}

	// Timer and velocity changing thing
	private IEnumerator rollTimeFunc(float rollTime) {
		for (float timer = 0; timer <= rollTime; timer += Time.deltaTime) {
			player.rigidbody.velocity = player.facing.normalized * player.stats.speed * 1.5f * rollSpeed;
			if (timer < rollTime * iFrameTime) player.invincible = true;
			else player.invincible = false;
			yield return 0;
		}
	}

	private IEnumerator rollLagTime() {
		for (float timer = 0; timer < iFrameTime/2; timer += Time.deltaTime) {
			player.rigidbody.velocity = Vector3.zero;
			yield return 0;
		}
	}
}
