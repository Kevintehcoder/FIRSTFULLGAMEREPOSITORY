using UnityEngine;

public class PiercingBullet : BulletScript
{
    public float pierce;
    float original_Pierce = 1;
    protected PlayerData playerData;

    protected override void OnEnable()
    {
        base.OnEnable();

        
    }

    protected override void OnDisable()
    {
        base.OnDisable();

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

    }

    public void Initialize(PlayerData playerData)
    {
        pierce = original_Pierce + playerData.Pierce;
    }


    public override void onHitObject()
    {
        if (pierce == float.MaxValue)
        { return; }

        pierce -= 1;
        if (pierce == 0)
        {
            base.onHitObject();
        }
        
        
    }
}
