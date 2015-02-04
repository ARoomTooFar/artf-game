using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//This class controls the circle HP ring
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
		//This value is the amount that the ring is filled in.
		//It goes between 0 and 1.
		CircleImage.fillAmount = 0.6f;

        CircleImage.color = Color.Lerp(start, end, scrollbar.value);
        current = Color.Lerp(start, end, scrollbar.value);
    }

     
}
