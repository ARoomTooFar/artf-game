using UnityEngine;
using System.Collections;

public class Equipment : MonoBehaviour {

	public Character player;

	public ParticleSystem particles;

	// Use this for initialization
	protected virtual void Start () {

	}

	public void equip(GameObject play) {
		player = play.GetComponent<Character>();
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
