using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBoss_Script : BaseBossScript
{
    [Header("Attack Settings")]
    [SerializeField] LazarAttack[] lazarAttacks;
    [SerializeField] SpawnEnemy[] MinionSpawners;
    [SerializeField] GameObject SporeBullet;
    [SerializeField] GameObject EyeTurrets;
    [SerializeField] GameObject SlugBall;

    BossDecisions Attacklogic = new();

    Coroutine BasicAttackCoroutine;
    Coroutine AttackTwoCoroutine;
    bool lazarAttackDone = false;

    float attackspeed = 2;

    Queue<BossAttacks> pendingAttacks = new Queue<BossAttacks>();


    protected override void Start()
    {
        base.Start();

        lazarAttacks = GetComponentsInChildren<LazarAttack>();
        MinionSpawners = GetComponentsInChildren<SpawnEnemy>();
        room = GetComponentInParent<RoomScript>();

    }

    protected override void Update()
    {
        base.Update();

        AttackStates();

    }

    #region MainAttacks
    protected override void AttackONE()
    {
        if (lazarAttackDone == false)
        {
            lazarAttackDone = true;
            foreach (LazarAttack attack in lazarAttacks)
            {
                attack.StartLazar();
            }
        }
    }

    protected override void AttackTWO()
    {
        if (AttackTwoCoroutine == null)
        {
            AttackTwoCoroutine = StartCoroutine(ShootBullets());
        }            
    }
    IEnumerator ShootBullets()
    {
        WaitForSeconds wait = new WaitForSeconds(3f);
        WaitForSeconds bulletDelay = new WaitForSeconds(0.3f);
        while (currentAttack == BossAttacks.Attack2)
        {
            for (int i = 0; i < attackspeed; i++)
            {
                GameObject bullet = ObjectPoolManager.instance.GetObject(Object.SporeBullet);

                bullet.GetComponent<BulletScript>().dir = Vlogic.FindDirection(transform.position, focus.transform.position);
                bullet.transform.position = gameObject.transform.position;
                bullet.SetActive(true);

                yield return bulletDelay;
            }
            yield return wait;
        }

        yield return wait;
        AttackTwoCoroutine = null;
    }

    protected override void AttackTHREE()
    {
        for (int i = 0; i < 8; i++)
        { 
            float RandomNumberX = Random.Range(-20, 20);
            float RandomNumberY = Random.Range(-20, 20);
            Vector2 tempVect = new Vector2(RandomNumberX, RandomNumberY);

            Instantiate(EyeTurrets, (Vector2)transform.position + tempVect, Quaternion.identity);

        }
    }
    #endregion

    #region Unique Attacks
    protected override void HalfHealthAttack()
    {
        attackspeed = attackspeed * 2;
        DMG_Data.dmgAmount *= 1.5f;

        StartCoroutine(SpawningTimer(MinionSpawners));
        
    }
    IEnumerator SpawningTimer(SpawnEnemy[] spawners)
    {
        for (int i = 0; i < 3; i++)
        {
            foreach (SpawnEnemy spawner in spawners)
            {
                spawner.SpawnObject(room.transform);
            }
            yield return new WaitForSeconds(2);
        }

    }

    protected override void SpecialAttack()
    {
        DMG_Data.dmgAmount *= 1.5f;
        attackspeed = attackspeed * 1.5f;
    }

    protected override void DeathAttack()
    {
        for (int i = 0; i < 360; i += 18)
        {
            float rad = i * Mathf.Deg2Rad;
            Vector3 tempVec = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            Vector2 dir = tempVec.normalized;
            GameObject ball = Instantiate(SlugBall, transform.position, Quaternion.identity);

            Rigidbody2D ballRb2d = ball.GetComponent<Rigidbody2D>();

            ballRb2d.AddForce(dir * 100, ForceMode2D.Impulse);

        }
    }
    #endregion

    protected override void DisableAllAttacks()
    { 
        foreach(LazarAttack attack in lazarAttacks)
        {
            attack.StopLazar();
        }

        lazarAttackDone = false;

        if (AttackTwoCoroutine != null)
        {
            StopCoroutine(AttackTwoCoroutine);
            AttackTwoCoroutine = null;
        }
    }

    #region StateMachine
    void HPStates()
    {
        float hpPercent = enemyData.currentHp / enemyData.MaxHp;
        BossAttacks decision = Attacklogic.GetNextAttack(hpPercent);

        if (decision != BossAttacks.Idle)
        {
            pendingAttacks.Enqueue(decision);

            // If a priority attack is ready stop and set
            // the attack coroutine to null to run it again
            if (BasicAttackCoroutine != null)
            {
                StopCoroutine(BasicAttackCoroutine);
                BasicAttackCoroutine = null;
            }
        }

        if (BasicAttackCoroutine == null)
        {
            BasicAttackCoroutine = StartCoroutine(BasicAttacks());
        }

    }
    IEnumerator BasicAttacks()
    {
        //Check if Priority Attacks exist
        if (pendingAttacks.Count > 0)
        {
            currentAttack = pendingAttacks.Dequeue();
        }

        yield return null;
        
        //If no priority moves then do basic moves
        //Lazar Attack = 25%
        //Spore Attack = 75%
        if (currentAttack == BossAttacks.Idle)
        {
            int attackIndex = Random.Range(1, 20);

            if (attackIndex <= 14)
            {
                currentAttack = BossAttacks.Attack2;
            }
            else
            {
                currentAttack = BossAttacks.Attack1;
 
            }
        }
        
        //If attack is not lazar, stop all lazarscripts
        if (currentAttack != BossAttacks.Attack1)
        {
            foreach (LazarAttack attack in lazarAttacks)
            {
                attack.StopLazar();
            }
            lazarAttackDone = false;
        }

        yield return new WaitForSeconds(10f);

        BasicAttackCoroutine = null;
    }
    void AttackStates()
    {
        HPStates();


        switch (currentAttack)
        {
            case BossAttacks.Idle:
                DisableAllAttacks();
                break;

            case BossAttacks.Attack1:
                AttackONE();
                break;

            case BossAttacks.Attack2:
                AttackTWO();
                break;

            case BossAttacks.Attack3:
                AttackTHREE();
                currentAttack = BossAttacks.Idle;
                break;

            case BossAttacks.HalfHealthAttack:
                HalfHealthAttack();
                currentAttack = BossAttacks.Idle;
                break;

            case BossAttacks.SpecialAttack:
                SpecialAttack();
                currentAttack = BossAttacks.Idle;
                break;

            case BossAttacks.DeathAttack:
                DeathAttack();
                currentAttack = BossAttacks.Idle;
                break;
        }
    }
    #endregion
}
