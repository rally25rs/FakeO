using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FakeO.Extenstions;

namespace FakeO
{
  /// <summary>
  /// Generates a string of random characters.
  /// </summary>
  public static class String
  {
    private static readonly string[] _upper = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    private static readonly string[] _lower = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
    private static readonly string[] _digit = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
    private static readonly string[] _punct = new string[] { "!", "\'", "#", "$", "%", "&", "\"", "(", ")", "*", "+", ",", "-", ".", "/", ":", ";", "<", "=", ">", "?", "@", "[", "\\", "]", "^", "_", "`", "{", "|", "}", "~" };
    private static readonly string[] _any = _upper.Union(_lower).Union(_digit).Union(_punct).ToArray();
    private static readonly string[] _salt = _upper.Union(_lower).Union(_digit).Union(new string[] { ".", "/" } ).ToArray();
    private static readonly Dictionary<string, string[]> patterns;

    static String()
    {
      patterns = new Dictionary<string, string[]>();
      patterns.Add(@".", _any);
      patterns.Add(@"\d", _digit);
      patterns.Add(@"\D", _upper.Union(_lower).Union(_punct).ToArray());
      patterns.Add(@"\w", _upper.Union(_lower).Union(_digit).Union(new string[] { "_" }).ToArray());
      patterns.Add(@"\W", _punct.Except(new string[] { "_" }).ToArray());
      patterns.Add(@"\s", new string[] { " ", "\t" });
      patterns.Add(@"\S", _any);
      patterns.Add(@"\t", new string[] { "\t" });
      patterns.Add(@"\n", new string[] { "\n" });
      patterns.Add(@"\r", new string[] { "\r" });
      patterns.Add(@"\f", new string[] { "\f" });
      patterns.Add(@"\a", new string[] { "\a" });
      //patterns.Add(@"\e", new string[] { "\e" });
    }

    /// <summary>
    /// Generates a string with a specifies length.
    /// Characters will all be printable [A-Za-z]
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string Random(int length)
    {
      return Random("[A-Za-z]{" + length.ToString() + "}");
    }

    /// <summary>
    /// Generate a string that fits the format of a regex.
    /// (Please, keep the regex simple if you don't want this to break)
    /// </summary>
    /// <param name="regex">A regex to use to define the format of the string.</param>
    /// <returns>A string that fits the format of the regex (the return string will pass Regex.IsMatch)</returns>
    public static string Random(string regex)
    {
      var charEnum = regex.GetEnumerator();
      var str = new StringBuilder();

      while (charEnum.MoveNext())
      {
        if (charEnum.Current == '\\')
        {
          charEnum.MoveNext();
          if (patterns.ContainsKey("\\" + new string(charEnum.Current, 1)))
            str.Append(patterns["\\" + new string(charEnum.Current, 1)].Random());
          else
            str.Append(new string(charEnum.Current, 1));
        }
        else if (charEnum.Current == '.')
        {
          str.Append(_any.Random());
        }
        else if (charEnum.Current == '[') // begin a range: "[A-Z]"
        {
          int start = -1;
          while (charEnum.MoveNext() && charEnum.Current != ']')
          {
            if (charEnum.Current == '-' && start != -1)
            {
              int end = -1;
              charEnum.MoveNext();
              if (start <= (int)charEnum.Current)
              {
                end = (int)charEnum.Current;
              }
              else
              {
                end = start;
                start = (int)charEnum.Current;
              }
              var charsInRange = new string[(end - start) + 1];
              for (int i = 0; i <= (end - start); i++)
                charsInRange[i] = new string((char)(start + i), 1);
              str.Append(charsInRange.Random());
              start = -1;
            }
            else
            {
              start = (int)charEnum.Current;
            }
          }
          if (charEnum.Current != ']')
            throw new InvalidOperationException("unmatched []");
        }
        else if (charEnum.Current == '{')
        {
          string minStr = "";
          string maxStr = "";
          bool hitComma = false;

          while (charEnum.MoveNext() && charEnum.Current != '}')
          {
            if (charEnum.Current == ',')
              hitComma = true;
            else
            {
              if (!hitComma)
                minStr += new string(charEnum.Current, 1);
              else
                maxStr += new string(charEnum.Current, 1);
            }
          }
          if (charEnum.Current != '}')
            throw new InvalidOperationException("unmatched {}");

          if (string.IsNullOrEmpty(maxStr))
            maxStr = minStr;
          var min = Int32.Parse(minStr);
          var max = Int32.Parse(maxStr);
          var timesToAdd = (min == max ? min : Number.Next(min, max));
          var charToAdd = new string(str[str.Length - 1], 1);
          str.Remove(str.Length - 1, 1);
          for(int i = 0; i < timesToAdd; i++)
            str.Append(charToAdd);
        }
        else
          str.Append(new string(charEnum.Current, 1));
      }

      return str.ToString();
    }


    //TODO: this is unimplemented.
    // This was going to be a port of Perl's String.Random found here:
    // http://cpansearch.perl.org/src/STEVE/String-Random-0.22/lib/String/Random.pm
    [Obsolete("Not fully implemented yet.", true)]
    private static void RegCh(CharEnumerator charEnum, string regex, List<KeyValuePair<string, string[]>> parsed)
    {
      if (charEnum.Current == '\\') // begins an escaped character
      {
        charEnum.MoveNext();
        if (charEnum.Current == 'x') // a number in octal hex: "\x99"
        {
          throw new NotImplementedException("Octal hex characters following the pattern \"\\x99\" are not yet implemented.");
        }
        else if (patterns.ContainsKey("\\" + charEnum.Current.ToString()))
        {
          parsed.Add(new KeyValuePair<string, string[]>(regex, patterns["\\" + charEnum.Current.ToString()]));
        }
        else
        {
          parsed.Add(new KeyValuePair<string,string[]>(regex, new string[] { charEnum.Current.ToString() }));
        }
      }

      if (charEnum.Current == '.') // any character
      {
        parsed.Add(new KeyValuePair<string, string[]>(regex, patterns[charEnum.Current.ToString()]));
      }

      if (charEnum.Current == '[') // begin a range: "[A-Z]"
      {
        int start = -1;
        while (charEnum.MoveNext() && charEnum.Current != ']')
        {
          if (charEnum.Current == '-' && start != -1)
          {
            int end = -1;
            charEnum.MoveNext();
            if (start <= (int)charEnum.Current)
            {
              end = (int)charEnum.Current;
            }
            else
            {
              end = start;
              start = (int)charEnum.Current;
            }
            var charsInRange = new string[(end - start) + 1];
            for (int i = 0; i <= (end - start); i++)
              charsInRange[i] = ((char)start + i).ToString();
            parsed.Add(new KeyValuePair<string, string[]>(regex, charsInRange));
            start = -1;
          }
          else
          {
            start = (int)charEnum.Current;
          }
        }
        if (charEnum.Current != ']')
          throw new InvalidOperationException("unmatched []");
      }

      if (charEnum.Current == '*') // matches 0 or more times
      {
      }
    }
  }
}
