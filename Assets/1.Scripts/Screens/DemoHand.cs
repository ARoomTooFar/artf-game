using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DemoHand : MonoBehaviour {
	private GSManager gsManager;
	private Button btnInsertCoin;
	private GameObject loadingBG;
	private GameObject mainCamera;
	
	void Start ()
	{
		gsManager = GameObject.Find("GSManager").GetComponent<GSManager>();
		btnInsertCoin = GameObject.Find("BtnInsertCoin").GetComponent<Button>();
		mainCamera = GameObject.Find("CameraDemo");
		
		// hide LoadingBG. can't hide in inspector because Find() can't find hidden objects.
		loadingBG = GameObject.Find("LoadingBG");
		loadingBG.SetActive(false);

		// add functions to be called when buttons are clicked
		btnInsertCoin.onClick.AddListener(() =>
			{
				mainCamera.GetComponent<CameraDemo>().StartMove();
			}
		);
	}
}
