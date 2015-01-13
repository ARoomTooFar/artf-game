using UnityEngine;
using System.Collections;

public class Equipment : MonoBehaviour {

	public Character player;

	public ParticleSystem particles;

	// Use this for initialization
	protected virtual void Start () {

	}

	public void equip(Character play) {
		player = play;
		setInitValues();
	}

	// Used for setting stats for each equipment piece
	protected virtual void setInitValues() {
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	}
}
