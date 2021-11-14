using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class DependencyFinder
{
    static DependencyFinder()
    {
        EditorApplication.hierarchyChanged += HierarchyWindowChanged;
    }
    private static void HierarchyWindowChanged()
    {
        var activeScene = SceneManager.GetActiveScene();
        var gameObjects = activeScene.GetRootGameObjects();
        foreach (var go in gameObjects)
        {
            var monoBehaviours = go.GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var monoBehaviour in monoBehaviours)
            {
                foreach (var field in monoBehaviour.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (field.GetCustomAttributes(typeof(FromSceneAttribute), true).FirstOrDefault() is FromSceneAttribute)
                    {
                        InjectFromScene(field, monoBehaviour);
                    }
                    if (field.GetCustomAttributes(typeof(FromPrefabAttribute), true).FirstOrDefault() is FromPrefabAttribute)
                    {
                        InjectFromAssets(field, monoBehaviour);
                    }
                }
            }
        }
    }

    private static void InjectFromAssets(FieldInfo field, MonoBehaviour monoBehaviour)
    {
        if (field.GetValue(monoBehaviour) != null) return;
        var value = FindPrefab(field.FieldType);
        if (value == null)
        {
            LogPrefabMissingInstanceWarning(field, monoBehaviour);
            return;
        }
        field.SetValue(monoBehaviour, value);
    }

    private static void LogPrefabMissingInstanceWarning(FieldInfo field, MonoBehaviour monoBehaviour)
    {
        Debug.LogWarning(
            $"[{nameof(FromPrefabAttribute)}] ({monoBehaviour.gameObject.name}){monoBehaviour.GetType().Name}.{field.Name} couldn't find a prefab of type {field.FieldType.FullName} in the assets folder.");
    }

    private static void LogSceneMissingInstanceWarning(FieldInfo field, MonoBehaviour monoBehaviour)
    {
        Debug.LogWarning(
            $"[{nameof(FromSceneAttribute)}] ({monoBehaviour.gameObject.name}){monoBehaviour.GetType().Name}.{field.Name} couldn't find a game object of type {field.FieldType.FullName} in the scene");
    }

    private static void InjectFromScene(FieldInfo field, MonoBehaviour monoBehaviour)
    {
        if (field.GetValue(monoBehaviour) != null) return;
        // inject from the gameobject if the desired object is present there first
        var firstFind = monoBehaviour.GetComponent(field.FieldType);
        // if not present on "self" get by searching entire hierarchy
        if (firstFind == null)
        {
            var value = FindTypeInChildren(field.FieldType);
            firstFind = value.FirstOrDefault();
        }

        if (firstFind == null)
        {
            LogSceneMissingInstanceWarning(field, monoBehaviour);
            return;
        }

        field.SetValue(monoBehaviour, firstFind);
    }

    public static List<Component> FindTypeInChildren(Type type)
    {
        var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        var result = new List<Component>();
        foreach (var gameObject in rootObjects)
        {
            var components = gameObject.GetComponentsInChildren(type, true);
            result.AddRange(components);
        }

        return result;
    }

    public static Component FindPrefab(Type t)
    {
        var assetIds = AssetDatabase.FindAssets("t:prefab");
        foreach (var id in assetIds)
        {
            var pathToAsset = AssetDatabase.GUIDToAssetPath(id);
            var asset = AssetDatabase.LoadAssetAtPath(pathToAsset, t);
            if (asset != null)
                return asset as Component;
        }

        return null;
    }
}