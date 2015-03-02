using UnityEngine;
using System.Collections;

public class Immobilize : Singular {
		
		private float spdPercent;
		
		public Immobilize() {
			name = "Immobilize";
			spdPercent = 1f;
		}
		
		protected override void bdEffects(BDData newData) {
			base.bdEffects(newData);
			newData.unit.stats.spdManip.setSpeedReduction(spdPercent);
		}
		
		protected override void removeEffects (BDData oldData, GameObject source) {
			base.removeEffects (oldData, source);
			oldData.unit.stats.spdManip.removeSpeedReduction(spdPercent);
		}
		
		public override void purgeBD(Character unit, GameObject source) {
			base.purgeBD (unit, source);
		}
	}