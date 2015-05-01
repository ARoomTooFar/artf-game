using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ContainerScript : MonoBehaviour {
    private Image imgPanel;
    private CanvasGroup groupStartMenu;

	// Use this for initialization
	void Start () {
        //gameObject.GetComponent<Renderer>().material.color = Color.white;
        /*Debug.Log(gameObject.GetComponent<CanvasRenderer>().GetColor());
        gameObject.GetComponent<CanvasRenderer>().SetColor(Color.red);
        Debug.Log("yeah");
        Debug.Log(gameObject.GetComponent<CanvasRenderer>().GetColor());*/
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in children) {
            if (child.name == "Panel") {
                imgPanel = child.GetComponent<Image>();
            } else if (child.name == "StartMenu") {
                groupStartMenu = child.GetComponent<CanvasGroup>();
            }
        }

        imgPanel.color = new Color(0.3f, 0.3f, 0.3f);
        groupStartMenu.interactable = false;
	}

    void SetActive() {
        Debug.Log("SetActive called");
        gameObject.GetComponentsInChildren<Transform>();
    }

    void SetInactive () {
        Debug.Log("SetInactive called");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
