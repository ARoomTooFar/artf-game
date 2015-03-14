using UnityEngine;
using System.Collections;

public class GSManager : MonoBehaviour {
    public float health;
    public float experience;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
