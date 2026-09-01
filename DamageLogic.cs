using UnityEngine;

[CreateAssetMenu(fileName = "DamageLogic", menuName = "Scriptable Objects/DamageLogic")]
public class DamageLogic : ScriptableObject
{
    public float CalcDamage(float dmg, string damageType = "none")
    {

        if (damageType != "status")
        {
            if (damageType == "fire")
            {
                dmg = dmg * 1.1f;
            }
            else if (damageType == "lazar")
            {
                dmg = dmg * 1.3f;
            }

            float randomNumber = Random.Range(1, 50);

            if (randomNumber == 1)
            {
                Debug.Log($"Crit, DMG: {dmg * 2}");
                return dmg * 2;
            }
            //Crit damage logic
        }



        return dmg;
    }


    
}
