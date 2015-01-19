// Item script for all items

using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public Character player;

	protected float cooldown;
	public float curCoolDown;

	// Use this for initialization
	protected virtual void Start () {
		setInitValues();
	}

	protected virtual void setInitValues() {
		cooldown = 10.0f;
	}

	protected virtual void FixedUpdate() {
	}

	// Update is called once per frame
	protected virtual void Update () {
		if (curCoolDown > 0) {
			curCoolDown -= Time.deltaTime;
		}
	}

	// Called when character with an this item selected uses their item key
	public virtual void useItem() {
		curCoolDown = cooldown;
	}

	public virtual void deactivateItem() {
	}
	
}
