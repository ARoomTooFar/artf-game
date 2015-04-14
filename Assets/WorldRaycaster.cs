using UnityEngine;
using System.Collections;

public class WorldRaycaster : UnityEngine.UI.GraphicRaycaster {
	
	[SerializeField]
	private int SortOrder = 0;
	
	public override int sortOrderPriority {
		get {
			return SortOrder;
		}
	}
}
