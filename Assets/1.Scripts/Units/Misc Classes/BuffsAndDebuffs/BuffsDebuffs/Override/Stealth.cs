using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stealth : Singular {
		public Renderer[] spots;
		public Texture invisible;
		public Stealth() {
			name = "Stealth";
		}
		protected override void bdEffects(BDData newData) {
			base.bdEffects(newData);
			spots = newData.unit.GetComponentsInChildren<Renderer>();
			newData.unit.invis = true;
		}
		
		protected override void removeEffects (BDData oldData, GameObject source) {
			base.removeEffects (oldData, source);
			oldData.unit.invis = false;
		}
		
		public override void purgeBD(Character unit, GameObject source) {
			base.purgeBD (unit, source);
		}
	}
