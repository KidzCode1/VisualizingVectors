using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SetNamedColor
{
  public string vectorName { get; set; }
  public string colorName { get; set; }

  /// <summary>
  /// Creates a new SetNamedColor based on the specified input text.
  /// </summary>
  /// <param name="input">The input text to get a match for. For example, "a.color = red".</param>
  /// <returns>Returns the new SetNamedColor, or null if a no matches were found for the specified input.</returns>
  public static SetNamedColor Create(string input)
  {
    const string pattern = @"^(?<vectorName>\w+).color\s*=\s*(?<colorName>\w+)$";

    Regex regex = new Regex(pattern);
    MatchCollection matches = regex.Matches(input);
    if (matches.Count == 0)
      return null;

    SetNamedColor setNamedColor = new SetNamedColor();
    setNamedColor.vectorName = RegexHelper.GetValue<string>(matches, "vectorName");
    setNamedColor.colorName = RegexHelper.GetValue<string>(matches, "colorName");
    return setNamedColor;
  }
}

// Sample usage: 
// SetNamedColor setNamedColor = SetNamedColor.Create("a.color = red");

