using System.Collections.Generic;
using UnityEngine;

public enum Object
{
    BasicBullet,
    PierceBullet,
    SporeBullet,
    Arc,
    BasicEnemyBullet,
    EyeBossBullet,
    FireBall,
    ExplosionBullet,
    ExpandingRing,

    Spore_Puddle,
    Fire_Hazard
}

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    [SerializeField] GameObject ArcPrefab;
    [SerializeField] GameObject BasicBulletPrefab;
    [SerializeField] GameObject PierceBulletPrefab;
    [SerializeField] GameObject SporeBulletPrefab;
    [SerializeField] GameObject BasicEnemyBulletPrefab;
    [SerializeField] GameObject EyeBossBulletPrefab;
    [SerializeField] GameObject FireBallPrefab;
    [SerializeField] GameObject ExplosionBulletPrefab;
    [SerializeField] GameObject ExpandingRingPrefab;

    [SerializeField] GameObject Spore_Puddle;
    [SerializeField] GameObject Fire_Hazard;

    Queue<GameObject> ArcList = new();
    Queue<GameObject> BasicBulletList = new();
    Queue<GameObject> BasicEnemyBulletList = new();
    Queue<GameObject> PierceBulletList = new();
    Queue<GameObject> SporeBulletList = new();
    Queue<GameObject> EyeBossBulletList = new();
    Queue<GameObject> FireBallList = new();
    Queue<GameObject> ExplosionBulletList = new();
    Queue<GameObject> ExpandingRingList = new();

    Queue<GameObject> Spore_PuddleList = new();
    Queue<GameObject> Fire_HazardList = new();

    Dictionary<Object, Queue<GameObject>> QueueDictionary;
    Dictionary<Object, GameObject> PrefabDictionary = new Dictionary<Object, GameObject>();

    private void Awake()
    {
        instance = this;

        QueueDictionary = new Dictionary<Object, Queue<GameObject>>
        {
            { Object.Arc, ArcList },
            { Object.BasicBullet, BasicBulletList },
            { Object.EyeBossBullet, EyeBossBulletList },
            { Object.SporeBullet, SporeBulletList },
            { Object.Fire_Hazard, Fire_HazardList },
            { Object.PierceBullet, PierceBulletList },
            { Object.BasicEnemyBullet, BasicEnemyBulletList },
            { Object.ExplosionBullet, ExplosionBulletList},
            { Object.ExpandingRing, ExpandingRingList},

            { Object.FireBall, FireBallList},
            { Object.Spore_Puddle, Spore_PuddleList },
        };

        PrefabDictionary = new Dictionary<Object, GameObject>
        {
            { Object.Arc, ArcPrefab },
            { Object.BasicBullet, BasicBulletPrefab },
            { Object.EyeBossBullet, EyeBossBulletPrefab },
            { Object.SporeBullet, SporeBulletPrefab },
            { Object.PierceBullet, PierceBulletPrefab },
            { Object.BasicEnemyBullet, BasicBulletPrefab },
            { Object.FireBall, FireBallPrefab},
            { Object.ExplosionBullet, ExplosionBulletPrefab },
            { Object.ExpandingRing, ExpandingRingPrefab},

            { Object.Spore_Puddle, Spore_Puddle },
            { Object.Fire_Hazard, Fire_Hazard }
        };
    }

    void Start()
    {
        StartPoolObjects(ArcPrefab, ArcList);
        StartPoolObjects(BasicEnemyBulletPrefab, BasicEnemyBulletList);
        StartPoolObjects(PierceBulletPrefab, PierceBulletList);
        StartPoolObjects(EyeBossBulletPrefab, EyeBossBulletList);
        StartPoolObjects(BasicBulletPrefab, BasicBulletList);
        StartPoolObjects(ExplosionBulletPrefab, ExplosionBulletList);
        StartPoolObjects(ExpandingRingPrefab, ExpandingRingList);

        StartPoolObjects(SporeBulletPrefab, SporeBulletList);
        StartPoolObjects(Spore_Puddle, Spore_PuddleList);
        StartPoolObjects(FireBallPrefab, FireBallList);
        StartPoolObjects(Fire_Hazard, Fire_HazardList);
    }



    void StartPoolObjects(GameObject ObjPrefab, Queue<GameObject> poolList)
    {
        for (int i = 0; i < 30; i++)
        {
            GameObject newObj = SpawnObject(ObjPrefab);

            poolList.Enqueue(newObj);

            newObj.transform.SetParent(gameObject.transform);
            
        }
    }

    GameObject SpawnObject(GameObject ObjPrefab)
    {
        GameObject newObj = Instantiate(ObjPrefab);

        newObj.SetActive(false);

        return newObj;
    }


    public GameObject GetObject(Object objects)
    {
        Queue<GameObject> list = QueueDictionary[objects];
        GameObject Prefab = PrefabDictionary[objects];

        if (list.Count ==  0)
        {
            return SpawnObject(Prefab);

        }
        GameObject returnObj = list.Dequeue();

        if (returnObj == null || returnObj.activeInHierarchy)
        {
            returnObj = SpawnObject(Prefab);
            list.Enqueue(returnObj);
        }

        return returnObj;

    }

    public void EnqueueObject(Object objects, GameObject gameObject)
    {
        Queue<GameObject> list = QueueDictionary[objects];

        
        if (list.Contains(gameObject) == false)
        {
            list.Enqueue(gameObject);
        }
    }

    
}
