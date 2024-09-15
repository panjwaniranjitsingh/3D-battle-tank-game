using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TankScriptableObjectList", menuName = "ScriptableObjects/NewTankScriptableObjectList")]
public class TankScriptableObjectList : ScriptableObject
{
    public TankScriptableObject[] Tanks;
}

[Serializable]
public class TankScriptableObject
{
    public TankColor TankColor;
    public string TankName;
    public float Speed;
    public float Health;
    public float Damage;
}