using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class preflist : MonoBehaviour {

	public Hashtable table = new Hashtable();
//	public List<GameObject> GOlist = new List<GameObject>();
	public GameObject insert;

	void Awake () {
		insert = null;
	}

	void Update () {
		if (insert != null) {
			table [insert.name] = (GameObject) insert;
			Debug.Log(table.Keys);
			insert = null;
		} else {
			Debug.Log(table.Count);
		}
	}
}
