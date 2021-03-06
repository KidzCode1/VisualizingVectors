using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class NegativeVector
{
  public string vectorName { get; set; }

  /// <summary>
  /// Creates a new NegativeVector based on the specified input text.
  /// </summary>
  /// <param name="input">The input text to get a match for. For example, "-myVector".</param>
  /// <returns>Returns the new NegativeVector, or null if a no matches were found for the specified input.</returns>
  public static NegativeVector Create(string input)
  {
    const string pattern = @"^(-(?<vectorName>\w+))$";

    Regex regex = new Regex(pattern);
    MatchCollection matches = regex.Matches(input);
    if (matches.Count == 0)
      return null;

    NegativeVector negativeVector = new NegativeVector();
    negativeVector.vectorName = RegexHelper.GetValue<string>(matches, "vectorName");
    return negativeVector;
  }
}

// Sample usage: 
// NegativeVector negativeVector = NegativeVector.Create("-myVector");

