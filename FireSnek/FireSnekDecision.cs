using System.Collections.Generic;
using UnityEngine;

public class FireSnekDecision
{
    HashSet<float> triggeredThreshholds = new();
    float[] threshHolds = { 0.6f, 0.4f, 0.3f };

    bool halfHealthDone = false;
    bool specialDone = false;

    public BaseBossScript.BossAttacks GetNextAttack(float hpPercent)
    {
        if (hpPercent <= 0.5f && halfHealthDone == false)
        {
            halfHealthDone = true;
            return BaseBossScript.BossAttacks.HalfHealthAttack;
        }
        if (hpPercent <= 0.2f && specialDone == false)
        {
            specialDone = true;
            return BaseBossScript.BossAttacks.SpecialAttack;
        }

        foreach (float t in threshHolds)
        {
            if (hpPercent <= t && !triggeredThreshholds.Contains(t))
            {
                triggeredThreshholds.Add(t);
                return BaseBossScript.BossAttacks.Attack3;
            }
        }
        return BaseBossScript.BossAttacks.Idle; //No Priority attack needs to be done
    }

}
