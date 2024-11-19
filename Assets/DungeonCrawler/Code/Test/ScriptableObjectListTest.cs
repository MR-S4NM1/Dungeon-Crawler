using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object Test/New SOT List")]
public class ScriptableObjectListTest : ScriptableObject
{
    [SerializeField] public ScriptableObjectTest[] listOfScriptableObjects;
}
