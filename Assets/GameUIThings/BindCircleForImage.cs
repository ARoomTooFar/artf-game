using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[RequireComponent(typeof(Scrollbar))]
public class BindCircleForImage : MonoBehaviour
{
    [SerializeField]
    Image CircleImage;
    [SerializeField]
    Color start;
    [SerializeField]
    Color end;

    [SerializeField]
    Color current;

    Scrollbar scrollbar { get { return GetComponent<Scrollbar>(); } }

    void Start()
    {
        CircleImage.type = Image.Type.Filled;
        CircleImage.fillMethod = Image.FillMethod.Radial360;
        CircleImage.fillOrigin = 0;
    }

    void Update()
    {
        CircleImage.fillAmount = Mathf.Max( scrollbar.value,0.001f);
        CircleImage.color = Color.Lerp(start, end, scrollbar.value);
        current = Color.Lerp(start, end, scrollbar.value);
    }

     
}
