using System;

namespace FakeO
{
  /// <summary>
  /// Provide access to random number geenrator.
  /// </summary>
  public static class Number
  {
    private static readonly System.Random _rnd = new System.Random();

    public static int Next()
    {
      return _rnd.Next();
    }

    public static int Next(int max)
    {
      return _rnd.Next(max);
    }

    public static int Next(int min, int max)
    {
      return _rnd.Next(min, max);
    }

    public static float NextFloat()
    {
      double mantissa = (_rnd.NextDouble() * 2.0) - 1.0;
      double exponent = Math.Pow(2.0, _rnd.Next(-126, 128));
      return (float)(mantissa * exponent);
    }

      public static float NextFloat(float min, float max)
      {
          return (float)_rnd.NextDouble() * (max - min) + min;
      }

    public static double NextDouble()
    {
      return NextDouble(double.MinValue, double.MaxValue);
    }

    public static double NextDouble(double min, double max)
    {
      return _rnd.NextDouble() * (max - min) + min;
    }

  }
}
