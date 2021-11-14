using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceToSelfDependee : MonoBehaviour
{
    [SerializeField, FromScene] ReferenceToSelfDependency dependency;
}
