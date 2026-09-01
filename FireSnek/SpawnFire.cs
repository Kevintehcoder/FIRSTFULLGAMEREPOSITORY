using System.Collections;
using UnityEngine;

public class SpawnFire : MonoBehaviour
{
    [SerializeField] float Fire_spawnTime;

    private void OnEnable()
    {
        StartCoroutine(KillSelf(6));
        StartCoroutine(FireTrail());
    }

    private void OnDisable()
    {

    }

    IEnumerator KillSelf(float timer)
    {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }

    IEnumerator FireTrail()
    {
        WaitForSeconds waitTime = new WaitForSeconds(Fire_spawnTime);
        for (int i = 0; i < 10; i += 1)
        {
            yield return waitTime;
            SpawnFirePrefab();
        }
    }

    void SpawnFirePrefab()
    {
        GameObject Fire_Hazard = ObjectPoolManager.instance.GetObject(Object.Fire_Hazard);

        Fire_Hazard.transform.position = gameObject.transform.position;
        Fire_Hazard.SetActive(true);

    }
}
