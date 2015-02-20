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
	
	// 0 - Overwrites Existing, 1 - Does nothing if it exists already, 2 - Buffs/Debuffs that can stack together
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

	/*
	public virtual bool isBetter() {
	}*/

	public virtual void applyBD(Character unit) {
	}
	
	public virtual void removeBD(Character unit) {
	}

	public virtual void purgeBD(Character unit) {
	}
}