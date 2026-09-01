using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public Object objectType;

    public void Spawn()
    {
        float randomRange = Random.Range(-5, 5);
        GameObject obj = ObjectPoolManager.instance.GetObject(objectType);
        obj.transform.position = new Vector2 (transform.position.x + randomRange, transform.position.y + randomRange);
        obj.transform.rotation = transform.rotation;
        obj.SetActive(true);
    }

}
