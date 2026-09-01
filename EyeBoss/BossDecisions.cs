using System.Collections.Generic;

public class BossDecisions
{
    HashSet<float> triggeredThresholds = new HashSet<float>(); // to keep track of which attack 3 thresholds have been triggered         
    float[] thresholds = { 0.6f, 0.4f, 0.3f }; // hp thresholds for attack 3

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

        foreach (float t in thresholds)
        {
            if (hpPercent <= t && !triggeredThresholds.Contains(t))
            {
                triggeredThresholds.Add(t);
                return BaseBossScript.BossAttacks.Attack3;
            }
        }

        return BaseBossScript.BossAttacks.Idle; //No Priority attack needs to be done
    }
}
