using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AssignNegativeVector
{
  public string newVectorName { get; set; }
  public string existingVectorName { get; set; }

  /// <summary>
  /// Creates a new AssignNegativeVector based on the specified input text.
  /// </summary>
  /// <param name="input">The input text to get a match for. For example, "myVector2 = -myVector".</param>
  /// <returns>Returns the new AssignNegativeVector, or null if a no matches were found for the specified input.</returns>
  public static AssignNegativeVector Create(string input)
  {
    const string pattern = @"^(?<newVectorName>\w+)\s*=\s*(-(?<existingVectorName>\w+))$";

    Regex regex = new Regex(pattern);
    MatchCollection matches = regex.Matches(input);
    if (matches.Count == 0)
      return null;

    AssignNegativeVector assignNegativeVector = new AssignNegativeVector();
    assignNegativeVector.newVectorName = RegexHelper.GetValue<string>(matches, "newVectorName");
    assignNegativeVector.existingVectorName = RegexHelper.GetValue<string>(matches, "existingVectorName");
    return assignNegativeVector;
  }
}

// Sample usage: 
// AssignNegativeVector assignNegativeVector = AssignNegativeVector.Create("myVector2 = -myVector");
