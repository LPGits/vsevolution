using UnityEngine;
using System.Collections;

public class PrefabInstantiator : MonoBehaviour {
	[SerializeField]
	GameObject _prefab;

	[SerializeField]
	int _amount = 100;


	// Use this for initialization
	void Start() 
	{
		GameObject tempPrefab;
		for (int i = 0; i < _amount; i++)
		{
			tempPrefab = (GameObject) GameObject.Instantiate(_prefab);
			tempPrefab.transform.parent = transform;
			tempPrefab.transform.position = new Vector3(0, 0.03f, 0);
		}
		Destroy(this);
	}
	
}
