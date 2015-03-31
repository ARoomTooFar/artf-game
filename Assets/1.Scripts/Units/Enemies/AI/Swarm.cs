using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Swarm : MonoBehaviour {

	private GameObject globalTarget;
	private List<GameObject> minions = new List<GameObject>();

	public AggroTable aggroTable;
	public GameObject target;
	public int priority;

	// Use this for initialization
	void Start () {
	}

	void Awake () {
		aggroTable = new AggroTable ();
	}
	
	// Update is called once per frame
	void Update () {
		target = aggroTable.getTarget ();
		priority = aggroTable.getVal ();
	}

	protected virtual void swarmTarget(GameObject newTarget)
	{
		//Change this value to as high as possible when I find out what a reasonable value is
		aggroTable.add (newTarget, 999);
	}
}
