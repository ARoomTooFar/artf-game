// Buff and Debuff system that tracks on buff and debuffs on this unit
//     In the future, seperation of Buffs and Debuffs may be neccessary if we have hostile purges

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffDebuffSystem {
	private Character affectedUnit;
	
	private Dictionary<string, List<BuffsDebuffs>> buffsAndDebuffs;

	public BuffDebuffSystem(Character unit) {
		affectedUnit = unit;
		buffsAndDebuffs = new Dictionary<string, List<BuffsDebuffs>>();
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
		List<BuffsDebuffs> list;
		
		if (buffsAndDebuffs.TryGetValue (newBD.name, out list)) {
			if ((list[0] as Override).overwrite(affectedUnit, source, newBD)) {
				list.RemoveAt(0);
				list.Add(newBD);
				affectedUnit.GetComponent<MonoBehaviour>().StartCoroutine(timedRemoval(newBD, source, duration));
			}
		} else {
			list = new List<BuffsDebuffs>();
			list.Add(newBD);
			buffsAndDebuffs[newBD.name] = list;
			newBD.applyBD(affectedUnit, source);
			affectedUnit.GetComponent<MonoBehaviour>().StartCoroutine(timedRemoval(newBD, source, duration));
		}
	}

	// For adding buffs/debuffs that don't do anything if unit is already affected by an instance of it
	private void addSingular(BuffsDebuffs newBD, GameObject source, float duration) {
		List<BuffsDebuffs> list;

		if (!buffsAndDebuffs.TryGetValue (newBD.name, out list)) {
			list = new List<BuffsDebuffs>();
			list.Add(newBD);
			buffsAndDebuffs[newBD.name] = list;
			newBD.applyBD(affectedUnit, source);
			affectedUnit.GetComponent<MonoBehaviour>().StartCoroutine(timedRemoval(newBD, source, duration));
		}
	}
	
	// For adding buffs/debuffs that can stack
	private void addStacking(BuffsDebuffs newBD, GameObject source, float duration) {
		List<BuffsDebuffs> list;
		
		if (buffsAndDebuffs.TryGetValue (newBD.name, out list)) {
			list.Add (newBD);
		} else {
			list = new List<BuffsDebuffs>();
			list.Add(newBD);
			buffsAndDebuffs[newBD.name] = list;
		}
		newBD.applyBD(affectedUnit, source);
		affectedUnit.GetComponent<MonoBehaviour>().StartCoroutine(timedRemoval(newBD, source, duration));
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
		List<BuffsDebuffs> list;
		
		if (buffsAndDebuffs.TryGetValue (newBD.name, out list)) {
			if ((list[0] as Override).overwrite(affectedUnit, source, newBD)) {
				list.RemoveAt(0);
				list.Add(newBD);
			}
		} else {
			list = new List<BuffsDebuffs>();
			list.Add(newBD);
			buffsAndDebuffs[newBD.name] = list;
			newBD.applyBD(affectedUnit, source);
		}
	}
	
	// For adding buffs/debuffs that don't do anything if unit is already affected by an instance of it
	private void addSingular(BuffsDebuffs newBD, GameObject source) {
		List<BuffsDebuffs> list;
		
		if (!buffsAndDebuffs.TryGetValue (newBD.name, out list)) {
			list = new List<BuffsDebuffs>();
			list.Add(newBD);
			buffsAndDebuffs[newBD.name] = list;
			newBD.applyBD(affectedUnit, source);
		}
	}
	
	// For adding buffs/debuffs that can stack
	private void addStacking(BuffsDebuffs newBD, GameObject source) {
		List<BuffsDebuffs> list;
		
		if (buffsAndDebuffs.TryGetValue (newBD.name, out list)) {
			list.Add (newBD);
		} else {
			list = new List<BuffsDebuffs>();
			list.Add(newBD);
			buffsAndDebuffs[newBD.name] = list;
		}
		newBD.applyBD(affectedUnit, source);
	}

	//----------------------------------------------------------//

	// For general removal
	public void rmvBuffDebuff(BuffsDebuffs bdToRemove, GameObject source) {
		List<BuffsDebuffs> list;

		if (buffsAndDebuffs.TryGetValue (bdToRemove.name, out list)) {
			for (int i = 0; i < list.Count; i++) {
				if (list[i] == bdToRemove) {
					list[i].removeBD(affectedUnit, source);
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
			Debug.LogWarning("No such buff/debuff with name " + bdToRemove.name + " exists.");
		}
	}

	// Purge removes the buff/debuff prematurely (Removes current coroutines affecting it)
	public void purgeBuffDebuff(BuffsDebuffs bdToPurge, GameObject source) {
		List<BuffsDebuffs> list;

		if (buffsAndDebuffs.TryGetValue (bdToPurge.name, out list)) {
			for (int i = 0; i < list.Count; i++) {
				if (list[i] == bdToPurge) {
					list[i].purgeBD(affectedUnit, source);
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

		foreach(KeyValuePair<string, List<BuffsDebuffs>> entry in buffsAndDebuffs) {
			for (int i = 0; i < entry.Value.Count; i++) {
				entry.Value[i].purgeBD(affectedUnit, source);
			}
		}
	}
}