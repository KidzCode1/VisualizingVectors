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
	public GameObject GameObject { get; set; }

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