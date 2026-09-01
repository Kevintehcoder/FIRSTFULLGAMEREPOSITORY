using System.Collections.Generic;
using UnityEngine;

public class FireSnekManager : MonoBehaviour
{
    [SerializeField] List<FollowFocus> bodysegments = new();

    public FireSnekHead head;
    public GameObject bodySeg;

    void Awake()
    {
        SpawnObject explosionBullet = GetComponentInChildren<SpawnObject>();
        head.SpeedChanges += changeSegment_Speed;
        head.explosionBullet = explosionBullet;
        head.EnableSegments += EnableBodySeg;

        FollowFocus.Speed = head.enemyData.Speed;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (var bodyScript in bodysegments)
        {
            bodyScript.followFocus();
        }
    }

    public void changeSegment_Speed(bool chargingState, Vector2 dir, float force)
    {

        if (chargingState)
        {
            FollowFocus.currentResponseTime = FollowFocus.SpeedyresponseTime;
            FollowFocus.Speed = head.enemyData.Speed * 800;

            foreach (var bodyScript in bodysegments)
            {
                bodyScript.rb2d.AddForce(dir * force, ForceMode2D.Impulse);
            }


        }
        else 
        {
            FollowFocus.currentResponseTime = FollowFocus.NormalresponseTime;
            FollowFocus.Speed = head.enemyData.Speed * 1.2f;

        }
    }

    public void EnableBodySeg(bool enabled)
    {
        foreach (var bodyScript in bodysegments)
        {
            bodyScript.gameObject.SetActive(enabled);
        }
    }


}
