using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
	public GameObject Cone;
	GameObject liveVector;
	public InputField InputField;
	bool ableToCreateVector;
	// Start is called before the first frame update
	void Start()
	{

	}

	string GetLastLine(string textToCaret)
	{
		int lastNewLine = textToCaret.LastIndexOf('\n');
		int startOfThisLine = 0;
		if (lastNewLine >= 0)
			startOfThisLine = lastNewLine + 1;

		return textToCaret.Substring(startOfThisLine);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			ableToCreateVector = true;
		}

		if (Input.GetKey(KeyCode.Return))
		{
			if (InputField.isFocused)
			{
				string textToCaret = InputField.text.Substring(0, InputField.caretPosition - 1);  // Trim away the last "\n"
				string lineWeJustTyped = GetLastLine(textToCaret);
				ExecuteCommandLine(lineWeJustTyped);
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (ableToCreateVector)
		{
			ableToCreateVector = false;
			//CreateVector(6, 4, 2);
		}
	}
	List<SuperVector> superVectors = new List<SuperVector>();

	void RemoveNamedVector(string vectorName)
	{
		SuperVector matchingVector = GetVectorByName(vectorName);
		if (matchingVector == null)
			return;
		superVectors.Remove(matchingVector);
		Destroy(matchingVector.GameObject);
	}

	private SuperVector GetVectorByName(string vectorName)
	{
		return superVectors.FirstOrDefault(test => test.vectorName == vectorName);
	}

	void AddNamedVector(SuperVector superVector)
	{
		superVectors.Add(superVector);
	}

	void ChangeVectorColor(string vectorName, string hexCode)
	{
		SuperVector matchingVector = GetVectorByName(vectorName);
		if (matchingVector == null)
			return;

		Color color;
		if (ColorUtility.TryParseHtmlString(hexCode, out color))
			SetVectorColor(matchingVector, color);
	}

	private static void SetVectorColor(SuperVector vector, Color color)
	{
		MeshRenderer[] coneMeshRenderers = vector.GameObject.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in coneMeshRenderers)
		{
			meshRenderer.material.color = color;
		}
	}

	public void TextSubmitted(InputField i)
	{
		Debug.Log("TextSubmitted!");
		if (Input.GetKey(KeyCode.Return))
		{
			i.ActivateInputField();
		}
			//Do stuff here
	}

	public void TextChanged(string text)
	{
		//ExecuteCommandLine(InputField.text);
	}

	private void ExecuteCommandLine(string text)
	{
		SuperVector superVector = SuperVector.Create(text);
		if (superVector != null)
		{
			CreateOrUpdateSuperVector(superVector);
		}
		SetColor setColor = SetColor.Create(text);
		if (setColor != null)
		{
			ChangeVectorColor(setColor.vectorName, setColor.hexCode);
		}
	}

	Color GetVectorColor(string vectorName)
	{
		SuperVector matchingVector = GetVectorByName(vectorName);
		if (matchingVector == null)
			return Color.black;

		MeshRenderer[] coneMeshRenderers = matchingVector.GameObject.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in coneMeshRenderers)
			return meshRenderer.material.color;

		return Color.black;
	}

	private void CreateOrUpdateSuperVector(SuperVector superVector)
	{
		bool unnamed = string.IsNullOrEmpty(superVector.vectorName);
		Color nextVectorColor = GetVectorColor(superVector.vectorName);
		if (unnamed)
		{
			DestroyLiveVectorIfItExists();
		}
		else
		{
			RemoveNamedVector(superVector.vectorName);
		}

		GameObject vectorGameObject = CreateVector(superVector.X, superVector.Y, superVector.Z, superVector.tailX, superVector.tailY, superVector.tailZ);

		if (unnamed)
			liveVector = vectorGameObject;
		else
		{
			superVector.GameObject = vectorGameObject;
			SetVectorColor(superVector, nextVectorColor);
			AddNamedVector(superVector);
		}
	}

	private void DestroyLiveVectorIfItExists()
	{
		if (liveVector != null)
			Destroy(liveVector);
	}

	GameObject CreateVector(double x, double y, double z, double tailX = 0, double tailY = 0, double tailZ = 0)
	{
		double deltaX = x - tailX;
		double deltaY = y - tailY;
		double deltaZ = z - tailZ;

		Vector3 vectorDirection = new Vector3((float)deltaX, (float)deltaY, (float)deltaZ);
		Vector3 position = new Vector3((float)x, (float)y, (float)z);
		double lengthToPoint = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2) + Math.Pow(deltaZ, 2));
		double vectorShaftLength = lengthToPoint - 2 * 0.9;

		GameObject vector = Instantiate(Cone, position, Quaternion.LookRotation(vectorDirection));
		Transform[] components = vector.GetComponentsInChildren<Transform>();
		foreach (Transform transform in components)
		{
			if (transform.name == "VectorShaft")
			{
				transform.localScale = new Vector3(1, 1, (float)vectorShaftLength);
				break;
			}
		}

		return vector;
	}
}
