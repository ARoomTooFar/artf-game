using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealthRingController : MonoBehaviour {
	public Player player;
	public Image ring1;
	public Image ring2;
	
	void Start () {
		player = this.transform.parent.gameObject.GetComponent<Player>();
		ring1 = transform.Find("Ring1").GetComponent<Image>();
		ring2 = transform.Find("Ring2").GetComponent<Image>();
	}
	
	//LateUpdate prevents UI flickering
	void LateUpdate(){
		rotateCanvasToCamera();
	}
	
	void Update () {
		updateHP();
	}
	
	void rotateCanvasToCamera(){
		Vector3 targetRot = Camera.main.transform.rotation.eulerAngles;
		targetRot.x = 90f;
		targetRot.z = 0f;
		this.GetComponent<Canvas>().transform.rotation = Quaternion.Euler(targetRot);
	}
	
	void updateHP(){
		float ringFill = normalizeForFilling(player.stats.health) / 2f;
		
		ring1.fillAmount = ringFill;
		ring2.fillAmount = ringFill;
		
		//		ring1.color = new Color(ring1.fillAmount * 2f, ring1.fillAmount * 2f, 0f);
		//		ring2.color = new Color(ring1.fillAmount * 2f, ring1.fillAmount * 2f, 0f);
	}
	
	float normalizeForFilling(int val){
		return ((float)val /  (float)player.stats.maxHealth);
	}
	
	float normalizeForFilling(float val){
		return (val /  (float)player.stats.maxHealth);
	}
}
