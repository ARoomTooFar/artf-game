using UnityEngine;
using System.Collections;

public class SpikeTrap : Traps {
	
	public GameObject spike;
	public AoETargetting aoe;

	protected Animator animator;
	protected float timeToReset;
	protected Vector3 spikeInitial;
	protected bool firing;

	// Use this for initialization
	protected override void Start () {
		base.Start ();

		firing = true;
		this.aoe = this.GetComponent<AoETargetting>();
		this.aoe.affectEnemies = true;
		this.aoe.affectPlayers = true;
		spikeInitial = spike.transform.localPosition;

		this.animator = this.GetComponent<Animator>();
		this.animator.GetBehaviour<SpikeTrapIdleBehaviour>().SetVar(this);
	}
	
	protected override void setInitValues() {
		base.setInitValues ();
		
		damage = 5;
		spike.GetComponent<HurtBox> ().damage = damage;
	}
	
	protected override void FixedUpdate() {
		base.FixedUpdate();
	}
	
	// Update is called once per framea
	protected override void Update () {
		base.Update ();
	}

	public void RaiseSpike() {
		StartCoroutine(riseUp());
	}

	protected virtual IEnumerator riseUp() {
		Vector3 newPos = spike.transform.localPosition + new Vector3 (0f, 3f, 0f);
		while (spike.transform.localPosition.y <= newPos.y - .1) {
			spike.transform.localPosition = Vector3.MoveTowards(spike.transform.localPosition, newPos, Time.deltaTime * 30);
			yield return null;
		}
		StartCoroutine(lower());
	}

	protected virtual IEnumerator lower() {
		while (spike.transform.localPosition.y >= spikeInitial.y + .1) {
			spike.transform.localPosition = Vector3.MoveTowards(spike.transform.localPosition, spikeInitial, Time.deltaTime * 30);
			yield return null;
		}
		StartCoroutine (countDown ());
	}

	protected virtual IEnumerator countDown() {
		timeToReset = 0.0f;
		while (timeToReset <= 1.0f) {
			timeToReset += Time.deltaTime;
			yield return null;
		}
		this.animator.SetBool("DoneFire", true);
	}

	public void unitEntered(Character entered) {
		this.animator.SetBool ("Fire", true);
	}

}
