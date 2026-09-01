using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LazarAttack : MonoBehaviour
{
    LineRenderer lineRenderer;
    TargetPlayer targetPlayerScript;
    [SerializeField] VectorLogics Vlogic;

    [SerializeField] LayerMask ObjectstoDamage;

    public Coroutine lazarCoroutine;
    Coroutine CoolDownCoroutine;

    [SerializeField] Damage_Data DMG_data;

    WaitForSeconds lazarDelayTime = new(0.1f);
    WaitForSeconds lazarWaitDuration = new(0.7f);


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;

        if (GetComponent<TargetPlayer>() != null)
        {
            targetPlayerScript = GetComponent<TargetPlayer>();
        }
    }


    public void StartLazar()
    {
        if (lazarCoroutine == null)
        {
            lazarCoroutine = StartCoroutine(ShootLazar());
        }
    }

    public void StopLazar()
    {
        if (lazarCoroutine != null)
        {
            StopCoroutine(lazarCoroutine);
            lazarCoroutine = null;
        }


        targetPlayerScript.AllowedtoFollow = true;
        lineRenderer.enabled = false;
    }

    #region Lazar Drawing
    void DrawLazar()
    {
        lineRenderer.enabled = true;

        Vector2 dir = Vlogic.FindDirection(transform.position, targetPlayerScript.Terminalpoint.position);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, dir * 100 + (Vector2)transform.position);

        DrawLazarHitBox(dir);
    }

    void DrawLazarHitBox(Vector2 dir)
    {
        RaycastHit2D hitobject = Physics2D.BoxCast(transform.position, new Vector2(0.5f, 0.5f), 0, dir, 100, ObjectstoDamage);

        Debug.DrawRay(transform.position, dir * 100f, Color.red);

        if (hitobject == true)
        {
            if (CoolDownCoroutine == null)
            {
                CoolDownCoroutine = StartCoroutine(DamageCooldown(hitobject));
            }
        }

    }
    #endregion

    IEnumerator ShootLazar()
    {
        yield return new WaitForSeconds(1.5f);
        while (true)
        {
            targetPlayerScript.AllowedtoFollow = false;
            yield return lazarDelayTime;

            float timer = 0;
            float timerDuration = 0.8f;

            while (timer < timerDuration)
            {
                DrawLazar();
                timer += Time.deltaTime;
                yield return null;
            }
            
            lineRenderer.enabled = false;
            targetPlayerScript.AllowedtoFollow = true;
            yield return lazarWaitDuration;

        }

    }

    IEnumerator DamageCooldown(RaycastHit2D hitobject)
    {
        IDamageable damageable = hitobject.collider.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(DMG_data.dmgAmount, DMG_data.dmgTypeName);
            
        }

        yield return new WaitForSeconds(0.5f);
        CoolDownCoroutine = null;
    }
}
