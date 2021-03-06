using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class MoveVector
{
  public string vector1 { get; set; }
  public string vector2 { get; set; }

  /// <summary>
  /// Creates a new MoveVector based on the specified input text.
  /// </summary>
  /// <param name="input">The input text to get a match for. For example, "a + b".</param>
  /// <returns>Returns the new MoveVector, or null if a no matches were found for the specified input.</returns>
  public static MoveVector Create(string input)
  {
    const string pattern = @"^(?<vector1>\w+)\s*\+\s*(?<vector2>\w+)$";

    Regex regex = new Regex(pattern);
    MatchCollection matches = regex.Matches(input);
    if (matches.Count == 0)
      return null;

    MoveVector moveVector = new MoveVector();
    moveVector.vector1 = RegexHelper.GetValue<string>(matches, "vector1");
    moveVector.vector2 = RegexHelper.GetValue<string>(matches, "vector2");
    return moveVector;
  }
}

// Sample usage: 
// MoveVector moveVector = MoveVector.Create("a + b");

