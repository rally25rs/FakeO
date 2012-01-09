using System;
using System.Collections.Generic;
using System.Linq;

namespace FakeO
{
	public class FakeCreator
	{
		private readonly Dictionary<string, object> remembered = new Dictionary<string, object>();

		/// <summary>
		/// Returns one new instance of the requested type.
		/// All setable properties and fields will be populated with fake data.
		/// </summary>
		/// <typeparam name="T">The type to return.</typeparam>
		/// <param name="actions">Optional list of actions that should be executed against each created object instance.</param>
		/// <returns>One new instance on the requested type.</returns>
		public T Fake<T>(params Action<T>[] actions) where T : class
		{
			return Create.Fake(GetRememberedActions<T>().Concat(actions).ToArray());
		}

		/// <summary>
		/// Returns a set number of created data objects.
		/// All setable properties and fields will be populated with fake data.
		/// </summary>
		/// <typeparam name="T">The type to create data.</typeparam>
		/// <param name="count">The number of items to return.</param>
		/// <param name="actions">Optional list of actions that should be executed against each created object instance.</param>
		/// <returns>An enumerable of the requested type.</returns>
		public IEnumerable<T> Fake<T>(int count, params Action<T>[] actions) where T : class
		{
			return Create.Fake(count, GetRememberedActions<T>().Concat(actions).ToArray());
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
		public IEnumerable<T> Fake<T>(int minCount, int maxCount, params Action<T>[] actions) where T : class
		{
			return Create.Fake(minCount, maxCount, GetRememberedActions<T>().Concat(actions).ToArray());
		}

		public void Remember<T>(params Action<T>[] actions) where T : class
		{
			var key = typeof(T).AssemblyQualifiedName;
			if (remembered.ContainsKey(key))
				remembered.Remove(key);
			remembered.Add(key, actions);
		}

		/// <summary>
		/// Remove all remembered actions for the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		internal void Forget<T>()
		{
			var key = typeof(T).AssemblyQualifiedName;
			if (remembered.ContainsKey(key))
				remembered.Remove(key);
		}

		/// <summary>
		/// Removes all remembered actions for all types.
		/// </summary>
		internal void ForgetAll()
		{
			remembered.Clear();
		}

		internal Action<T>[] GetRememberedActions<T>()
		{
			var key = typeof(T).AssemblyQualifiedName;
			if (remembered.ContainsKey(key))
				return (Action<T>[])remembered[key];
			return new Action<T>[0];
		}

	}
}
