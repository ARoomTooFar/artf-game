using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DemoHand : MonoBehaviour {
	private GSManager gsManager;
	private Button btnInsertCoin;
	private GameObject loadingBG;
	
	void Start ()
	{
		gsManager = GameObject.Find("GSManager").GetComponent<GSManager>();
		btnInsertCoin = GameObject.Find("BtnInsertCoin").GetComponent<Button>();
		
		// hide LoadingBG. can't hide in inspector because Find() can't find hidden objects.
		loadingBG = GameObject.Find("LoadingBG");
		loadingBG.SetActive(false);
		
		// add functions to be called when buttons are clicked
		btnInsertCoin.onClick.AddListener(() =>
			{
				ZoomToLevelSelect ();
			}
		);
	}

	void ZoomToLevelSelect ()
	{
		Debug.Log ("hyeeeaaachaaaa");
	}
}
