using System.Collections;
using UnityEngine;

public class SpawnSpore : MonoBehaviour
{
    [SerializeField]float spawnRadius;
    int spawningAngle;

    private void OnEnable()
    {
        float timer = UnityEngine.Random.Range(0.5f, 2f);
        StartCoroutine(KillSelf(timer));
    }

    private void OnDisable()
    {
        SpawnSporePrefab();
    }

    IEnumerator KillSelf(float timer)
    {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }
    void SpawnSporePrefab()
    {
        spawningAngle = Random.Range(60, 73);
        for (int i = 0; i < 360; i += spawningAngle)
        {
            float rad = i * Mathf.Deg2Rad;
            Vector3 tempVec = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * spawnRadius;
            GameObject Spore_Puddle = ObjectPoolManager.instance.GetObject(Object.Spore_Puddle);

            Spore_Puddle.transform.position = gameObject.transform.position + tempVec;
            Spore_Puddle.SetActive(true);

        }

    }
}
