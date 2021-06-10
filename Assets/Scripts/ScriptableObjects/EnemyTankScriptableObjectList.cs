using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTankScriptableObjectList", menuName = "ScriptableObjects/NewEnemyTankScriptableObjectList")]
public class EnemyTankScriptableObjectList : ScriptableObject
{
    public EnemyTankScriptableObject[] EnemyTanks;
}

[Serializable]
public class EnemyTankScriptableObject 
{
    public TankColor TankColor;
    public EnemyType EnemyType;
    public string TankName;
    public float Speed;
    public float Health;
    public float Damage;
}
