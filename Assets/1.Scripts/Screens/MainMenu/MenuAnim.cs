using UnityEngine;
using System.Collections;

public class MenuAnim : MonoBehaviour {
    private Animator menuAnim;

	// Use this for initialization
	void Awake () {
        menuAnim = GetComponent<Animator>();
	}

    public void BeginMenu()
    {
        menuAnim.SetTrigger("FadeIn");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
