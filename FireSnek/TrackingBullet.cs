using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingBullet : BulletScript
{
    [SerializeField] VectorLogics Vlogic;
    Transform focus;
    [SerializeField] string tagName = "Player";
    [SerializeField] MiscLogicManager miscLogic;

    float timer = 0;
    float FollowTime = 5;
    [SerializeField] float ExplosionRadius = 10f;

    public float startPos;

    Collider2D[] colliders;
    SpriteRenderer[] Spriterenderers;

    bool canFollow = true;

    protected override void OnEnable()
    {
        colliders= GetComponentsInChildren<Collider2D>();
        Spriterenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (Collider2D collider in colliders)
        {
            collider.enabled = true;
        }

        foreach (SpriteRenderer Spriterenderer in Spriterenderers)
        {
            Spriterenderer.enabled = true;
        }

        focus = GameObject.FindGameObjectWithTag(tagName).transform;

        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }


    protected override void FixedUpdate()
    {
        if (timer < FollowTime && canFollow)
        {
            dir = Vlogic.FindDirection(transform.position, focus.position);

            float targetangle = 180 + Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            float smoothAngle = Mathf.MoveTowardsAngle(rb2d.rotation, targetangle, 10000 * Time.fixedDeltaTime);

            rb2d.MoveRotation(smoothAngle);

            timer += Time.fixedDeltaTime;
        }

        base.FixedUpdate();
    }

    public override void onHitObject()
    {
        RaycastHit2D[] explosionObj = Physics2D.CircleCastAll(transform.position, ExplosionRadius, Vector2.zero);
        foreach (RaycastHit2D hit in explosionObj)
        {
            Rigidbody2D rb = hit.collider.GetComponent<Rigidbody2D>();

            float distance = (hit.collider.transform.position - transform.position).magnitude;

            if (rb != null && distance < ExplosionRadius)
            {
                Iexploded iexploded = hit.collider.GetComponent<Iexploded>();
                if (iexploded != null)
                {
                    Vector3 explosiondir = (hit.collider.transform.position - transform.position).normalized;
                    float Force = 70 * (1 - (distance / ExplosionRadius));

                    iexploded.Explode();
                    rb.AddForce(explosiondir * Force, ForceMode2D.Impulse);
                    Debug.Log($"Explosion force applied to {hit.collider.name}");

                }
                
            }

        }


        ExplosionParticles();

        Invoke(nameof(DisableObject), 3f);

    }


    private void ExplosionParticles()
    {
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
        foreach (SpriteRenderer Spriterenderer in Spriterenderers)
        {
            Spriterenderer.enabled = false;
        }

        ParticleSystem ps = GetComponent<ParticleSystem>();

        ps.Play();
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.startLifetime = 1f;
        ps.Emit(emitParams, 3);


        canFollow = false;
        dir = Vector3.zero;

    }


    private void DisableObject()
    {
        base.onHitObject(); 
    }


}
