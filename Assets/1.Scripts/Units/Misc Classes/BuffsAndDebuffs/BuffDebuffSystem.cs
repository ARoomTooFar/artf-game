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

	//---------------------------------------------------------//
	// Functions that add and apply timed buffs/debuff go here //
	//---------------------------------------------------------//
	
	public void addBuffDebuff(ref BuffsDebuffs newBD, float duration) {
		// 0 - Overwrites Existing, 1 - Does nothing if it exists already, 2 - Buffs/Debuffs that can stack together
		switch (newBD.bdType) {
			case 0: // If an instance of this buff/debuff exists, overwrite it with new instance, else just apply it
				addOverride(ref newBD, duration);
				break;
			case 1: // If buff/debuff exists already, do nothing, else apply it on
				addSingular(ref newBD, duration);
				break;
			case 2: // If there is already an instance of this buff/debuff, add another to the list
				addStacking(ref newBD, duration);
				break;
			default:
				Debug.LogWarning("Buff/Debuff has no appropriate type");
				break;
		}
	}
	
	// For adding buffs/debuffs that override existing ones
	private void addOverride(ref BuffsDebuffs newBD, float duration) {
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
	private void addSingular(ref BuffsDebuffs newBD, float duration) {
		List<BuffsDebuffs> list;
		
		if (!buffsAndDebuffs.TryGetValue (newBD.name, out list)) {
			list = new List<BuffsDebuffs>();
			list.Add(newBD);
			buffsAndDebuffs[newBD.name] = list;
			newBD.applyBD(affectedUnit);
		}
	}
	
	// For adding buffs/debuffs that can stack
	private void addStacking(ref BuffsDebuffs newBD, float duration) {
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


	//----------------------------------------------------------//
	// Buffs/Debuffs that are removed by something else go here //
	//----------------------------------------------------------//

	public void addBuffDebuff(ref BuffsDebuffs newBD) {
		// 0 - Overwrites Existing, 1 - Does nothing if it exists already, 2 - Buffs/Debuffs that can stack together
		switch (newBD.bdType) {
		case 0: // If an instance of this buff/debuff exists, overwrite it with new instance, else just apply it
			addOverride(ref newBD);
			break;
		case 1: // If buff/debuff exists already, do nothing, else apply it on
			addSingular(ref newBD);
			break;
		case 2: // If there is already an instance of this buff/debuff, add another to the list
			addStacking(ref newBD);
			break;
		default:
			Debug.LogWarning("Buff/Debuff has no appropriate type");
			break;
		}
	}

	// For adding buffs/debuffs that override existing ones
	private void addOverride(ref BuffsDebuffs newBD) {
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
	private void addSingular(ref BuffsDebuffs newBD) {
		List<BuffsDebuffs> list;
		
		if (!buffsAndDebuffs.TryGetValue (newBD.name, out list)) {
			list = new List<BuffsDebuffs>();
			list.Add(newBD);
			buffsAndDebuffs[newBD.name] = list;
			newBD.applyBD(affectedUnit);
		}
	}
	
	// For adding buffs/debuffs that can stack
	private void addStacking(ref BuffsDebuffs newBD) {
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

	public void rmvBuffDebuff(ref BuffsDebuffs bdToRemove) {

	}

	//----------------------------------------------------------//
}