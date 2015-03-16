using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraDemo : MonoBehaviour {
	private GSManager gsManager;
	private Farts serv;
	private GameObject btnLevel1;
	private GameObject btnLevel2;
	private GameObject btnLevel3;
	private GameObject btnLevel4;
	private GameObject btnLevel5;
	private GameObject btnLevel6;
	private GameObject btnLevel7;
	private GameObject btnLevel8;
	public bool moveTrigger = false;

	void Start () {
		gsManager = GameObject.Find("GSManager").GetComponent<GSManager>();
		serv = gameObject.AddComponent<Farts>();

		btnLevel1 = GameObject.Find("BtnLevel1");
		btnLevel2 = GameObject.Find("BtnLevel2");
		btnLevel3 = GameObject.Find("BtnLevel3");
		btnLevel4 = GameObject.Find("BtnLevel4");
		btnLevel5 = GameObject.Find("BtnLevel5");
		btnLevel6 = GameObject.Find("BtnLevel6");
		btnLevel7 = GameObject.Find("BtnLevel7");
		btnLevel8 = GameObject.Find("BtnLevel8");

		btnLevel1.SetActive(false);
		btnLevel2.SetActive(false);
		btnLevel3.SetActive(false);
		btnLevel4.SetActive(false);
		btnLevel5.SetActive(false);
		btnLevel6.SetActive(false);
		btnLevel7.SetActive(false);
		btnLevel8.SetActive(false);

		btnLevel1.GetComponent<Button>().onClick.AddListener(() =>
			{
				Debug.Log ("BtnLevel1 clicked");
				//StartDl("5715999101812736");
                gsManager.LoadScene(1);
			}
		);

		btnLevel2.GetComponent<Button>().onClick.AddListener(() =>
			{
				Debug.Log ("BtnLevel2 clicked");
			}
		);

		btnLevel3.GetComponent<Button>().onClick.AddListener(() =>
			{
				Debug.Log ("BtnLevel3 clicked");
			}
		);

		btnLevel4.GetComponent<Button>().onClick.AddListener(() =>
			{
				Debug.Log ("BtnLevel4 clicked");
			}
		);

		btnLevel5.GetComponent<Button>().onClick.AddListener(() =>
			{
				Debug.Log ("BtnLevel5 clicked");
			}
		);

		btnLevel6.GetComponent<Button>().onClick.AddListener(() =>
			{
				Debug.Log ("BtnLevel6 clicked");
			}
		);

		btnLevel7.GetComponent<Button>().onClick.AddListener(() =>
			{
				Debug.Log ("BtnLevel7 clicked");
				Debug.Log ("Move to new scene!");
				gsManager.LoadScene(1);
			}
		);

		btnLevel8.GetComponent<Button>().onClick.AddListener(() =>
			{
				Debug.Log ("BtnLevel8 clicked");
				Debug.Log ("Print level 1 data!");
				Debug.Log (gsManager.level1Data);
			}
		);
	}
	
	void Update () {
		if (moveTrigger) {
			if (transform.position.z <= -116) {
				transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 0.2f);
			} else if (transform.position.z > -117) {
				btnLevel1.SetActive(true);
				btnLevel2.SetActive(true);
				btnLevel3.SetActive(true);
				btnLevel4.SetActive(true);
				btnLevel5.SetActive(true);
				btnLevel6.SetActive(true);
				btnLevel7.SetActive(true);
				btnLevel8.SetActive(true);
			}
		}
	}

	public void StartMove () {
		moveTrigger = true;
	}

	public void StartDl (string levelId) {
		WWW www = serv.getLvlWww(levelId);
		StartCoroutine(dlLvl(www));
	}

	// should have 2nd parameter be a callback function
	public IEnumerator dlLvl(WWW www)
	{
		yield return www;
		
		gsManager.level1Data = www.text;

		if (serv.dataCheck(gsManager.level1Data))
		{
			Debug.Log("LVL DL SUCCESS: " + gsManager.level1Data);
		}
		else
		{
			Debug.Log("ERROR: Level download failed. Level ID doesn't exist.");
		}
	}
}
