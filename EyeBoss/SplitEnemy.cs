using UnityEngine;
using System.Collections.Generic;

public class SplitEnemy : ChaseEnemy_Script
{
    [SerializeField] GameObject copyPrefab;
    public GameObject Sprite;
    float scaleDecrease = 0.75f;
    public int splitCount = 0;


    protected override float CurrentHP
    {
        get => base.CurrentHP;
        set
        {

            if (value <= 0)
            {
                Split();
            }

            base.CurrentHP = value;



        }
    }
    protected override void Start()
    {
        base.Start();

    }





    public void Split()
    {
        if (splitCount < 1)
        {
            for (int i = 0; i < 2; i++)
            {
                float randomValue = Random.Range(-2f, 2f);

                Vector3 spawnPosition = transform.position + new Vector3(randomValue, randomValue, 0);
                float spawnRotation = Sprite.transform.rotation.eulerAngles.z;



                GameObject spawned = Instantiate(copyPrefab, spawnPosition, Quaternion.identity, room.transform);
                spawned.transform.localScale = this.transform.localScale * scaleDecrease;

                SplitEnemy spawnedScript = spawned.GetComponentInChildren<SplitEnemy>();
                TargetPlayer targetplayerScript = spawned.GetComponentInChildren<TargetPlayer>();


                spawnedScript.Sprite.transform.rotation = Quaternion.Euler(0, 0, spawnRotation);
                spawnedScript.splitCount = this.splitCount + 1;
                spawnedScript.transform.position = spawnPosition;

            }

        }
    }
}
