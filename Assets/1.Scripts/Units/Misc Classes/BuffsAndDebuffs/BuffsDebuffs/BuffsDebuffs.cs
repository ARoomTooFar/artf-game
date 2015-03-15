using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffsDebuffs {

	protected string _particleName;
	protected string particleName {
		get {return _particleName;}
		set {
			if (!string.IsNullOrEmpty (value)) {
				_particleName = value;
				this.particles = Resources.Load ("BDParticles/" + _particleName, typeof(GameObject)) as GameObject;

				if (this.particles == null) {
					Debug.LogWarning ("No particle object with name \"" + value + "\" found in Resource folder under BDParticles, check to make sure the name is correct and/or it is in the folder.");
				}
			}
		}
	}

	protected GameObject particles;

	protected class BDData {
		public Character unit;
		public GameObject source;
		public CoroutineController controller;
		public GameObject particles;

		public BDData(Character unit, GameObject source, GameObject particles) {
			this.unit = unit;
			this.source = source;
			this.particles = particles;
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

		GameObject newParticle = null;

		if (particles != null) {
			newParticle = Object.Instantiate (particles) as GameObject;
			newParticle.transform.SetParent (unit.transform, false);
		}

		BDData newData = new BDData(unit, source, newParticle);
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
		if (oldData.particles != null) Object.Destroy (oldData.particles);
	}

	public virtual void purgeBD(Character unit, GameObject source) {
	}
}