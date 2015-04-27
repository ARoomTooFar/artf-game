using UnityEngine;

public class VinePull : Approach {

	public Stun stun;
	public MeleeWeapons weap;
	public CVSensor feeler;
	public GameObject tether;
	public Vector3 MyMawPos;
	private Player p;

	// This will be called when the animator first transitions to this state.
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		tether = GameObject.Find ("CVFeelers");
		feeler = tether.GetComponent<CVSensor> ();
		p = unit.target.GetComponent<Player> ();
	}
	
	// This will be called once the animator has transitioned out of the state.
	public override void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
	}


	public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		unit.facing = unit.target.transform.position - unit.transform.position;
		unit.facing.y = 0.0f;
		if (!feeler.Hooked ()) {
			tether.transform.RotateAround (unit.transform.position, Vector3.up, 50 * Time.deltaTime);
		} else if (MyMawPos != null) {
			unit.target.transform.position = unit.target.transform.position - pullVelocity (MyMawPos);
			if (unit.actable && !unit.attacking){
				unit.gear.weapon.initAttack();
			}
		} else { 
			unit.target.transform.position = unit.target.transform.position - pullVelocity (unit.transform.position);
			if (unit.actable && !unit.attacking){
				unit.gear.weapon.initAttack();
			}

		}


	}

	private Vector3 pullVelocity(Vector3 destination){
		Vector3 dir = unit.target.transform.position - destination;
		float time =  dir.magnitude/((NewCableVine)unit).pull_velocity;
		Vector3 velocity = new Vector3 ();
		velocity.x = dir.x / time;
		velocity.y = dir.y / time;
		velocity.z = dir.z / time;
		return velocity;
	}

	private Vector3 pullVelocity(float pull_velocity){
		float time = unit.facing.magnitude/pull_velocity;
		Vector3 velocity = new Vector3 ();
		velocity.x = unit.facing.x / time;
		velocity.y = unit.facing.y / time;
		velocity.z = unit.facing.z / time;
		return velocity;
	}
}
