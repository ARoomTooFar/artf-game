using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponStats {
	public int damage, atkSpeed;
}

public class Equipment : MonoBehaviour {
	
	public WeaponStats stats;

	public ParticleSystem particles;

	// Use this for initialization
	protected virtual void Start () {

	}
	
	// Update is called once per frame
	protected virtual void Update () {
	}
}
