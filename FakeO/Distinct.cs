using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FakeO
{
  /// <summary>
  /// Creates distinct values, not really random.
  /// Within the same AppDomain, the same 2 values shouldnt be repeated.
  /// Useful for objects that need unique IDs.
  /// </summary>
  public static class Distinct
  {
    private static int nextNumber = 1;

    /// <summary>
    /// Creates a new Guid
    /// </summary>
    /// <returns>A new Guid</returns>
    public static Guid Guid()
    {
      return System.Guid.NewGuid();
    }

    /// <summary>
    /// Returns a 32-character unique string
    /// This is actually just a Guid.ToString("N")
    /// </summary>
    /// <returns>Returns a 32-character unique string</returns>
    public static string String()
    {
      return Guid().ToString("N");
    }

    /// <summary>
    /// Returns a distinct number.
    /// The numbers run up in sequence, starting at 1.
    /// </summary>
    /// <returns>A number.</returns>
    public static int Number()
    {
      return nextNumber++;
    }
  }
}
