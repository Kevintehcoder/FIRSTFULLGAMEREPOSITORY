
using UnityEngine;
using System.Collections;

public class BasicEnemyScript : CullableObject, IDamageable, IDropCoins, Iexploded
{
    [Header("Enemy Data")]
    [SerializeField] public  BasicObjectStats enemyData;
    [SerializeField] public  Damage_Data DMG_Data;
    public int coins;

    public RoomScript room;


    [Header("Important logics")]
    [SerializeField] protected VectorLogics Vlogic;

    [SerializeField] protected DamageLogic Dlogic;

    protected bool exploded = false;
    protected Transform focus;

    [SerializeField] protected LayerMask ObjectstoDamage;


    protected virtual float CurrentHP
    {
        get
        {
            return enemyData.currentHp;
        }
        set
        {

            enemyData.currentHp = value;

            if (enemyData.currentHp <= 0)
            {
 
                room.RemoveEnemy(gameObject);

                Destroy(gameObject.transform.parent.gameObject);

                DropCoins(coins);
            }
            

        }
    }


    protected override void Start()
    {
        base.Start();
        focus = GameObject.FindGameObjectWithTag("Player").transform;
        CurrentHP = enemyData.MaxHp;

        room = GetComponentInParent<RoomScript>();

    }


    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable hitTarget = collision.gameObject.GetComponent<IDamageable>();

        if (hitTarget != null && ((1 << collision.gameObject.layer) & ObjectstoDamage) != 0)
        {
            hitTarget.TakeDamage(DMG_Data.dmgAmount, DMG_Data.dmgTypeName);

        }
    }
     
    public virtual void TakeDamage(float dmg, string damageType)
    {
        float totaldmg = Dlogic.CalcDamage(dmg, damageType);

        CurrentHP -= totaldmg;
    }

    public void DropCoins(int coins)
    {
        PlayerShopData.coins += coins;
    }

    public void Explode()
    {
        exploded = true;

        StartCoroutine(Exploded());
    }
    IEnumerator Exploded()
    {
        yield return new WaitForSeconds(0.5f);
        exploded = false;
    }
}
