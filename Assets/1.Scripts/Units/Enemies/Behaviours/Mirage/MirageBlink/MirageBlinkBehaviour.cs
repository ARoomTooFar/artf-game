// For Behaviours that need a Blink reference

using UnityEngine;

public class MirageBlinkBehaviour : EnemyBehaviour {
	
	protected MirageBlink blink;
	
	public virtual void SetVar(MirageBlink blink) {
		this.blink = blink;
	}
}