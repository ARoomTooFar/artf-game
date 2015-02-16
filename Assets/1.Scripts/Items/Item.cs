// Item script for all items

using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	public Character user;

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
	}

	// Called when character with an this item selected uses their item key
	public virtual void useItem() {
		curCoolDown = cooldown;
	}

	public virtual void deactivateItem() {

	}

	// Called when the skill is completely done (animation wise mostly)
	//     Mainly used to start the cooldown period
	protected virtual void animDone() {
		StartCoroutine(bgnCooldown());
	}

	// Change to virtual if it seems there are items that do things while it cools down
	protected IEnumerator bgnCooldown() {
		for(int i = 0; i <= curCoolDown; curCoolDown -= Time.deltaTime) {
			yield return null;
		}
	}
}
