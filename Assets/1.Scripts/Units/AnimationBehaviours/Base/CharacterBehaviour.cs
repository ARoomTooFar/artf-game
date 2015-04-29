// All character animation behaviours should derive from this
//     Used for general things that should happen when transitioning out of an animation

using UnityEngine;

public class CharacterBehaviour : StateMachineBehaviour {
	protected Character unit;
	
	public virtual void SetVar(Character unit) {
		this.unit = unit;
	}
}