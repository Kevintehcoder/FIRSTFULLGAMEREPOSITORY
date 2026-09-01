using System.Collections;
using UnityEngine;

public class ChaseEnemy_Script : BasicEnemyScript
{
    Animator animator;

    //Coroutine damagedAnimationCoroutine;
    
    public Vector2 dir;

    protected Rigidbody2D rb2d;

    protected virtual void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();

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


    public override void OnDestroy()
    {
        base.OnDestroy();
        /*if (damagedAnimationCoroutine != null)
        {
            StopCoroutine(damagedAnimationCoroutine);
        }
        */
    }

    public override void TakeDamage(float dmg, string damageType)
    {
        //Debug.Log("Starting Animation(Damaged)");
        base.TakeDamage(dmg, damageType);

        /*if (this.gameObject.IsDestroyed() == false)
        {
            damagedAnimationCoroutine = StartCoroutine(StartdamagedAnimation());
        }
        */

    }

    IEnumerator StartdamagedAnimation()
    {
        float originalSpeed = enemyData.Speed;
        animator.SetBool("Damaged", true);

        enemyData.Speed /= 3f;
        yield return new WaitForSeconds(0.4f);

        animator.SetBool("Damaged", false);
        enemyData.Speed = originalSpeed;
    }


}
