using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
	public GameObject Cone;
	bool ableToCreateVector;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (ableToCreateVector)
		{
			ableToCreateVector = false;
			CreateVector(7, 7, 8);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			ableToCreateVector = true;
		}
	}

	void CreateVector(int x, int y, int z)
	{
		Vector3 position = new Vector3(x, y, z);
		// We are going to assume the beginning of the vector is the origin (0, 0, 0).
		Instantiate(Cone, position, Quaternion.identity);
		//Debug.Log("Creating Vector");
	}
}
