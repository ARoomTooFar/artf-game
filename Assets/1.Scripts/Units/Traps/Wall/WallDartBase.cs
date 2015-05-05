// Base script for wall darts for animations and stuff

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallDartBase : MonoBehaviour {
	public WallDarts wall;

	private Animator animator;

	private void Awake() {
		this.animator = this.GetComponent<Animator>();
	}

	private void Start() {
		this.animator.GetBehaviour<WallDartIdleBehaviour>().SetVar(this.wall);
	}

	private void Update() {
	}


	public virtual void Fire() {
		wall.fireDarts();
	}
	
	public virtual void SetFire() {
		animator.SetBool ("DoneFire", true);
	}
}