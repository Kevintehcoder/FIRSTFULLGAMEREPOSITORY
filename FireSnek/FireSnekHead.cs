using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSnekHead : BaseBossScript
{
    [SerializeField] public Transform Terminalpoint;
    [SerializeField] GameObject Rings;

    Coroutine AttackCoroutine;
    Coroutine BasicAttackCoroutine;
    Coroutine HalfHealthAttackCoroutine;

    [SerializeField] int ShootAngle;
    bool canMove = true;
    bool Allow_headFollow = true;

    public SpawnObject explosionBullet;
    public Action<bool, Vector2, float> SpeedChanges;
    public Action<bool> EnableSegments;

    BossDecisions Attacklogic = new();

    Queue<BossAttacks> pendingAttacks = new Queue<BossAttacks>();

    private void OnEnable()
    {
        EnableSegments?.Invoke(true);
    }

    private void OnDisable()
    {
        EnableSegments?.Invoke(false);
    }

    protected override void Start()
    {
        base.Start();

    }

    protected override void Update()
    {
        base.Update();

        AttackStates();

    }


    protected override void FixedUpdate()
    {
        if (canMove)
        {
            base.FixedUpdate();
        }
        else
        {
            rb2d.linearVelocity = Vector2.zero;
        }

        if (Allow_headFollow)
        {
            rotateHead();
        }
    }

    void rotateHead()
    {
        Vector2 Rotdir = (Vector2)focus.position - rb2d.position;
        Vector2 Curretndir = (Vector2)Terminalpoint.transform.position - rb2d.position;

        float angle = Mathf.Atan2(Rotdir.y, Rotdir.x) * Mathf.Rad2Deg + 180;

        rb2d.MoveRotation(angle);
    }

    #region MainAttacks
    protected override void AttackONE()
    {
        rb2d.linearVelocity = Vector2.zero;
        canMove = false;

        if (AttackCoroutine == null)
        {
            AttackCoroutine = StartCoroutine(ShootBullets());
        }
    }

    IEnumerator ShootBullets()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.5f);

        for (int i = 0; i <= 180; i += ShootAngle)
        {
            GameObject bullet = ObjectPoolManager.instance.GetObject(Object.FireBall);

            Vector2 posChange = new Vector2(Mathf.Cos(i * Mathf.Deg2Rad), Mathf.Sin(i * Mathf.Deg2Rad));

            BulletScript bulletscript = bullet.GetComponent<BulletScript>();
            bulletscript.dir = Vlogic.FindDirection(transform.position, focus.transform.position);
            

            bullet.transform.position = (Vector2)gameObject.transform.position + (posChange * 1.5f);
            bullet.SetActive(true);


            yield return waitTime;

            
        }

        yield return new WaitForSeconds(2);
        AttackCoroutine = null;
        canMove = true;
    }


    protected override void AttackTWO()
    {
        if (AttackCoroutine == null)
        {
            AttackCoroutine = StartCoroutine(SpawnBullets());
        }   
    }
    public IEnumerator SpawnBullets()
    {
        for (int i = 0; i <= 3; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                explosionBullet.Spawn();

            }

            yield return new WaitForSeconds(2f);
        }

        AttackCoroutine = null;
    }


    protected override void AttackTHREE()
    {
        if (AttackCoroutine == null)
        {
            AttackCoroutine = StartCoroutine(SpeedUp(30, 1.5f));
        }
    }
    IEnumerator SpeedUp(float time, float percentageIncrease)
    {
        enemyData.Speed *= percentageIncrease;
        

        yield return StartCoroutine(Charge(132f, time));

        enemyData.Speed /= percentageIncrease;
        SpeedChanges?.Invoke(false, Vector2.zero, 0);
        AttackCoroutine = null;
    }
    IEnumerator Charge(float Force, float totalTIme)
    {
        float timer = 0;

        float dashingTime = 0.8f;
        float followTime = 2.1f;
        float backupTime = 0.7f;

        WaitForSeconds dashingSec = new WaitForSeconds(dashingTime);
        WaitForSeconds followSec = new WaitForSeconds(followTime);
        WaitForSeconds backupSec = new WaitForSeconds(backupTime);

        while (timer < totalTIme)
        {
            canMove = false;
            Allow_headFollow = false;
            rb2d.freezeRotation = true;

            Vector2 dashDir = Vlogic.FindDirection(transform.position, focus.position);
            rb2d.AddForce(-dashDir * Force, ForceMode2D.Impulse);
            yield return backupSec;


            rb2d.AddForce(dashDir * Force, ForceMode2D.Impulse);
            SpeedChanges?.Invoke(true, dashDir, Force);


            yield return dashingSec;
            canMove = true;
            rb2d.freezeRotation = false;
            Allow_headFollow = true;

            yield return followSec; 
            timer += dashingTime + followTime + backupTime;
        }
        
    }
    #endregion

    #region Unique Attacks
    protected override void HalfHealthAttack()
    {
        if(HalfHealthAttackCoroutine == null)
        {
            HalfHealthAttackCoroutine = StartCoroutine(HalfHealthAttackRoutine());
        }
    }
    private IEnumerator HalfHealthAttackRoutine()
    {
        for (int i = 0; i < 3; i++)
        {
            halfHPAttack();
            yield return new WaitForSeconds(1.5f);
            halfHPAttack();

            yield return new WaitForSeconds(4f);
        }

        HalfHealthAttackCoroutine = null;
    }

    void halfHPAttack()
    {
        for (int i = 0; i < 4; i++)
        {
            int randomIntX = UnityEngine.Random.Range(-40, 40);
            float randomIntY = UnityEngine.Random.Range(-40f, 40f);
            GameObject ExpandingRingObj = ObjectPoolManager.instance.GetObject(Object.ExpandingRing);
            ExpandingRingObj.transform.position = new Vector2(transform.position.x + randomIntX, transform.position.y + randomIntY);
            ExpandingRingObj.GetComponent<SpreadingRing>().Initialize(3f, 2f, 10f, true, 15.75f);
            ExpandingRingObj.SetActive(true);
        }
    }


    protected override void SpecialAttack()
    {
        StartCoroutine(spawnRings());
    }

    IEnumerator spawnRings()
    {
        Rings.SetActive(true);
        yield return new WaitForSeconds(20f);
        Rings.SetActive(false);

    }
   

    protected override void DeathAttack()
    {
        for (int i = 0; i < 3; i++)
        {
            int randomIntX = UnityEngine.Random.Range(-22, 22);
            float randomIntY = UnityEngine.Random.Range(-5f, 5f);
            GameObject ExpandingRing = ObjectPoolManager.instance.GetObject(Object.ExpandingRing);
            ExpandingRing.transform.position = new Vector2(transform.position.x + randomIntX, transform.position.y + randomIntY);
            ExpandingRing.GetComponent<SpreadingRing>().Initialize(30f, 8f, 15f, true, 12);
            ExpandingRing.SetActive(true);
        }

    }
    #endregion


    protected override void DisableAllAttacks()
    {


    }

    public void HPStates()
    {
        float percent = (float)(enemyData.currentHp / enemyData.MaxHp);
        BossAttacks HpAttack = Attacklogic.GetNextAttack(percent);

        if (HpAttack != BossAttacks.Idle)
        {
            pendingAttacks.Enqueue(HpAttack);

            if (BasicAttackCoroutine != null)
            {
                StopCoroutine(BasicAttackCoroutine);
                BasicAttackCoroutine = null;
            }
        }

        if (BasicAttackCoroutine == null)
        {
            BasicAttackCoroutine = StartCoroutine(BasicAttack());
        }
    }

    IEnumerator BasicAttack()
    {
        if (pendingAttacks.Count > 0)
        {
            currentAttack = pendingAttacks.Dequeue();
        }

        yield return null;

        if (currentAttack == BossAttacks.Idle)
        {
            int number = UnityEngine.Random.Range(5, 9);

            if (number == 5)
            {
                currentAttack = BossAttacks.Attack2;
            }
            else if(number == 8)
            {       
                currentAttack= BossAttacks.Attack1;
            }

            yield return new WaitForSeconds(3f);

            BasicAttackCoroutine = null;
        }
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
                currentAttack = BossAttacks.Idle;
                break;

            case BossAttacks.Attack2:
                AttackTWO();
                currentAttack = BossAttacks.Idle;
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

}
