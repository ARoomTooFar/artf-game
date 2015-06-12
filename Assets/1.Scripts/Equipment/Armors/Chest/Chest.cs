using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

	public SkinnedMeshRenderer keyArmor;
	
	public void Equip(Character u, int tier) {
		SkinnedMeshRenderer smr = this.GetComponent<SkinnedMeshRenderer>();
		
		if (this.keyArmor == null) return;
		smr.sharedMesh = this.keyArmor.sharedMesh;
		smr.materials = this.keyArmor.sharedMaterials;
		
		Armor armor = this.gameObject.AddComponent(keyArmor.GetComponent<Armor>().GetType()) as Armor;
		armor.hideHelm = keyArmor.GetComponent<Armor>().hideHelm;
	
		if (armor == null) {
			Debug.LogError(keyArmor.name + " has no armor component assigned");
			return;
		}
	
		armor.Equip(u, tier);
	}
}
