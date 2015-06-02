using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GearSelectCtrl : MonoBehaviour {
	Image weaponIF;

	// Use this for initialization
	void Start () {
		Sprite newSprite = Resources.Load<Sprite>("ItemFrames/HuntersRifle");
		Debug.Log (newSprite);
		weaponIF = GameObject.Find ("WeaponIF").GetComponent<Image> ();
		Debug.Log (weaponIF);
		Debug.Log (weaponIF.sprite);
		weaponIF.sprite = newSprite;
		Debug.Log (weaponIF.sprite);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
