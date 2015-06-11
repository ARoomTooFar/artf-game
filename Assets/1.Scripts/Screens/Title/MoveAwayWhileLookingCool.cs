using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveAwayWhileLookingCool : MonoBehaviour {

	public GameObject cam;
	public Text text;
	public SpriteRenderer logo;
	
	private TxtPressButton script;
	private bool goingDown;
	private GSManager manager;

	// Use this for initialization
	void Start () {
		goingDown = false;
		this.script = text.gameObject.GetComponent<TxtPressButton>();
		this.manager = GameObject.Find("GSManager").GetComponent<GSManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown && !goingDown) {
			this.goingDown = true;
			this.StartCoroutine (GoDown());
		}
	}
	
	private IEnumerator GoDown() {
		yield return this.StartCoroutine(Fadeout ());
		yield return this.StartCoroutine(MoveDown ());
		manager.LoadScene("MainMenu");
	}
	
	private IEnumerator Fadeout() {
		this.script.enabled = false;
		Color txtColor = text.color;
		txtColor.a = 1.0f;
		while (txtColor.a > 0f) {
			txtColor.a -= 0.02f;
			logo.color = txtColor;
			text.color = txtColor;
			yield return null;
		}
	}
	
	private IEnumerator MoveDown() {
		Vector3 pos = cam.transform.position;
		Vector3 endJourney = new Vector3(pos.x, -30.5f, pos.z);
		float journeyLength = Vector3.Distance(pos, endJourney);
		float startTime = Time.time;
		while (cam.transform.position.y > -30.5f) {
			float distCovered = (Time.time - startTime) * 8;
			float fracJourney = distCovered / journeyLength;
			this.cam.transform.position = Vector3.Lerp(pos, endJourney, fracJourney);
			yield return null;
		}
	}
}
