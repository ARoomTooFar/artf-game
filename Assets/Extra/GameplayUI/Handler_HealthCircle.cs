using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//This class is for the GamePlay UI.
//It creates a circular healthbar around the object.
//Currently it is not attached to any value, and is changed via a scrollbar
[RequireComponent(typeof(Scrollbar))]
public class Handler_HealthCircle : MonoBehaviour
{
//	[SerializeField]
//	Image CircleImage;
//	[SerializeField]
//	Color start;
//	[SerializeField]
//	Color end;
//	
//	[SerializeField]
//	Color current;
//	
//	Scrollbar scrollbar { get { return GetComponent<Scrollbar>(); } }
//	
//	void Start()
//	{
//		CircleImage.type = Image.Type.Filled;
//		CircleImage.fillMethod = Image.FillMethod.Radial360;
//		CircleImage.fillOrigin = 0;
//	}
//	
//	void Update()
//	{
//		CircleImage.fillAmount = Mathf.Max( scrollbar.value,0.001f);
//		CircleImage.color = Color.Lerp(start, end, scrollbar.value);
//		current = Color.Lerp(start, end, scrollbar.value);
//	}
	
	
}
