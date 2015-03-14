using UnityEngine;
using System.Collections;

public class GSManager : MonoBehaviour {
    public float health;
    public float experience;
    public int currScene;

	void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
    public void LoadScene (int scene)
    {
        Application.LoadLevel(scene);
    }
}
