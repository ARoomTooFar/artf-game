// All character animation behaviours should derive from this
//     Used for general things that should happen when transitioning out of an animation

using UnityEngine;

public class PlayerBehaviour : StateMachineBehaviour {
	protected Player unit;
	
	public virtual void SetVar(Player unit) {
		this.unit = unit;
	}
}