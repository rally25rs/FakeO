using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FakeO
{
  public static class Data
  {
    /// <summary>
    /// Tries to guess at a random data value for a given type.
    /// </summary>
    /// <typeparam name="T">The type to generate data for.</typeparam>
    /// <returns>Random data of the requested type.</returns>
    public static T Random<T>()
    {
      return (T)Random(typeof(T));
    }

    /// <summary>
    /// Tries to guess at a random data value for a given type.
    /// </summary>
    /// <param name="t">The type to generate data for.</param>
    /// <returns>Random data of the requested type.</returns>
    public static object Random(Type t)
    {
      if (t == typeof(byte) || t == typeof(byte?))
        return (byte)Number.Next(Byte.MinValue, Byte.MaxValue);
      if (t == typeof(short) || t == typeof(short?))
        return (short)Number.Next(Int16.MinValue, Int16.MaxValue);
      if (t == typeof(int) || t == typeof(int?))
        return Number.Next(Int32.MinValue, Int32.MaxValue);
      if (t == typeof(long) || t == typeof(long?))
        return (long)Number.Next(Int32.MinValue, Int32.MaxValue);
      if (t == typeof(float) || t == typeof(float?))
        return Number.NextFloat();
      if (t == typeof(double) || t == typeof(double?))
        return Number.NextDouble();
      if (t == typeof(decimal) || t == typeof(decimal?))
        return Number.NextDouble();
      if (t == typeof(char) || t == typeof(char?))
        return String.Random(1)[0];
      if (t == typeof(string))
        return String.Random(10);
      if (t == typeof(DateTime) || t == typeof(DateTime?))
        return new DateTime(Number.Next(1900, 2100), Number.Next(1, 12), Number.Next(1, 29), Number.Next(1,24), Number.Next(0,59), Number.Next(0,59));
      if (t == typeof(TimeSpan) || t == typeof(TimeSpan?))
        return new TimeSpan(Number.Next(0, 10), Number.Next(1, 24), Number.Next(0, 59), Number.Next(0, 59));
      if (t.IsEnum)
      {
        var vals = Enum.GetValues(t);
        return vals.GetValue(Number.Next(0, vals.Length-1));
      }
      if (t.IsValueType)
        return 0;
      return null;
    }
  }
}
