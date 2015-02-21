using UnityEngine;
using System.Collections;

public class BuffsDebuffs {

	// protected Character affectedUnit;

	protected string _name;
	public string name {
		get {
			return _name;
		}
		protected set {
			_name = value;
		}
	}
	
	// 1 - Does nothing if it exists already, 2 - Buffs/Debuffs that can stack together
	protected int _bdType;
	public int bdType {
		get {
			return _bdType;
		}
		protected set {
			_bdType = value;
		}
	}
	
	protected BuffsDebuffs() {
	}

	public virtual void applyBD(Character unit, GameObject source) {
	}
	
	public virtual void removeBD(Character unit, GameObject source) {
	}

	public virtual void purgeBD(Character unit, GameObject source) {
	}
}