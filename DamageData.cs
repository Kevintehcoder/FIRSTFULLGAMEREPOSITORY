using UnityEngine;

[CreateAssetMenu(fileName = "DamageData", menuName = "Scriptable Objects/DamageData")]
public class DamageData : ScriptableObject
{
    public string dmgTypeName;
    public float dmgAmount;
}
