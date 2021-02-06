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
			CreateVector(3, 2, 1);
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			ableToCreateVector = true;
		}
	}

	void CreateVector(double endX, double endY, double endZ, double startX = 0, double startY = 0, double startZ = 0)
	{
		double deltaX = endX - startX;
		double deltaY = endY - startY;
		double deltaZ = endZ - startZ;
		double halfPosX = startX + deltaX / 2;
		double halfPosY = startY + deltaY / 2;
		double halfPosZ = startZ + deltaZ / 2;
		Vector3 vectorDirection = new Vector3((float)deltaX, (float)deltaY, (float)deltaZ);
		Vector3 position = new Vector3((float)endX, (float)endY, (float)endZ);
		double length = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2) + Math.Pow(deltaZ, 2));

		GameObject cone = Instantiate(Cone, position, Quaternion.LookRotation(vectorDirection));
		//Debug.Log("Creating Vector");
		GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
		cylinder.transform.localPosition = new Vector3((float)halfPosX, (float)halfPosY, (float)halfPosZ);
		Quaternion lookRotation = Quaternion.LookRotation(vectorDirection);
		cylinder.transform.localRotation = lookRotation;
		cylinder.transform.Rotate(Vector3.right, 90);  // Because the cylinder's default orientation appears to be completely wrong!
		cylinder.transform.localScale = new Vector3(0.5f, (float)length / 2, 0.5f); // For some reason the cylinder scales twice as long as a cube
	}
}
