using UnityEngine;
using System.Collections;

public class TestLocalPosition : MonoBehaviour {

	public float movementSpeed = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);

	}
}
