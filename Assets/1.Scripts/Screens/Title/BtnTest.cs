using UnityEngine;
using System.Collections;

public class BtnTest : MonoBehaviour {
    private GSManager gsManager;

	public void TestDaBtn () {
        gsManager = GameObject.Find("GSManager").GetComponent<GSManager>();
        Debug.Log(gsManager.health);
	}
}
