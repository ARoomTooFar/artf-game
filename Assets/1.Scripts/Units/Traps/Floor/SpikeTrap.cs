using UnityEngine;
using System.Collections;

public class SpikeTrap : Traps {
	
	public GameObject spike;

	protected float timeToReset;
	protected Vector3 spikeInitial;
	protected int unitsInTrap;
	protected bool firing;

	// Use this for initialization
	protected override void Start () {
		base.Start ();

		firing = true;
		unitsInTrap = 0;
		spikeInitial = spike.transform.localPosition;
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
	
	protected virtual void spikeRise() {
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
		if (unitsInTrap > 0) spikeRise ();
	}

	void OnTriggerEnter(Collider other) {
		unitsInTrap++;
		spikeRise ();
	}
	
	void OnTriggerExit(Collider other) {
		unitsInTrap--;
	}
}
