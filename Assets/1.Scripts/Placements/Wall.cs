using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {

	public NavMeshObstacle obs;

	void Awake () {
		obs = GetComponent<NavMeshObstacle> ();
		obs.carving = true;
	}
}
