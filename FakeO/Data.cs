using System;

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
      /// <param name="min">Minimum value.</param>
      /// <param name="max">Maximum value.</param>
      /// <returns>Random data of the requested type.</returns>
      public static object Random(Type t, double min = double.MinValue, double max = double.MaxValue)
    {
      if (t == typeof(bool))
        return RandomBool();
      if (t == typeof(bool?))
        return RandomBoolNullable();
      if (t == typeof(byte) || t == typeof(byte?))
        return RandomByte(min, max);
      if (t == typeof(short) || t == typeof(short?))
        return RandomShort(min, max);
      if (t == typeof(int) || t == typeof(int?))
        return RandomInt(min, max);
      if (t == typeof(long) || t == typeof(long?))
        return RandomLong(min, max);
      if (t == typeof(float) || t == typeof(float?))
        return RandomFloat(min, max);
      if (t == typeof(double) || t == typeof(double?))
        return RandomDouble(min, max);
      if (t == typeof(decimal) || t == typeof(decimal?))
        return RandomDecimal(min, max);
      if (t == typeof(char) || t == typeof(char?))
        return String.Random(1)[0];
      if (t == typeof(string))
        return RandomString(min, max);
      if (t == typeof(DateTime) || t == typeof(DateTime?))
        return new DateTime(Number.Next(1900, 2100), Number.Next(1, 12), Number.Next(1, 29), Number.Next(1,24), Number.Next(0,59), Number.Next(0,59));
      if (t == typeof(TimeSpan) || t == typeof(TimeSpan?))
        return new TimeSpan(Number.Next(0, 10), Number.Next(1, 24), Number.Next(0, 59), Number.Next(0, 59));
      if (t.IsEnum)
      {
        var vals = Enum.GetValues(t);
        return vals.GetValue(Number.Next(0, vals.Length-1));
      }
      if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
      {
        var t2 = Nullable.GetUnderlyingType(t);
        if (t2.IsEnum)
        {
          var vals = Enum.GetValues(t2);
          var n = Number.Next(0, vals.Length);
          return n == 0 ? null : vals.GetValue(n - 1);
        }
      }
      if (t.IsValueType)
        return 0;
      return null;
    }

      private static string RandomString(double min, double max)
      {
          var length = RandomShort(min, max);
          if (max == double.MaxValue)
              length = 10;
          return String.Random(length);
      }

      private static decimal RandomDecimal(double min, double max)
      {
          if ((double)decimal.MinValue > min)
              min = (double)decimal.MinValue;
          if ((double)decimal.MaxValue < max)
              max = (double)decimal.MaxValue;
          return (decimal)RandomDouble(min, max);
      }

      private static double RandomDouble(double min, double max)
      {
          return Number.NextDouble(min, max);
      }

      private static float RandomFloat(double min, double max)
      {
          var minimum = ToRange(min, float.MinValue, float.MaxValue);
          var maximum = ToRange(max, float.MinValue, float.MaxValue);
          return Number.NextFloat((float)minimum, (float)maximum);
      }

      private static object RandomLong(double min, double max)
      {
          return (long)RandomInt(min, max);
      }

      private static int RandomInt(double min, double max)
      {
          var minimum = ToRange(min, Int32.MinValue, Int32.MaxValue);
          var maximum = ToRange(max, Int32.MinValue, Int32.MaxValue);
          return Number.Next((int)minimum, (int)maximum);
      }

      private static short RandomShort(double min, double max)
      {
          var minimum = ToRange(min, Int16.MinValue, Int16.MaxValue);
          var maximum = ToRange(max, Int16.MinValue, Int16.MaxValue);
          return (short)Number.Next((short)minimum, (short)maximum);
      }

      private static byte RandomByte(double min, double max)
      {
          var minimum = ToRange(min, Byte.MinValue, Byte.MaxValue);
          var maximum = ToRange(max, Byte.MinValue, Byte.MaxValue);
          return (byte)Number.Next((byte)minimum, (byte)maximum);
      }

      private static bool? RandomBoolNullable()
      {
          var x = Number.Next(0, 100);
          if (x <= 33)
              return false;
          if (x <= 66)
              return true;
          return null;
      }

      private static bool RandomBool()
      {
          return !(Number.Next(0, 100) < 50);
      }

      private static double ToRange(double value, double min, double max)
      {
          if (value < min)
              return min;
          if (value > max)
              return max;
          return value;
      }
  }
}
