using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DefineColor
{
  public string colorName { get; set; }
  public string hexCode { get; set; }

  /// <summary>
  /// Creates a new DefineColor based on the specified input text.
  /// </summary>
  /// <param name="input">The input text to get a match for. For example, "blue = #00f".</param>
  /// <returns>Returns the new DefineColor, or null if a no matches were found for the specified input.</returns>
  public static DefineColor Create(string input)
  {
    const string pattern = @"^(?<colorName>\w+)\s*=\s*(?<hexCode>#([0-9a-f]){3}(([0-9a-f]){3})?)";

    Regex regex = new Regex(pattern);
    MatchCollection matches = regex.Matches(input);
    if (matches.Count == 0)
      return null;

    DefineColor defineColor = new DefineColor();
    defineColor.colorName = RegexHelper.GetValue<string>(matches, "colorName");
    defineColor.hexCode = RegexHelper.GetValue<string>(matches, "hexCode");
    return defineColor;
  }
}

// Sample usage: 
// DefineColor defineColor = DefineColor.Create("blue = #00f");

