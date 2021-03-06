using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class VectorCommand
{
  public string command { get; set; }
  public string vectorName { get; set; }

  /// <summary>
  /// Creates a new VectorCommand based on the specified input text.
  /// </summary>
  /// <param name="input">The input text to get a match for. For example, "hide a".</param>
  /// <returns>Returns the new VectorCommand, or null if a no matches were found for the specified input.</returns>
  public static VectorCommand Create(string input)
  {
    const string pattern = @"^(?<command>\w+)\s*(?<vectorName>\w+)$";

    Regex regex = new Regex(pattern);
    MatchCollection matches = regex.Matches(input);
    if (matches.Count == 0)
      return null;

    VectorCommand vectorCommand = new VectorCommand();
    vectorCommand.command = RegexHelper.GetValue<string>(matches, "command");
    vectorCommand.vectorName = RegexHelper.GetValue<string>(matches, "vectorName");
    return vectorCommand;
  }
}

// Sample usage: 
// VectorCommand vectorCommand = VectorCommand.Create("hide a");

