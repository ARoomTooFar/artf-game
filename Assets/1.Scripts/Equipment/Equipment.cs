using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponStats {
	public int damage, atkSpeed;
}

public class Equipment : MonoBehaviour {
	
	public WeaponStats stats;

	public ParticleEmitter particles;
	public ParticleAnimator partAnimator;

	// Use this for initialization
	protected virtual void Start () {
		particles = GetComponent<ParticleEmitter>();
		partAnimator = GetComponent<ParticleAnimator>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	}
}
