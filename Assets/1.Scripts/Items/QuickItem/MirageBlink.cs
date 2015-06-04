using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MirageBlink : Blink {

	public int tier;
	public GameObject image;
	public List<MirageImage> mirrors;

	// Use this for initialization
	protected override void Start () {
		base.Start();
		this.mirrors = new List<MirageImage>();
	}
	
	protected override void setInitValues() {
		base.setInitValues();
		cooldown = 2.0f;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}

	protected override void blinkFunc() {
		if (this.tier > 1) {
			MirageImage newImage = (Instantiate(image, user.transform.position, user.transform.rotation) as GameObject).GetComponent<MirageImage>();
			newImage.SetData(this, this.user.GetComponent<Mirage>(), this.tier > 3 ? 2 : 1);
			mirrors.Add(newImage);
		}

		base.blinkFunc();
	}
}
