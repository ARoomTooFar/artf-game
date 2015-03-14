// Buff and Debuff system that tracks on buff and debuffs on this unit
//     In the future, seperation of Buffs and Debuffs may be neccessary if we have friendly/hostile purges

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffDebuffSystem {

	private class BD {
		public BuffsDebuffs bd;
		public float timeStart, maxDuration;
		public CoroutineController duration;

		public BD(BuffsDebuffs bd) {
			this.bd = bd;
		}

		public float timeLeft() {
			return maxDuration - (Time.time - timeStart);
		}
	}

	private Character affectedUnit;
	private Dictionary<string, List<BD>> buffsAndDebuffs;

	public BuffDebuffSystem(Character unit) {
		affectedUnit = unit;
		buffsAndDebuffs = new Dictionary<string, List<BD>>();
	}

	//---------------------------------------------------------//
	// Functions that add and apply timed buffs/debuff go here //
	//---------------------------------------------------------//
	
	public void addBuffDebuff(BuffsDebuffs newBD, GameObject source, float duration) {
		// 1 - Does nothing if it exists already, 2 - Buffs/Debuffs that can stack together
		switch (newBD.bdType) {
			case 0: // If new buff is better, overwrite old buff/debuff
				addOverride(newBD, source, duration);
				break;
			case 1: // If buff/debuff exists already, do nothing, else apply it on
				addSingular(newBD, source, duration);
				break;
			case 2: // If there is already an instance of this buff/debuff, add another to the list
				addStacking(newBD, source, duration);
				break;
			default:
				Debug.LogWarning("Buff/Debuff has no appropriate type");
				break;
		}
	}

	// For adding buffs/debuffs that override existing ones
	private void addOverride(BuffsDebuffs newBD, GameObject source, float duration) {
		BD newBuffDebuff = new BD(newBD);
		List<BD> list;
		
		if (buffsAndDebuffs.TryGetValue (newBD.name, out list)) {
			if ((list[0].bd as Override).isBetter(newBD, duration, list[0].timeLeft ())) { // Check if new (de)buff will be better than old one
				list[0].duration.Stop();
				list[0].bd.removeBD(affectedUnit, source);
				list.RemoveAt(0);

				list.Add(newBuffDebuff);
				setBD(newBuffDebuff, source, duration);
			}
		} else {
			list = new List<BD>();
			list.Add(newBuffDebuff);
			buffsAndDebuffs[newBD.name] = list;

			setBD(newBuffDebuff, source, duration);
		}
	}

	// For adding buffs/debuffs that don't do anything if unit is already affected by an instance of it
	private void addSingular(BuffsDebuffs newBD, GameObject source, float duration) {
		if (!buffsAndDebuffs.ContainsKey(newBD.name)) {
			BD newBuffDebuff = new BD(newBD);
			List<BD> list = new List<BD>();
			list.Add(newBuffDebuff);
			buffsAndDebuffs[newBD.name] = list;
			
			setBD(newBuffDebuff, source, duration);
		}
	}
	
	// For adding buffs/debuffs that can stack
	private void addStacking(BuffsDebuffs newBD, GameObject source, float duration) {
		BD newBuffDebuff = new BD(newBD);
		List<BD> list;
		
		if (buffsAndDebuffs.TryGetValue (newBD.name, out list)) {
			list.Add (newBuffDebuff);
		} else {
			list = new List<BD>();
			list.Add(newBuffDebuff);
			buffsAndDebuffs[newBD.name] = list;
		}
		setBD(newBuffDebuff, source, duration);
	}

	private void setBD(BD newBD, GameObject source, float duration) {
		newBD.bd.applyBD(affectedUnit, source);
		newBD.maxDuration = duration;
		newBD.timeStart = Time.time;
		affectedUnit.StartCoroutineEx(timedRemoval(newBD.bd, source, duration), out newBD.duration);
	}

	private IEnumerator timedRemoval(BuffsDebuffs bdToRemove, GameObject source, float duration) {
		while (duration > 0.0f) {
			duration -= Time.deltaTime;
			yield return null;
		}
		rmvBuffDebuff(bdToRemove, source);
	}

	//------------------------------------------------------//


	//-----------------------------------------//
	// Buffs/Debuffs that are removed manually //
	//-----------------------------------------//

	public void addBuffDebuff(BuffsDebuffs newBD, GameObject source) {
		// 1 - Does nothing if it exists already, 2 - Buffs/Debuffs that can stack together
		switch (newBD.bdType) {
			case 0: // If new buff is better, overwrite old buff/debuff
				addOverride(newBD, source);
				break;
			case 1: // If buff/debuff exists already, do nothing, else apply it on
				addSingular(newBD, source);
				break;
			case 2: // If there is already an instance of this buff/debuff, add another to the list
				addStacking(newBD, source);
				break;
			default:
				Debug.LogWarning("Buff/Debuff has no appropriate type");
				break;
		}
	}

	// For adding buffs/debuffs that override existing ones
	private void addOverride(BuffsDebuffs newBD, GameObject source) {
		BD newBuffDebuff = new BD(newBD);
		List<BD> list;
		
		if (buffsAndDebuffs.TryGetValue (newBD.name, out list)) {
			if ((list[0].bd as Override).isBetter(newBD, 0.0f, 0.0f)) { // Check if new (de)buff will be better than old one
				list.RemoveAt(0);
				
				list.Add(newBuffDebuff);
				setBD(newBuffDebuff, source);
			}
		} else {
			list = new List<BD>();
			list.Add(newBuffDebuff);
			buffsAndDebuffs[newBD.name] = list;
			
			setBD(newBuffDebuff, source);
		}
	}
	
	// For adding buffs/debuffs that don't do anything if unit is already affected by an instance of it
	private void addSingular(BuffsDebuffs newBD, GameObject source) {
		if (!buffsAndDebuffs.ContainsKey(newBD.name)) {
			BD newBuffDebuff = new BD(newBD);
			List<BD> list = new List<BD>();
			list.Add(newBuffDebuff);
			buffsAndDebuffs[newBD.name] = list;
			
			setBD(newBuffDebuff, source);
		}
	}
	
	// For adding buffs/debuffs that can stack
	private void addStacking(BuffsDebuffs newBD, GameObject source) {
		BD newBuffDebuff = new BD(newBD);
		List<BD> list;
		
		if (buffsAndDebuffs.TryGetValue (newBD.name, out list)) {
			list.Add (newBuffDebuff);
		} else {
			list = new List<BD>();
			list.Add(newBuffDebuff);
			buffsAndDebuffs[newBD.name] = list;
		}
		setBD(newBuffDebuff, source);
	}

	private void setBD(BD newBD, GameObject source) {
		newBD.bd.applyBD(affectedUnit, source);
		newBD.maxDuration = 0;
		newBD.timeStart = Time.time;
	}

	//----------------------------------------------------------//

	// For general removal
	public void rmvBuffDebuff(BuffsDebuffs bdToRemove, GameObject source) {
		List<BD> list;

		if (buffsAndDebuffs.TryGetValue (bdToRemove.name, out list)) {
			for (int i = 0; i < list.Count; i++) {
				if (list[i].bd == bdToRemove) {
					list[i].bd.removeBD(affectedUnit, source);
					list.RemoveAt(i);
					break;
				}

				if (i == list.Count - 1) {
					Debug.LogWarning("Specific Buff/Debuff not found.");
				}
			}

			if (list.Count == 0) {
				buffsAndDebuffs.Remove(bdToRemove.name);
			}
		} else {
			// Debug.LogWarning("No such buff/debuff with name " + bdToRemove.name + " exists.");
		}
	}

	// Purge removes the buff/debuff prematurely (Removes current coroutines affecting it)
	public void purgeBuffDebuff(BuffsDebuffs bdToPurge, GameObject source) {
		List<BD> list;

		if (buffsAndDebuffs.TryGetValue (bdToPurge.name, out list)) {
			for (int i = 0; i < list.Count; i++) {
				if (list[i].bd == bdToPurge) {
					list[i].bd.purgeBD(affectedUnit, source);
					list.RemoveAt(i);
					break;
				}
				
				if (i == list.Count - 1) {
					Debug.LogWarning("Specific Buff/Debuff not found.");
				}
			}
		} else {
			Debug.LogWarning("No such buff/debuff with name " + bdToPurge.name + " exists.");
		}
	}

	// Purges all buffs/Debuffs on this unit
	public void purgeAll(GameObject source) {
		List<BuffsDebuffs> list;

		foreach(KeyValuePair<string, List<BD>> entry in buffsAndDebuffs) {
			for (int i = 0; i < entry.Value.Count; i++) {
				entry.Value[i].bd.purgeBD(affectedUnit, source);
			}
		}
	}
}