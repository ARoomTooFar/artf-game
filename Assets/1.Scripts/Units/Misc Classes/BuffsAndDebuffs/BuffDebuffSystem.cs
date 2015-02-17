using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffDebuffSystem {

	private Character affectedUnit;
	
	private Dictionary<string, List<BuffsDebuffs>> buffsAndDebuffs;

	protected delegate void timeDelegate();

	public BuffDebuffSystem(Character unit) {
		affectedUnit = unit;
		buffsAndDebuffs = new Dictionary<string, List<BuffsDebuffs>>();
	}

	//------------------------------------------------------//
	// Functions that add and applying buffs/debuff go here //
	//------------------------------------------------------//
	
	public void addBuffDebuffTimed(ref BuffsDebuffs newBD) {
		// 0 - Overwrites Existing, 1 - Does nothing if it exists already, 2 - Buffs/Debuffs that can stack together
		switch (newBD.bdType) {
			case 0: // If an instance of this buff/debuff exists, overwrite it with new instance, else just apply it
				addTimedOverride(ref newBD);
				break;
			case 1: // If buff/debuff exists already, do nothing, else apply it on
				addTimedSingular(ref newBD);
				break;
			case 2: // If there is already an instance of this buff/debuff, add another to the list
				addTimedStacking(ref newBD);
				break;
			default:
				Debug.LogWarning("Buff/Debuff has no appropriate type");
				break;
		}
	}
	
	// For adding buffs/debuffs that override existing ones
	private void addTimedOverride(ref BuffsDebuffs newBD) {
		List<BuffsDebuffs> list;
	
		if (buffsAndDebuffs.TryGetValue (newBD.name, out list)) {
			list[0].removeBD();
			list.RemoveAt(0);
			list.Add(newBD);
		} else {
			list = new List<BuffsDebuffs>();
			list.Add(newBD);
			buffsAndDebuffs[newBD.name] = list;
		}
		newBD.applyBD(affectedUnit);
	}

	// For adding buffs/debuffs that don't do anything if unit is already affected by an instance of it
	private void addTimedSingular(ref BuffsDebuffs newBD) {
		List<BuffsDebuffs> list;
		
		if (!buffsAndDebuffs.TryGetValue (newBD.name, out list)) {
			list = new List<BuffsDebuffs>();
			list.Add(newBD);
			buffsAndDebuffs[newBD.name] = list;
			newBD.applyBD(affectedUnit);
		}
	}
	
	// For adding buffs/debuffs that can stack
	private void addTimedStacking(ref BuffsDebuffs newBD) {
		List<BuffsDebuffs> list;
		
		if (buffsAndDebuffs.TryGetValue (newBD.name, out list)) {
			list.Add (newBD);
		} else {
			list = new List<BuffsDebuffs>();
			list.Add(newBD);
			buffsAndDebuffs[newBD.name] = list;
		}
		newBD.applyBD(affectedUnit);
	}

	//------------------------------------------------------//

	public void rmvBuffDebuff(BuffDebuffSystem bdToRemove) {

	}
}