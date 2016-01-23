using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour {

	public GameObject mutationPrefab;

	private GameObject ground;

	private int face;
	private Color[] colors =
		new Color[] {Color.magenta, Color.blue, Color.green, Color.red, Color.yellow, Color.cyan};

	private float noMovementThreshold = 0.02f;
	private bool isMoving;
	private bool wasMoving;

	public int dronesToFeed = 5;


	private Vector3 previousLocation;
	public Vector3 startingLocation;
	public int movingFramesLimit = 2;
	private int frameNumberWithoutMoving;

	void Start() {
		ground = GameObject.FindGameObjectWithTag ("ground");
		wasMoving = true;
		isMoving = true;
		previousLocation = transform.position;
		startingLocation = transform.position;
		frameNumberWithoutMoving = 0;
		InvokeRepeating ("droneRoutine",0.25f,0.1f);
	}

	void FixedUpdate () {
		Debug.Log ("Face: " + face);
		Debug.Log ("isMoving: " + isMoving);
	}

	void droneRoutine() {
		face = DiceFace();
		calculateMovement ();
		if (isMoving) {
			GetComponent<Renderer>().material.color = Color.white;
		} else {
			GetComponent<Renderer>().material.color = colors[face-1];
		}
	}

	int DiceFace() {
		var upDir = Vector3.up;
		var x = Vector3.Dot(transform.right, upDir);
		var y = Vector3.Dot(transform.up, upDir);
		var z = Vector3.Dot(transform.forward, upDir);
		var xAbs = Mathf.Abs(x);
		var yAbs = Mathf.Abs(y);
		var zAbs = Mathf.Abs(z);

		if (xAbs > yAbs) {
			if (xAbs > zAbs) {
				return x > 0 ? 1 : 6;
			} else {
				return z > 0 ? 2 : 5;
			}
		} else if (yAbs > zAbs) {
			return y > 0 ? 3 : 4;
		} else {
			return z > 0 ? 2 : 5;
		}
	}

	private void updateDrones() {
		// Change the target and color ONLY if the state triggered from moving to not moving
		if (wasMoving && !isMoving && GetComponent<Renderer> ().enabled) {
			for (int i = 0; i < dronesToFeed; i++) {
				var mutationPosition = transform.position;
				mutationPosition.y = transform.position.y + 0.5f;
				var mutationRotation = transform.rotation;
				mutationRotation.y = transform.rotation.y + i * 80;
				GameObject tempMutation = (GameObject) Instantiate(mutationPrefab);
				tempMutation.transform.position = mutationPosition;
				tempMutation.transform.rotation = mutationRotation;
				tempMutation.GetComponent<Renderer> ().material.color = colors[face - 1];
				tempMutation.GetComponent<Rigidbody> ().velocity = new Vector3 (Random.Range (-4, 4),1,Random.Range (-4, 4));
				tempMutation.transform.parent = ground.transform.parent;
				tempMutation.GetComponent<Mutation> ().mutationIndex = face-1;
				tempMutation.GetComponent<Mutation> ().mutationColors = colors;
				Debug.Log ("mutation created, face: " + face);
				/*
				rand = Random.Range (0, drones.Length);
				drones [rand].GetComponent<DroneBehavior> ().targetPosition = startingLocation;
				drones [rand].GetComponent<DroneBehavior> ().chaseTarget = true;
				drones [rand].GetComponent<DroneBehavior> ().rendererColor = colors [face - 1];
				*/
			}
		} /* else {
			foreach(GameObject drone in drones) {
				drone.GetComponent<DroneBehavior> ().chaseTarget = false;
			}
		}
		*/
	}

	public void mutate() {
		
	}

	private void calculateMovement() {
		if (Vector3.Distance (previousLocation, transform.position) <= noMovementThreshold) {
			frameNumberWithoutMoving++;
		} else {
			frameNumberWithoutMoving = 0;
			isMoving = true;
		}
		if ((frameNumberWithoutMoving == movingFramesLimit) && GetComponent<Renderer> ().enabled) {
			frameNumberWithoutMoving = 0;
			wasMoving = isMoving;
			isMoving = false;
			updateDrones ();
			startingLocation = previousLocation;
		}
		previousLocation = transform.position;
	}

	//Let other scripts see if the object is moving
	public bool IsMoving
	{
		get{ return isMoving; }
	}

	public Vector3 getStartingLocation {
		get{ return startingLocation; }
	}
}
