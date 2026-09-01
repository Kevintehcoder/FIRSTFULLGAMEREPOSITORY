using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class HazardScript : MonoBehaviour
{
    public LayerMask ObjectstoEffect;

    [Header("Status setting")]
    [SerializeField] StatusDMGdata statusData;
    [SerializeField] HazardType hazardType;
    float dmgAmount;

    [Space(15)]

    [Header("Duration settings")]
    [SerializeField] bool hasDuration = false;

    [SerializeField] float hazardDuration;

    [Space(15)]

    [Header("Lingering DMG settings")]
    [SerializeField] float dmgInterval_num;
    WaitForSeconds dmgInterval;
    [SerializeField] int dmgStep = 3;

    private Dictionary<IDamageable, Coroutine> objectsInHazard = new();

    [SerializeField] Object objectType;
    enum HazardType
    {
        Fire,
        Poison,
        Frozen,
        Mute
    }

    private void OnEnable()
    {
        dmgInterval = new WaitForSeconds(dmgInterval_num);
        SetHazardDMG();

        StartCoroutine(CleanUpDictionary());

        if (hasDuration == true)
        {
            StartCoroutine(DestroySelf());
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Collsion gameobject: {collision.gameObject.name}");
        if (((1 << collision.gameObject.layer) & ObjectstoEffect) != 0)
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();

            if (damageable != null)
            {
                if (objectsInHazard.ContainsKey(damageable) == false)
                {
                    Coroutine inZoneRoutine = StartCoroutine(InzoneCoroutine(damageable));
                    objectsInHazard.Add(damageable, inZoneRoutine);

                }

            }

        }

        if (collision.gameObject.layer == 7) //ENDWALL LAYER
        {
            gameObject.SetActive(false);
        }
    }

   


    private void OnTriggerExit2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            if (objectsInHazard.ContainsKey(damageable))
            {
                StopCoroutine(objectsInHazard[damageable]);


                objectsInHazard.Remove(damageable);
            }

            if (collision.gameObject != null && gameObject.activeInHierarchy == true)
            {
                StartCoroutine(LeaveZoneCoroutine(damageable));
            }

        }


    }



    public void SetHazardDMG()
    {
        switch (hazardType)
        { 
            case HazardType.Fire:
                dmgAmount = statusData.FireDMG;
                break;
            case HazardType.Poison:
                dmgAmount = statusData.PoisonDMG;
                break;
            case HazardType.Frozen:
                dmgAmount = statusData.FrozenDMG;
                break;
            case HazardType.Mute:
                dmgAmount = statusData.MuteDMG;
                break;
        }
    }

    IEnumerator LeaveZoneCoroutine(IDamageable damageable)
    {
        MonoBehaviour Object = damageable as MonoBehaviour;
       
        yield return new WaitForSeconds(0.3f);

        for(int i = 0; i < dmgStep; i++)
        {
            if (Object == null)
            {
                yield break;
            }

            damageable.TakeDamage(dmgAmount, "status");
            yield return dmgInterval;
        }
    }

    IEnumerator InzoneCoroutine(IDamageable damageable)
    {
        while (true)
        {
            damageable.TakeDamage(dmgAmount, "status");
            Debug.Log($"{(damageable as MonoBehaviour).gameObject.name} took dmg");
            yield return dmgInterval;
        }
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(hazardDuration);
        
        ObjectPoolManager.instance.EnqueueObject(objectType, gameObject);
        gameObject.SetActive(false);
    }

    #region Dictionary Cleanup
    IEnumerator CleanUpDictionary()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);

        while (true)
        {
            yield return wait;
            PruneDestroyedKeys();
        }
    }

    void PruneDestroyedKeys()
    { 
        List<IDamageable> toRemove = new List<IDamageable>();

        foreach (var key in objectsInHazard)
        { 
            MonoBehaviour obj = key.Key as MonoBehaviour;

            if (obj == null)
            { 
                toRemove.Add(key.Key);
            }
        }

        foreach (var key in toRemove)
        {
            if (objectsInHazard.TryGetValue(key, out Coroutine c) && c != null)
            {
                StopCoroutine(c);
            }
            objectsInHazard.Remove(key);
        }
    }
    #endregion
}
