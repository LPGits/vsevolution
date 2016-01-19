using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour {


	private int face;
	private Color[] colors =
		new Color[] {Color.black, Color.blue, Color.green, Color.red, Color.yellow, Color.cyan};

	private float noMovementThreshold = 0.02f;
	private bool isMoving;
	private bool wasMoving;

	public int dronesToFeed = 5;


	private Vector3 previousLocation;
	public Vector3 startingLocation;
	public int movingFramesLimit = 2;
	private int frameNumberWithoutMoving;

	void Start() {
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
		GameObject[] drones = GameObject.FindGameObjectsWithTag ("drone");
		int rand;
		// Change the target and color ONLY if the state triggered from moving to not moving
		if (wasMoving && !isMoving && GetComponent<Renderer> ().enabled) {
			for (int i = 0; i < dronesToFeed; i++) {
				rand = Random.Range (0, drones.Length);
				drones [rand].GetComponent<DroneBehavior> ().targetPosition = startingLocation;
				drones [rand].GetComponent<DroneBehavior> ().chaseTarget = true;
				drones [rand].GetComponent<DroneBehavior> ().rendererColor = colors [face - 1];
			}
		} else {
			foreach(GameObject drone in drones) {
				drone.GetComponent<DroneBehavior> ().chaseTarget = false;
			}
		}
	}

	void OnCollisionEnter (Collision col)
	{
		Debug.Log ("Name of col: " + col.gameObject.name);
		if(col.gameObject.name == "Drone_Prefab(Clone)")
		{
			col.gameObject.GetComponent<DroneBehavior> ().chaseTarget = false;
		}
	}

	private void calculateMovement() {
		if (Vector3.Distance (previousLocation, transform.position) <= noMovementThreshold) {
			frameNumberWithoutMoving++;
		} else {
			frameNumberWithoutMoving = 0;
			isMoving = true;
		}
		if (frameNumberWithoutMoving == movingFramesLimit) {
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
