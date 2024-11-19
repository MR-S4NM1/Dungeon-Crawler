using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Structs

[System.Serializable]
public struct CharacterProperties
{
    [Header("Base Definition")]
    [SerializeField] public string name;
    [SerializeField] public int currentHealth;
    [SerializeField] public int maxHealth;
    [SerializeField] public Vector2 spawnPosition;
    [Header("References Assets")]
    [SerializeField] public Sprite characterSprite;
    [SerializeField] public GameObject prefabAgent;
}

#endregion

[CreateAssetMenu(menuName = "Scriptable Object Test/New SOT")]
public class ScriptableObjectTest : ScriptableObject
{
    [SerializeField] CharacterProperties characterProperties;
}
