using System;
using System.Text.RegularExpressions;
using UnityEngine;

public class SuperVector
{
  public string vectorName { get; set; }
  public double tailX { get; set; }
  public double tailY { get; set; }
  public double tailZ { get; set; }
  public double X { get; set; }
  public double Y { get; set; }
  public double Z { get; set; }
  public double deltaX { get => X - tailX; }
	public double deltaY { get => Y - tailY; }
	public double deltaZ { get => Z - tailZ; }
  public static GameObject VectorPrototype;
	Color color;


	public void MoveRelative(double deltaX, double deltaY, double deltaZ)
	{
    tailX += deltaX;
		tailY += deltaY;
		tailZ += deltaZ;
    X += deltaX;
    Y += deltaY;
    Z += deltaZ;
    RefreshGameObject();
  }

	public void RefreshGameObject()
	{
    DestroyGameObject();
    CreateGameObject();
	}

	public void MoveTailTo(double x, double y, double z)
  {
    MoveRelative(x - tailX, y - tailY, z - tailZ);
  }

  public GameObject VectorGameObject { get; set; }

	/// <summary>
	/// Creates a new SuperVector based on the specified input text.
	/// </summary>
	/// <param name="input">The input text to get a match for. For example, "a = (1, 2, 3) -> (2, 2, 2)".</param>
	/// <returns>Returns the new SuperVector, or null if a no matches were found for the specified input.</returns>
	public static SuperVector Create(string input)
  {
    const string pattern = @"^((?<vectorName>\w+)\s*=\s*)?(\(?(?<tailX>[+-]?((\d+(\.\d+)?)))[, ]\s*(?<tailY>[+-]?((\d+(\.\d+)?)))[, ]\s*(?<tailZ>[+-]?((\d+(\.\d+)?)))\s*\)?\s*->)?\s*\(?(?<X>[+-]?((\d+(\.\d+)?)))[, ]\s*(?<Y>[+-]?((\d+(\.\d+)?)))[, ]\s*(?<Z>[+-]?((\d+(\.\d+)?)))\s*\)?$";

    Regex regex = new Regex(pattern);
    MatchCollection matches = regex.Matches(input);
    if (matches.Count == 0)
      return null;

    SuperVector superVector = new SuperVector();
    superVector.vectorName = RegexHelper.GetValue<string>(matches, "vectorName");
    superVector.tailX = RegexHelper.GetValue<double>(matches, "tailX");
    superVector.tailY = RegexHelper.GetValue<double>(matches, "tailY");
    superVector.tailZ = RegexHelper.GetValue<double>(matches, "tailZ");
    superVector.X = RegexHelper.GetValue<double>(matches, "X");
    superVector.Y = RegexHelper.GetValue<double>(matches, "Y");
    superVector.Z = RegexHelper.GetValue<double>(matches, "Z");
    return superVector;
  }

	public void SetVectorColor(Color color)
  {
		this.color = color;
		MeshRenderer[] coneMeshRenderers = VectorGameObject.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in coneMeshRenderers)
			meshRenderer.material.color = color;
	}

  public GameObject CreateGameObject()
  {
    double deltaX = X - tailX;
    double deltaY = Y - tailY;
    double deltaZ = Z - tailZ;

    Vector3 vectorDirection = new Vector3((float)deltaX, (float)deltaY, (float)deltaZ);
    Vector3 position = new Vector3((float)X, (float)Y, (float)Z);
    double lengthToPoint = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2) + Math.Pow(deltaZ, 2));
    double vectorShaftLength = lengthToPoint - 2 * 0.9;

    VectorGameObject = GameObject.Instantiate(VectorPrototype, position, Quaternion.LookRotation(vectorDirection));
    Transform[] components = VectorGameObject.GetComponentsInChildren<Transform>();
    foreach (Transform transform in components)
    {
      if (transform.name == "VectorShaft")
      {
        transform.localScale = new Vector3(1, 1, (float)vectorShaftLength);
        break;
      }
    }

    if (color != null)
      SetVectorColor(color);

    return VectorGameObject;
  }

	public void DestroyGameObject()
	{
    if (VectorGameObject != null)
		  GameObject.Destroy(VectorGameObject);
  }
	public void Add(SuperVector superVector)
	{
		if (superVector == null)
			return;

		X += superVector.deltaX;
		Y += superVector.deltaY;
		Z += superVector.deltaZ;
	}

	public void SetTo(SuperVector superVector)
	{
    tailX = superVector.tailX;
    tailY = superVector.tailY;
    tailZ = superVector.tailZ;
    X = superVector.X;
    Y = superVector.Y;
    Z = superVector.Z;
  }
}

// Sample usage: 
// SuperVector superVector = SuperVector.Create("a = (1, 2, 3) -> (2, 2, 2)");

public static class RegexHelper
{
  public static T GetValue<T>(MatchCollection matches, string groupName)
  {
    foreach (Match match in matches)
    {
      GroupCollection groups = match.Groups;
      Group group = groups[groupName];
      if (group == null)
        continue;

      string value = group.Value;

      if (string.IsNullOrEmpty(value))
        return default(T);

      if (typeof(T).Name == typeof(double).Name)
        if (double.TryParse(value, out double result))
          return (T)(object)result;

      return (T)(object)value;
    }

    return default(T);
  }
}