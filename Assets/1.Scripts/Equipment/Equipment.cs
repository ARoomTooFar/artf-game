using UnityEngine;
using System.Collections;

public class Equipment : MonoBehaviour {

	public Character user;

	public ParticleSystem particles;
	
	protected int tier;

	// Use this for initialization
	protected virtual void Start () {
	}

	public virtual void Equip(Character u, int tier) {
		this.user = u;
		this.tier = tier;
		this.SetInitValues();
	}

	// Used for setting stats Sfor each equipment piece
	protected virtual void SetInitValues() {
	}

	// Update is called once per frame
	protected virtual void Update () {
	}
}
