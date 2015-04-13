using UnityEngine;
using System.Collections;

public class InventoryHand : MonoBehaviour {
    private GameObject loadingBG;

	// Use this for initialization
	void Start () {
        loadingBG = GameObject.Find("LoadingBG");
        loadingBG.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
