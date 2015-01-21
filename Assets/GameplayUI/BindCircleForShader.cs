using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BindCircleForShader : MonoBehaviour 
{
    [SerializeField]
    Color start;
    [SerializeField]
    Color end;

    [SerializeField]
    Material CircleMaterial;

    Scrollbar scrollbar { get { return GetComponent<Scrollbar>(); } }

	// Update is called once per frame
	void Update () 
    {
        CircleMaterial.SetFloat("_Angle", Mathf.Lerp(-3.14f, 3.14f, scrollbar.value));
        CircleMaterial.SetColor("_Color", Color.Lerp(start, end, scrollbar.value));
	}
}
