using UnityEngine;
using System.Collections;

public class Mutation : MonoBehaviour {
	private bool onGround;
	private GameObject droneToMutate;
	// Use this for initialization
	void Start () {
		onGround = false;
		GameObject[] drones = GameObject.FindGameObjectsWithTag ("drone");
		int rand = Random.Range (0, drones.Length);
		droneToMutate = drones [rand];
		InvokeRepeating ("destroyRoutine",7,2);
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (transform.position.y < 0.3) {
			if (!onGround) {
				droneToMutate.GetComponent<DroneBehavior> ().targetPosition = transform.position;
				droneToMutate.GetComponent<DroneBehavior> ().chaseTarget = true;
				onGround = true;
			}
		}
		if (transform.position.y < -0.2) {
			Destroy (gameObject);
			droneToMutate.GetComponent<DroneBehavior> ().chaseTarget = false;
		}
	}

	void destroyRoutine() {
		Destroy (gameObject);
		droneToMutate.GetComponent<DroneBehavior> ().chaseTarget = false;
	}

	void OnCollisionEnter (Collision col)
	{
		Debug.Log ("Name of col: " + col.gameObject.name);
		if(col.gameObject.name == "Drone_Prefab(Clone)")
		{
			droneToMutate.GetComponent<DroneBehavior> ().chaseTarget = false;
			col.gameObject.GetComponent<DroneBehavior> ().chaseTarget = false;
			col.gameObject.GetComponent<DroneBehavior> ().rendererColor = GetComponent<Renderer> ().material.color;
			Destroy (gameObject);
		}
	}
}
