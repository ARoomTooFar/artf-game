using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleHand : MonoBehaviour {
    public GSManager gsManager;
    public Button btnStartGame;

	// Use this for initialization
	void Start ()
    {
        gsManager = GameObject.Find("GSManager").GetComponent<GSManager>();
        btnStartGame = GameObject.Find("BtnStartGame").GetComponent<Button>();

        btnStartGame.onClick.AddListener(() => {
            test();
        });
	}

    void test()
    {
        Debug.Log(gsManager.health);
        Debug.Log(gsManager.experience);
    }
}
