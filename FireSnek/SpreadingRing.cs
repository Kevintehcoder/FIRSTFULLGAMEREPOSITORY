using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadingRing : MonoBehaviour
{
    Coroutine LifeCoroutine;

    LineRenderer lineRenderer;
    float radius = 1f;
    float segments = 30;

    float lineWidth;
    float DMG = 10;
    float LifeTime = 0;
    float speed = 15.75f;

    public bool startBool = false;

    Dictionary<IDamageable, float> damageDictionary = new Dictionary<IDamageable, float>();
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = (int)segments + 1;

        lineWidth = lineRenderer.startWidth/ 2;

    }

    private void OnEnable()
    {
        damageDictionary.Clear();

        if (LifeCoroutine == null && startBool)
        {
            LifeCoroutine = StartCoroutine(KillSelf());
        }
    }

    private void OnDisable()
    {
        radius = 1f;
        startBool = false;
    }

    // Update is called once per frame
    void Update()
    {
        DrawCircle();
        DodamageObject();

        radius += Time.deltaTime * speed;
    }

    private void OnDrawGizmos()
    {
        if (lineRenderer != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius - lineWidth);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, radius + lineWidth);
        }
    }

    public void Initialize(float lineWidth, float dmg, float lifeTimez, bool boolean, float Speed)
    {
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        DMG = dmg;
        LifeTime = lifeTimez;
        startBool = boolean;
        speed = Speed;
    }

    private void DrawCircle()
    {

        for (float i = 0; i <= segments; i ++)
        {
            float angle = i * (360f / segments);

            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

            Vector2 VectorShifted = (Vector2)transform.position + new Vector2(x, y);
            lineRenderer.SetPosition((int)i, VectorShifted);
        }

    }

    IEnumerator KillSelf()
    {
        yield return new WaitForSeconds(LifeTime);
        LifeCoroutine = null;

        gameObject.SetActive(false);


    }

    void DodamageObject()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius + lineWidth, Vector2.zero);

        foreach (var hit in hits)
        {
            if (Vector2.Distance(transform.position, hit.collider.gameObject.transform.position) > radius - lineWidth)
            {
                if (hit.collider.gameObject.layer == 10) return;

                if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
                {
                    if (damageDictionary.ContainsKey(damageable) == false || Time.time > damageDictionary[damageable] + 0.5f)
                    {
                        damageable?.TakeDamage(DMG, "fire");
                        damageDictionary[damageable] = Time.time;
                    }
                }
            }
        }


    }
}
