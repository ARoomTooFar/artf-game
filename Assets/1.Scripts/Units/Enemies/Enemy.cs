// Parent scripts for enemy units

using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IDamageable<int> {
	
	// Use this for initialization
	protected virtual void Start () {
		
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}

	public virtual void damage(int dmgTaken) {
		print ("Fuck");
	}

	public virtual void OnTriggerEnter(Collider other) {
		damage (1);
	}
}