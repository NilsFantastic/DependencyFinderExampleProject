using System;

/// <summary>
/// Finds a single component in the scene with the script matching the type of the field carrying this attribute.
/// It will first look on the component itself, if not found there it will go on and look in the entire hierarchy
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class FromSceneAttribute : UnityEngine.PropertyAttribute { }
