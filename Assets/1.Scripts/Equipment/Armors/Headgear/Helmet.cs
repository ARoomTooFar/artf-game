using UnityEngine;
using System.Collections;

public class Helmet : MonoBehaviour {
	public SkinnedMeshRenderer keyHelmet;
	
	public void Equip(Character u, int tier) {
		SkinnedMeshRenderer smr = this.GetComponent<SkinnedMeshRenderer>();
		
		if (this.keyHelmet == null) return;
		smr.sharedMesh = this.keyHelmet.sharedMesh;
		smr.materials = this.keyHelmet.sharedMaterials;
		
		Armor helm = this.gameObject.AddComponent(keyHelmet.GetComponent<Armor>().GetType()) as Armor;
		
		if (helm == null) {
			Debug.LogError(keyHelmet.name + " has no armor component assigned");
			return;
		}
		
		helm.Equip(u, tier);
	}
}
