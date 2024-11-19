using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Enums

public enum EnumType
{
    TYPE_0,
    TYPE_1, 
    TYPE_2, 
    TYPE_3
}

#endregion

#region Structs

[System.Serializable]
public struct StructExample
{
    #region Variables

    public int health;
    public int mana;
    public int strength;
    public EnumType type;
    public string name;

    #endregion

    #region Methods

    // Getters or Setters
    public void MyMethod()
    {

    }

    #endregion

    #region GettersSetters

    public int GetTotalDamage
    {
        get
        {
            return health * mana * strength;
        }
    }

    #endregion
}

#endregion

public class StructTest : MonoBehaviour
{
    #region Knobs
    
    public StructExample example;

    public StructExample[] examples;

    #endregion
    void Start()
    {
        
    }
}
