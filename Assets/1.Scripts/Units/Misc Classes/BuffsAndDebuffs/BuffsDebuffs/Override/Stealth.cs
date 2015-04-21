using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Stealth : Singular {
		public Stealth() {
			name = "Stealth";
		}
		protected override void bdEffects(BDData newData) {
			base.bdEffects(newData);
			newData.unit.invis = true;
			foreach(Cloak skin in newData.unit.skins){
				skin.cloakSkin();
			}
		}
		
		protected override void removeEffects (BDData oldData, GameObject source) {
			base.removeEffects (oldData, source);
			oldData.unit.invis = false;
			foreach(Cloak skin in oldData.unit.skins){
				skin.cloakSkin();
			}
		}
		
		public override void purgeBD(Character unit, GameObject source) {
			base.purgeBD (unit, source);
		}
	}
