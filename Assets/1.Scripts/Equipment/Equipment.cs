using UnityEngine;
using System.Collections;

public class Equipment : MonoBehaviour {

	public GameObject user;

	public ParticleSystem particles;

	// Use this for initialization
	protected virtual void Start () {

	}

	public void equip(GameObject u) {
		user = u;
		setInitValues();
	}

	// Used for setting stats Sfor each equipment piece
	protected virtual void setInitValues() {
	}

	protected virtual void FixedUpdate() {
	}

	// Update is called once per frame
	protected virtual void Update () {
	}
}
