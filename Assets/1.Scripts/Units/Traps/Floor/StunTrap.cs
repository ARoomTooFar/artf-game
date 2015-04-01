using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StunTrap : Traps {
	
	public GameObject spike;
	protected AoETargetting aoe;
	
	protected float timeToReset;
	protected Vector3 spikeInitial;
	protected bool firing;

	private Stun debuff;
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();

		debuff = new Stun();

		firing = true;
		this.aoe = this.GetComponent<AoETargetting>();
		spikeInitial = spike.transform.localPosition;
		
	}
	
	protected override void setInitValues() {
		base.setInitValues ();
	}
	
	protected override void FixedUpdate() {
		base.FixedUpdate();
	}
	
	// Update is called once per framea
	protected override void Update () {
		base.Update ();
	}
	
	protected virtual void stunBlast() {
		if (firing) {
			firing = false;
			StartCoroutine(riseUp());
		}
	}
	
	protected virtual IEnumerator riseUp() {
		Vector3 newPos = spike.transform.localPosition + new Vector3 (0f, 10f, 0f);
		while (spike.transform.localPosition.y <= newPos.y - .1) {
			spike.transform.localPosition = Vector3.MoveTowards(spike.transform.localPosition, newPos, Time.deltaTime * 60);
			yield return null;
		}
		foreach (Character suckers in this.aoe.unitsInRange) {//unitsInTrap) {
			suckers.BDS.addBuffDebuff(debuff, this.gameObject, 1.0f);
		}
		StartCoroutine(lower());
	}
	
	protected virtual IEnumerator lower() {
		while (spike.transform.localPosition.y >= spikeInitial.y + .1) {
			spike.transform.localPosition = Vector3.MoveTowards(spike.transform.localPosition, spikeInitial, Time.deltaTime * 5);
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
		firing = true;
		if (this.aoe.unitsInRange.Count > 0) stunBlast ();
	}

	public void unitEntered(Character entered) {
		stunBlast ();
	}
}
