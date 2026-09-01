using UnityEngine;
using System.Collections;

public abstract class BaseBossScript : BasicEnemyScript
{
    protected Vector2 dir;

    [SerializeField] protected BossAttacks currentAttack;
    [SerializeField] protected ShopData shopStatus;

    protected Rigidbody2D rb2d;
    protected override float CurrentHP 
    { 
        get => base.CurrentHP;
        set 
        {
            base.CurrentHP = value;


        }
    }
    public enum BossAttacks
    {
        Idle,
        Attack1,
        Attack2,
        Attack3,
        HalfHealthAttack,
        SpecialAttack,
        DeathAttack,
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        DeathAttack();

        shopStatus.SetShopStates(true);
        
    }
    protected virtual void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        
    }

    protected virtual void Update()
    {
        dir = Vlogic.FindDirection(transform.position, focus.transform.position);

    }
    protected virtual void FixedUpdate()
    {
        if (!exploded)
        {
            rb2d.linearVelocity = dir * enemyData.Speed;
        }
    }

    protected abstract void AttackONE();

    protected abstract void AttackTWO();

    protected abstract void AttackTHREE();

    protected abstract void HalfHealthAttack();

    protected abstract void SpecialAttack();

    protected abstract void DeathAttack();

    protected abstract void DisableAllAttacks();


}
