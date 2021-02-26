using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SetColor
{
  public string vectorName { get; set; }
  public string hexCode { get; set; }

  /// <summary>
  /// Creates a new SetColor based on the specified input text.
  /// </summary>
  /// <param name="input">The input text to get a match for. For example, "a.SetColor(#ffffff)".</param>
  /// <returns>Returns the new SetColor, or null if a no matches were found for the specified input.</returns>
  public static SetColor Create(string input)
  {
    const string pattern = @"^(?<vectorName>\w+).SetColor\((?<hexCode>#([0-9a-f]){6})\)";

    Regex regex = new Regex(pattern);
    MatchCollection matches = regex.Matches(input);
    if (matches.Count == 0)
      return null;

    SetColor setColor = new SetColor();
    setColor.vectorName = RegexHelper.GetValue<string>(matches, "vectorName");
    setColor.hexCode = RegexHelper.GetValue<string>(matches, "hexCode");
    return setColor;
  }
}