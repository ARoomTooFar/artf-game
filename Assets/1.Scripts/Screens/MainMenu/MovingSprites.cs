using UnityEngine;
using System.Collections;

public class MovingSprites : MonoBehaviour {

	public bool reverse;
	public float speed;
	public Vector3 startingPosition, endingPosition;
	
	private float startTime;
	private float journeyLength;

	// Use this for initialization
	void Start () {
		if (this.startingPosition == null) Debug.LogWarning ("No starting position given to object " + this.name);
		else if (this.endingPosition == null) Debug.LogWarning ("No ending position given to object " + this.name);
		else if (this.startingPosition == this.endingPosition) Debug.LogWarning ("Starting and ending position are the same on object " + this.name);
		else if (this.speed == 0f) Debug.LogWarning ("No speed value given to object " + this.name);
		else {
			this.journeyLength = Vector3.Distance(this.startingPosition, this.endingPosition);
			this.StartCoroutine (MoveToEnd ());
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private IEnumerator MoveToEnd() {
		this.transform.position = this.startingPosition;
		this.startTime = Time.time;
		while (this.transform.position != this.endingPosition) {
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			this.transform.position = Vector3.Lerp(startingPosition, endingPosition, fracJourney);
			yield return null;
		}
		if (reverse) this.StartCoroutine(MoveToStart ());
		else this.StartCoroutine(MoveToEnd ());
	}
	
	private IEnumerator MoveToStart() {
		this.transform.position = this.endingPosition;
		this.startTime = Time.time;
		while (this.transform.position != this.startingPosition) {
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			this.transform.position = Vector3.Lerp(endingPosition, startingPosition, fracJourney);
			yield return null;
		}
		this.StartCoroutine(MoveToEnd ());
	}
}
