using System;
/// <summary>
/// Finds a single prefab in the assets directory with a component matching the type of the field carrying this attribute on it's topmost object (the prefab itself)
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class FromPrefabAttribute : UnityEngine.PropertyAttribute { }
