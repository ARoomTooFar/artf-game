using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public NavMeshObstacle obs;

	void Awake () {
		obs = GetComponent<NavMeshObstacle> ();
		obs.carving = true;
//		obs.height = transform.lossyScale.y;
//		obs.radius = transform.lossyScale.z;
		Debug.Log (obs.height + " " + obs.radius);
	}
}
