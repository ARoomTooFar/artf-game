using UnityEngine;
using System.Collections;

public class Equipment : MonoBehaviour {

	public Character user;

	public ParticleSystem particles;

	// Use this for initialization
	protected virtual void Start () {

	}

	public virtual void equip(Character u) {
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
