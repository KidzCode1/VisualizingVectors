using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SumVectors
{
  public string newVectorName { get; set; }
  public string vector1 { get; set; }
  public string vector2 { get; set; }
  public string vector3 { get; set; }
  public string vector4 { get; set; }
  public string vector5 { get; set; }

  /// <summary>
  /// Creates a new SumVectors based on the specified input text.
  /// </summary>
  /// <param name="input">The input text to get a match for. For example, "c = a + b+ d + e + f".</param>
  /// <returns>Returns the new SumVectors, or null if a no matches were found for the specified input.</returns>
  public static SumVectors Create(string input)
  {
    const string pattern = @"^(?<newVectorName>\w+)\s*=\s*(?<vector1>\w+)\s*\+\s*(?<vector2>\w+)(\s*\+\s*(?<vector3>\w+))?(\s*\+\s*(?<vector4>\w+))?(\s*\+\s*(?<vector5>\w+))?$";

    Regex regex = new Regex(pattern);
    MatchCollection matches = regex.Matches(input);
    if (matches.Count == 0)
      return null;

    SumVectors sumVectors = new SumVectors();
    sumVectors.newVectorName = RegexHelper.GetValue<string>(matches, "newVectorName");
    sumVectors.vector1 = RegexHelper.GetValue<string>(matches, "vector1");
    sumVectors.vector2 = RegexHelper.GetValue<string>(matches, "vector2");
    sumVectors.vector3 = RegexHelper.GetValue<string>(matches, "vector3");
    sumVectors.vector4 = RegexHelper.GetValue<string>(matches, "vector4");
    sumVectors.vector5 = RegexHelper.GetValue<string>(matches, "vector5");
    return sumVectors;
  }
}

// Sample usage: 
// SumVectors sumVectors = SumVectors.Create("c = a + b+ d + e + f");


