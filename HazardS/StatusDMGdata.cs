using UnityEngine;

[CreateAssetMenu(fileName = "StausDMGdata", menuName = "Scriptable Objects/StausDMGdata")]
public class StatusDMGdata : ScriptableObject
{
    public float FireDMG;
    public float PoisonDMG = 10;
    public float FrozenDMG;
    public float MuteDMG;
}
