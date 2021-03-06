using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class OffsetVector
{
  public string vectorName { get; set; }
  public double offsetX { get; set; }
  public double offsetY { get; set; }
  public double offsetZ { get; set; }

  /// <summary>
  /// Creates a new OffsetVector based on the specified input text.
  /// </summary>
  /// <param name="input">The input text to get a match for. For example, "a + (1, 2, -3.222)".</param>
  /// <returns>Returns the new OffsetVector, or null if a no matches were found for the specified input.</returns>
  public static OffsetVector Create(string input)
  {
    const string pattern = @"^(?<vectorName>\w+)\s*\+\s*\(?(?<offsetX>[+-]?((\d+(\.\d+)?)))[, ]\s*(?<offsetY>[+-]?((\d+(\.\d+)?)))[, ]\s*(?<offsetZ>[+-]?((\d+(\.\d+)?)))\s*\)?\s*$";

    Regex regex = new Regex(pattern);
    MatchCollection matches = regex.Matches(input);
    if (matches.Count == 0)
      return null;

    OffsetVector offsetVector = new OffsetVector();
    offsetVector.vectorName = RegexHelper.GetValue<string>(matches, "vectorName");
    offsetVector.offsetX = RegexHelper.GetValue<double>(matches, "offsetX");
    offsetVector.offsetY = RegexHelper.GetValue<double>(matches, "offsetY");
    offsetVector.offsetZ = RegexHelper.GetValue<double>(matches, "offsetZ");
    return offsetVector;
  }
}

// Sample usage: 
// OffsetVector offsetVector = OffsetVector.Create("a + (1, 2, -3.222)");

