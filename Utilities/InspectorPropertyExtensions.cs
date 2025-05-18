#if UNITY_EDITOR
using System;
using Sirenix.OdinInspector.Editor;

#nullable enable

namespace PhenomTools.Utility
{
  public static class InspectorPropertyExtensions
  {
    /// <summary>
    /// Gets a value from the state of an inspector property.
    /// </summary>
    /// <typeparam name="T">The type of value to attempt to get.</typeparam>
    /// <param name="property">The inspector property to read the state of. Cannot be null.</param>
    /// <param name="key">The key to look up in the property's state.</param>
    /// <returns>
    /// Returns default(T) if <paramref name="property"/> does not contain an entry for <see cref="key"/> within its state, or if a state entry
    /// exists, but the value held within that state entry is not assignable to <typeparamref name="T"/>.
    /// </returns>
    public static T? GetState<T>(this InspectorProperty property, string key)
    {
      try
      {
        return property.State.Get<T>(key);
      }
      catch (InvalidOperationException)
      {
        return default;
      }
    }

    /// <summary>
    /// Sets a value onto the state of the provided <paramref name="property"/>.
    /// If the property does not have a state entry for the provided <paramref name="key"/>
    /// then a new state entry will be created and then set to the provided <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="T">The type of value being stored in the property state.</typeparam>
    /// <param name="property">The property whose state is being modified.</param>
    /// <param name="key">The key of the property states to store the value in.</param>
    /// <param name="value">The value to store in the property state.</param>
    /// <exception cref="InvalidOperationException">
    /// If <paramref name="property"/> already contains a state entry for <paramref name="key"/>, but values of type <typeparamref name="T"/> are
    /// not assignable for the property's state entry for that key.
    /// </exception>
    public static void SetState<T>(this InspectorProperty property, string key, T? value)
    {
      if (!property.State.Exists(key))
        property.State.Create<T?>(key, persistent: true, defaultValue: default);
      property.State.Set(key, value);
    }
  }
}

#nullable restore

#endif