using UnityEngine;

public class Dependee : MonoBehaviour
{
    [SerializeField, FromScene] private SceneDependency1 _dependency1;
    [SerializeField, FromScene] private SceneDependency2 _sceneDependency2;
    [SerializeField, FromPrefab] private PrefabDependency _prefabDependency;
}
