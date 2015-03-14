using UnityEngine;
using System.Collections;

public class GSManager : MonoBehaviour {
    // persistent data
    public static GSManager gsManager;
    public float health;
    public float experience;

	void Awake () {
        // ensure gsManager is a singleton
        if (gsManager == null)
        {
            DontDestroyOnLoad(gameObject);  // don't destroy itself between scene loads
            gsManager = this;
        }
        else if (gsManager != this)
        {
            Destroy(gameObject);
        }
	}
	
    public void LoadScene (int scene)
    {
        Application.LoadLevel(scene);
    }
}
