using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DashingEnemy_Script : ChaseEnemy_Script
{
    bool Ramming = false;

    public float DashForce;

    [Header("Sprites")]
    [SerializeField] GameObject Horn1;
    [SerializeField] GameObject Horn2;

    Coroutine dashingCoroutine;

    protected override void Update()
    {
        base.Update();

        if (CurrentHP <= 80 && Horn1.IsDestroyed() == false)
        {
            triggerDashing(Horn1);
        }

        if (CurrentHP <= 60 && Horn2.IsDestroyed() == false)
        {
            triggerDashing(Horn2);
        }



        if (Horn1.IsDestroyed() == true && Horn2.IsDestroyed() == true)
        {
            if (dashingCoroutine == null)
            {
                dashingCoroutine = StartCoroutine(ContinuousDashing());
            }
        }

    }

    protected override void FixedUpdate()
    {
        if (Ramming == false)
        {
            base.FixedUpdate();
        }
    }

    void triggerDashing(GameObject horn)
    {
        Ramming = true;
        Destroy(horn);
        StartCoroutine(StartDashing());
    }

    IEnumerator StartDashing()
    {
        Debug.Log("Enemy Dashed");
        rb2d.AddForce(dir * DashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.4f);
        Ramming = false;
    }

    IEnumerator ContinuousDashing()
    {
        enemyData.Speed *= 7.75f / enemyData.Speed;
        while (true)
        {
            Ramming = true;
            StartCoroutine(StartDashing());
            yield return new WaitForSeconds(0.8f);
        }
    }
}
