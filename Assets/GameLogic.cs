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
		SuperVector.VectorPrototype = Cone;
		colorLookups.Add("red", "#f00");
		colorLookups.Add("blue", "#00f");
		colorLookups.Add("green", "#0f0");
		colorLookups.Add("white", "#fff");
		colorLookups.Add("gray", "#888");
		colorLookups.Add("black", "#000");
		colorLookups.Add("purple", "#5400d1");
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

	void RemoveNamedVector(string vectorName, bool suppressErrors = true)
	{
		SuperVector matchingVector = GetVectorByName(vectorName, suppressErrors);
		if (matchingVector == null)
			return;
		superVectors.Remove(matchingVector);
		matchingVector.DestroyGameObject();
	}

	private SuperVector GetVectorByName(string vectorName, bool suppressErrors = false)
	{
		SuperVector superVector = superVectors.FirstOrDefault(test => test.vectorName == vectorName);
		if (superVector == null && !suppressErrors)
			Debug.LogError($"We can't find a vector named \"{vectorName}\"!");
		return superVector;
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
			matchingVector.SetVectorColor(color);
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

	Dictionary<string, string> colorLookups = new Dictionary<string, string>();

	void DefineColorName(string colorName, string hexCode)
	{
		if (!colorLookups.ContainsKey(colorName))
			colorLookups.Add(colorName, hexCode);
		else
			colorLookups[colorName] = hexCode;
	}

	void MoveVectorTo(string vector1, string vector2)
	{
		SuperVector superVector1 = GetVectorByName(vector1);
		if (superVector1 == null)
			return;

		SuperVector superVector2 = GetVectorByName(vector2);
		if (superVector2 == null)
			return;

		// If we are here, we know that both superVector1 and superVector2 are both good.
		superVector2.MoveTailTo(superVector1.X, superVector1.Y, superVector1.Z);
	}

	private void SumAllVectors(string newVectorName, string vector1, string vector2, string vector3, string vector4, string vector5)
	{
		RemoveNamedVector(newVectorName);

		SuperVector superVector1 = GetVectorByName(vector1);
		if (superVector1 == null)
			return;

		SuperVector superVector2 = GetVectorByName(vector2);
		if (superVector2 == null)
			return;

		SuperVector superVector3 = GetVectorByName(vector3);
		SuperVector superVector4 = GetVectorByName(vector4);
		SuperVector superVector5 = GetVectorByName(vector5);

		SuperVector superVector = new SuperVector();
		superVector.vectorName = newVectorName;
		superVector.SetTo(superVector1);
		superVector.Add(superVector2);
		superVector.Add(superVector3);
		superVector.Add(superVector4);
		superVector.Add(superVector5);
		superVector.RefreshGameObject();
		AddNamedVector(superVector);
	}

	Color GetVectorColor(string vectorName)
	{
		SuperVector matchingVector = GetVectorByName(vectorName, true);
		if (matchingVector == null)
			return Color.black;

		MeshRenderer[] coneMeshRenderers = matchingVector.VectorGameObject.GetComponentsInChildren<MeshRenderer>();
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

		GameObject vectorGameObject = superVector.CreateGameObject();

		if (unnamed)
			liveVector = vectorGameObject;
		else
		{
			superVector.SetVectorColor(nextVectorColor);
			AddNamedVector(superVector);
		}
	}

	private void DestroyLiveVectorIfItExists()
	{
		if (liveVector != null)
			Destroy(liveVector);
	}

	private void ExecuteCommandLine(string text)
	{
		SuperVector superVector = SuperVector.Create(text);
		if (superVector != null)
		{
			CreateOrUpdateSuperVector(superVector);
			return;
		}

		SetColor setColor = SetColor.Create(text);
		if (setColor != null)
		{
			ChangeVectorColor(setColor.vectorName, setColor.hexCode);
			return;
		}

		SetNamedColor setNamedColor = SetNamedColor.Create(text);
		if (setNamedColor != null)
		{
			string hexCode;
			if (colorLookups.ContainsKey(setNamedColor.colorName))
			{
				hexCode = colorLookups[setNamedColor.colorName];
				ChangeVectorColor(setNamedColor.vectorName, hexCode);
			}
			else
				Debug.LogError($"\"{setNamedColor.colorName}\" needs to be defined first!!!");
			return;
		}

		DefineColor defineColor = DefineColor.Create(text);
		if (defineColor != null)
		{
			DefineColorName(defineColor.colorName, defineColor.hexCode);
			return;
		}

		MoveVector moveVector = MoveVector.Create(text);
		if (moveVector != null)
		{
			MoveVectorTo(moveVector.vector1, moveVector.vector2);
			return;
		}

		SumVectors sumVectors = SumVectors.Create(text);
		if (sumVectors != null)
		{
			SumAllVectors(sumVectors.newVectorName, sumVectors.vector1, sumVectors.vector2, sumVectors.vector3, sumVectors.vector4, sumVectors.vector5);
			return;
		}

	}
}
