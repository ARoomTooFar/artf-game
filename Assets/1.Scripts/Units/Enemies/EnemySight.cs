using UnityEngine;
using System.Collections;

public class EnemySight : Enemy{

	//Public variables to tweak in inspector
	public float fov = 110f;
	public bool playerInSight;
	public Vector3 targetPosition;

	//Private variables for use in player detection
	private NavMeshAgent nav;
	private Animator ani;
	private SphereCollider col;
	private GameObject[] players;
	private GameObject target;

	//Get players, navmesh and all colliders
	void Awake (){
		nav = GetComponent<NavMeshAgent> ();
		col = GetComponent<SphereCollider> ();
		ani = GetComponent<Animator> ();
		players = GameObject.FindGameObjectsWithTag ("Player");
	}

	void OnTriggerStay (Collider other)
	{
		for (int i = 0; i <=3; i++) 
		{
			if (other.gameObject == players[i]) 
			{
				playerInSight = false;
			
				// Check angle of forward direction vector against the vector of enemy position relative to player position
				Vector3 direction = other.transform.position - transform.position;
				float angle = Vector3.Angle (direction, transform.forward);
			
				if (angle < fov * 0.5f) 
				{
					RaycastHit hit;
					if (Physics.Raycast (transform.position + transform.up, direction.normalized, out hit, col.radius)) {

						if (hit.collider.gameObject == players[i]) 
						{

							playerInSight = true;
							//print ("Enemy can see me");
							targetPosition = other.transform.position;
							attackPlayer(players[i]);
														
						}
					}
				}
			
			}
		}
	}

	void attackPlayer (GameObject player)
	{
		Vector3 distanceToTarget = transform.position - targetPosition;
		if (distanceToTarget.sqrMagnitude > 6) 
		{
			nav.destination = targetPosition;
			ani.SetBool ("Moving", true);
		} else {
			nav.Stop ();
			ani.SetBool ("Moving", false);
		}
	}
}
