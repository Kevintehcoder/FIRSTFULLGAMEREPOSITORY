using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BasicArcScript : PiercingBullet
{
    Coroutine killCoroutine;
    protected override void Awake()
    {
        base.Awake();
        pierce = float.MaxValue;

    }
    protected override void OnEnable()
    {
        killCoroutine = StartCoroutine(KillSelf());
    }

    protected override void OnDisable()
    {
        if (killCoroutine != null)
        {
            StopCoroutine(killCoroutine);
        }

    }

    IEnumerator KillSelf()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
        ObjectPoolManager.instance.EnqueueObject(Object.Arc, gameObject);


    }

}
