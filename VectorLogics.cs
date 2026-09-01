using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "VectorLogics", menuName = "Scriptable Objects/VectorLogics")]
public class VectorLogics : ScriptableObject
{
    public Vector2 FindDirection(Vector2 Startpos,Vector2 Endpos)
    {
        Vector2 dir = (Endpos - Startpos).normalized;

        return dir;
    }

}
