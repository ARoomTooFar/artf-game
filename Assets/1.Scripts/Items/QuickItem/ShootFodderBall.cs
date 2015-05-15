using UnityEngine;
using System.Collections;

public class ShootFodderBall : QuickItem {

	public GameObject targetCircle;
	public TargetCircle curCircle;
	
	public GameObject projectile;
	protected FodderBall bullet;

	// Use this for initialization
	protected override void Start () {
		base.Start();
	}
	
	protected override void setInitValues() {
		cooldown = 10.0f;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	// Called when character with an this item selected uses their item key
	public override void useItem() {
		base.useItem ();
		
		shootBall ();
		animDone();
	}
	
	protected virtual void shootBall(){
		Vector3 tarPos = Quaternion.AngleAxis(Random.Range (0f, 360f), Vector3.up) * (Vector3.right * 5f);
		curCircle = ((GameObject)Instantiate(targetCircle, this.transform.position + tarPos + targetCircle.transform.position, user.transform.rotation)).GetComponent<TargetCircle>();
		curCircle.setValues (this.user);

		this.bullet = ((GameObject)Instantiate(projectile, user.transform.position + Vector3.up, user.transform.rotation)).GetComponent<FodderBall>();
		this.bullet.setTarget (this.curCircle.gameObject);
		this.bullet.hive = (NewFoliantHive)this.user;
		this.curCircle.moveable = false;
	}
}
