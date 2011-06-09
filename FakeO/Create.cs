using System;
using System.Collections.Generic;
using System.Reflection;

namespace FakeO
{
  /// <summary>
  /// This class does the main work for FakeO.
  /// </summary>
  public static class Create
  {
    #region create New object without fake data

    /// <summary>
    /// Returns one new instance of the requested type.
    /// No properties or fields will be populated with fake data.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <returns>One new instance on the requested type.</returns>
    /// <param name="actions">Optional list of actions that should be executed against each created object instance.</param>
    public static T New<T>(params Action<T>[] actions) where T : class
    {
      T obj;
      var constructor = typeof(T).GetConstructor(System.Type.EmptyTypes);
      if (typeof(T).IsValueType || constructor == null)
        obj = default(T);
      else
        obj = Activator.CreateInstance<T>();

      foreach (var action in actions)
        action(obj);

      return obj;
    }

    /// <summary>
    /// Returns a set number of created data objects.
    /// No properties or fields will be populated with fake data.
    /// </summary>
    /// <typeparam name="T">The type to create data.</typeparam>
    /// <param name="count">The number of items to return.</param>
    /// <param name="actions">Optional list of actions that should be executed against each created object instance.</param>
    /// <returns>An enumerable of the requested type.</returns>
    public static IEnumerable<T> New<T>(int count, params Action<T>[] actions) where T : class
    {
      if (count < 0)
        throw new ArgumentOutOfRangeException("'count' must be greater than or equal to 0.");
      for (int i = 0; i < count; i++)
        yield return New<T>(actions);
    }

    /// <summary>
    /// Returns a random number of created data objects.
    /// No properties or fields will be populated with fake data.
    /// </summary>
    /// <typeparam name="T">The type to create data.</typeparam>
    /// <param name="minCount">The minimum number of items to return.</param>
    /// <param name="maxCount">The maximum number of items to return.</param>
    /// <param name="actions">Optional list of actions that should be executed against each created object instance.</param>
    /// <returns>An enumerable of the requested type.</returns>
    public static IEnumerable<T> New<T>(int minCount, int maxCount, params Action<T>[] actions) where T : class
    {
      return New<T>(Number.Next(minCount, maxCount), actions);
    }

    #endregion

    #region create Random objects with fake data

    /// <summary>
    /// Returns one new instance of the requested type.
    /// All setable properties and fields will be populated with fake data.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    /// <param name="actions">Optional list of actions that should be executed against each created object instance.</param>
    /// <returns>One new instance on the requested type.</returns>
    public static T Fake<T>(params Action<T>[] actions) where T : class
    {
      T obj = New<T>();

      FakeTheRest(obj);

      foreach (var action in actions)
        action(obj);

      return obj;
    }

    /// <summary>
    /// Returns a set number of created data objects.
    /// All setable properties and fields will be populated with fake data.
    /// </summary>
    /// <typeparam name="T">The type to create data.</typeparam>
    /// <param name="count">The number of items to return.</param>
    /// <param name="actions">Optional list of actions that should be executed against each created object instance.</param>
    /// <returns>An enumerable of the requested type.</returns>
    public static IEnumerable<T> Fake<T>(int count, params Action<T>[] actions) where T : class
    {
      if (count < 0)
        throw new ArgumentOutOfRangeException("'count' must be greater than or equal to 0.");
      for (int i = 0; i < count; i++)
        yield return Fake<T>(actions);
    }

    /// <summary>
    /// Returns a random number of created data objects.
    /// All setable properties and fields will be populated with fake data.
    /// </summary>
    /// <typeparam name="T">The type to create data.</typeparam>
    /// <param name="minCount">The minimum number of items to return.</param>
    /// <param name="maxCount">The maximum number of items to return.</param>
    /// <param name="actions">Optional list of actions that should be executed against each created object instance.</param>
    /// <returns>An enumerable of the requested type.</returns>
    public static IEnumerable<T> Fake<T>(int minCount, int maxCount, params Action<T>[] actions) where T : class
    {
      return Fake<T>(Number.Next(minCount, maxCount), actions);
    }

    #endregion

    private static void FakeTheRest<T>(T obj) where T : class
    {
      // dump fake data into properties
      foreach (var pi in typeof(T).GetProperties(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (pi.CanWrite && pi.GetSetMethod(true) != null && (pi.GetSetMethod(true).IsPublic || pi.GetSetMethod(true).IsAssembly))
          pi.SetValue(obj, Data.Random(pi.PropertyType), null);
      }
      // dump fake data into fields
      foreach (var fi in typeof(T).GetFields(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (!fi.IsInitOnly && !fi.IsLiteral && (fi.IsPublic || fi.IsAssembly))
          fi.SetValue(obj, Data.Random(fi.FieldType));
      }
    }

  }
}
