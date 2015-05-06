using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class TargetCircle : MonoBehaviour {

	protected bool _moveable;
	public bool moveable {
		get {return _moveable;}
		set {if (!value) this.rb.velocity = Vector3.zero;
			this._moveable = value;
		}
	}

	protected Rigidbody rb;

	protected Character user;
	protected Controls controls;
	protected float speed;

	//-------------------//
	// Primary Functions //
	//-------------------//

	protected virtual void Start () {
	}

	protected virtual void Update () {
		if (this.controls != null && this.moveable) {
			this.moveCommands();
		}
	}

	//--------------------//


	//------------------//
	// Public Functions //
	//------------------//

	// Sets the user of the target
	//     If a player, sets the controls equal to player controls
	//     If AI, they must call the move Circle function
	public virtual void setValues(Character user) {
		this.user = user;
		this.rb = this.GetComponent<Rigidbody> ();
		this.moveable = true;

		if (user.GetComponent<Player> () != null) {
			this.controls = user.GetComponent<NewPlayer>().controls;
		}

		this.speed = user.stats.speed * 3;
	}

	// AI uses this to move circle
	public virtual void moveCircle(Vector3 move) {
		if (this.moveable) {
			move.y = 0.0f;
			this.rb.velocity = move.normalized * this.speed;
		}
	}

	//-------------------//

	//---------------------//
	// Protected Functions //
	//---------------------//

	// How players move the circle, should be based off of their control scheme
	protected virtual void moveCommands() {
		Vector3 newMoveDir = Vector3.zero;

		//"Up" key assign pressed
		if (Input.GetKey(controls.up)) {
			newMoveDir += Vector3.forward;
		}

		//"Down" key assign pressed
		if (Input.GetKey(controls.down)) {
			newMoveDir += Vector3.back;
		}

		//"Left" key assign pressed
		if (Input.GetKey(controls.left)) {
			newMoveDir += Vector3.left;
		}

		//"Right" key assign pressed
		if (Input.GetKey(controls.right)) {
			newMoveDir += Vector3.right;
		}

		//Joystick form
		if(controls.joyUsed){
			newMoveDir = new Vector3(Input.GetAxis(controls.hori),0,Input.GetAxis(controls.vert));
		}
			
		this.rb.velocity = newMoveDir.normalized * this.speed;
	}

}