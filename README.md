# Dependency Finder for Unity

A simple dependency injection tool for unity. It works by automating the manual work of referencing objects in the hierarchy. Therefore it can run at editor runtime and will not affect runtime performance of your game.

When fields get accidentally unreferenced they will automatically be re-referenced. Another great aspect is that when adding a new field to an already existing component there is no need to commit the scene it's on since the reference will automatically be picked up by the other developer's editor.

## Code example
```csharp
[SerializeField, FromScene] private SceneDependency1 _dependency1;
[SerializeField, FromScene] private SceneDependency2 _sceneDependency2;
[SerializeField, FromPrefab] private PrefabDependency _prefabDependency;
```

## The logic of the available attributes

### FromSceneAttribute
When using the *FromScene* attribute DependencyFinder will (in the following order):
1. Look at the object itself to see if it has a component of the required type, if so use that.
2. Try to find the first component of the required type anywhere in the scene.

### FromPrefabAttribute
When using the *FromPrefab* attribute DependencyFinder will look in the entire assets directory to see if there is a prefab that has the required component on it's topmost object if found it will be referenced.

## How it works
It makes use of the [EditorApplication.hierarchyChanged](https://docs.unity3d.com/ScriptReference/EditorApplication-hierarchyChanged.html)-event to detect changes. Mainly this means that every time you load a scene, change an object in the hierarchy or save the scene DependencyFinder will find any missing references and set them. 

## How to install
Checkout this project and copy the folder called DependencyFinder to anywhere inside your Assets folder. A good practice is to put it in the Plugins-folder.

## When to use
For small to medium sized projects, makes the greatest impact for a project when working in a small team.

## Limitations
Does not work for scene references for objects that are added to the scene during runtime since DependencyFinder runs in the editor.
