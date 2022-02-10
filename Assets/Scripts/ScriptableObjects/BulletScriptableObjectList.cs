using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletScriptableObjectList", menuName = "ScriptableObjects/NewBulletScriptableObjectList")]
public class BulletScriptableObjectList : ScriptableObject
{
    public BulletScriptableObject[] Bullets;
}

[Serializable]  
public class BulletScriptableObject
{
    public BulletType BulletType;
    public string BulletName;
    public float BulletForce;
    public float Damage;
}