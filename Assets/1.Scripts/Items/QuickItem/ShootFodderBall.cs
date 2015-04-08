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
	}
	
	protected virtual void shootBall(){
		curCircle = ((GameObject)Instantiate(targetCircle, new Vector3(user.transform.position.x, 0.55f, user.transform.position.z), user.transform.rotation)).GetComponent<TargetCircle>();
		curCircle.setValues (this.user);

		this.bullet = ((GameObject)Instantiate(projectile, user.transform.position, user.transform.rotation)).GetComponent<FodderBall>();
		this.bullet.setTarget (this.curCircle.gameObject);
		this.curCircle.moveable = false;
	}
}
