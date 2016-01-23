using UnityEngine;
using System.Collections;

public class Mutation : MonoBehaviour {
	public int mutationIndex;
	public Color[] mutationColors;
	private Color mutationColor;
	private bool onGround;
	private GameObject droneToMutate;
	private Vector3 groundPos;
	// Use this for initialization
	void Start () {
		onGround = false;
		GameObject[] drones = GameObject.FindGameObjectsWithTag ("drone");
		int rand = Random.Range (0, drones.Length);
		droneToMutate = drones [rand];
		InvokeRepeating ("destroyRoutine",7,2);
		groundPos = GameObject.FindGameObjectWithTag ("ground").gameObject.transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (transform.position.y < 0.08) {
			if (!onGround) {
				droneToMutate.GetComponent<DroneBehavior> ().targetPosition = transform.position;
				droneToMutate.GetComponent<DroneBehavior> ().chaseTarget = true;
				onGround = true;
			}
		}
		if (transform.position.y < -0.4) {
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
			float scaleFactor = 1f;
			float heightFactor = 0.04f;
			Vector3 tempPos = col.gameObject.transform.position;
			droneToMutate.GetComponent<DroneBehavior> ().chaseTarget = false;
			col.gameObject.GetComponent<DroneBehavior> ().chaseTarget = false;
			col.gameObject.GetComponent<Renderer> ().material.color = mutationColors [mutationIndex];

			switch (mutationIndex)
			{
			case 0:
				scaleFactor = 1.2f;
				Debug.Log("mutation 1");
				break;
			case 1: 
				scaleFactor = 0.8f;
				Debug.Log("mutation 2");
				break;
			case 2:
				scaleFactor = 0.5f;
				Debug.Log("mutation 3");
				break;
			case 3:
				scaleFactor = 1.1f;
				Debug.Log("mutation 4");
				break;
			case 4:
				scaleFactor = 0.9f;
				Debug.Log("mutation 5");
				break;
			case 5:
				scaleFactor = 1.4f;
				Debug.Log("mutation 6");
				break;
			default:
				Debug.Log ("none of the above");
				break;
			}
			Vector3 newScale = col.gameObject.transform.localScale * scaleFactor;
			Debug.Log ("New scale y: " + newScale.y);
			if (newScale.y < 12f && newScale.y > 1f) {
				col.gameObject.transform.localScale = newScale;
				tempPos.y = groundPos.y + 0.5f*col.gameObject.GetComponent<Renderer>().bounds.size.y+heightFactor;
				col.gameObject.transform.position = tempPos;
			}
			Destroy (gameObject);
		}
	}
}
