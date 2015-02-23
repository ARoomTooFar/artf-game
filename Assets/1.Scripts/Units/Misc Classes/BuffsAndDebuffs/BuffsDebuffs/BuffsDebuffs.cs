using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffsDebuffs {

	protected class BDData {
		public Character unit;
		public GameObject source;
		public CoroutineController controller;

		public BDData(Character unit, GameObject source) {
			this.unit = unit;
			this.source = source;
		}
	}

	protected Dictionary<Character, List<BDData>> affectedUnits;

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
		affectedUnits = new Dictionary<Character, List<BDData>>();
	}

	public void applyBD(Character unit, GameObject source) {
		List<BDData> list;
		BDData newData = new BDData(unit, source);

		bdEffects(newData);

		if (affectedUnits.TryGetValue (unit, out list)) {
			list.Add(newData);
		} else {
			list = new List<BDData>();
			list.Add(newData);
			affectedUnits[unit] = list;
		}
	}
	
	public void removeBD(Character unit, GameObject source) {
		List<BDData> list;
		
		if (affectedUnits.TryGetValue (unit, out list)) {
			if (list.Count > 0) {
				removeEffects(list[0], source);
				list.RemoveAt(0);
			}
			
			if (list.Count == 0) {
				affectedUnits.Remove(unit);
			}
		} else {
			Debug.LogWarning("Buff/Debuff \"" + this.name + "\" is not affecting " + unit.name);
		}
	}

	// What the buff/debuff does should go in here
	protected virtual void bdEffects(BDData newData) {
	}
	
	// What is stopped when buff/debuff is done/removed
	//     * Mostly for stopping coroutines, but popping effects can also go in here
	protected virtual void removeEffects(BDData oldData, GameObject source) {
	}

	public virtual void purgeBD(Character unit, GameObject source) {
	}
}