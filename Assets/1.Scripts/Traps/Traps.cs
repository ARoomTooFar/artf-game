using UnityEngine;
using System.Collections;

[System.Serializable]
public class TrapStats {
	public int damage;
}

public class Traps : MonoBehaviour {
	
	public TrapStats stats;

	// Use this for initialization
	protected virtual void Start () {
		setInitValues ();
	}

	protected virtual void setInitValues() {
		stats.damage = 1;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}
}
