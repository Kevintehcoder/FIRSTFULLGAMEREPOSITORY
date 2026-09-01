using System.Collections;
using UnityEngine;


public class GunTarget : WeaponSprite
{
    PlayerData playerStats;

    [Header("Shooting settings")]
    public Object CurrentBullet;
    [SerializeField] float FireRate;

    Coroutine shootingCooldown;
    PlayerScript playerscript;

    protected override void Start()
    {
        base.Start();
        playerscript = GetComponentInParent<PlayerScript>();
        playerStats = playerscript.playerData;
        shootingArcMax = 60;
        shootingArcMin = -60;
    }

    protected override void Update()
    {
        base.Update();
    }


    public void TryShoot()
    {
        if (shootingCooldown == null)
        {
            SpawnBullet();
            shootingCooldown = StartCoroutine(ShootCooldown());
        }    

    }

    private IEnumerator ShootCooldown()
    { 
        yield return new WaitForSeconds(FireRate);

        shootingCooldown = null;
    }


    public void SpawnBullet()
    {
        GameObject bullet = ObjectPoolManager.instance.GetObject(CurrentBullet);
        bullet.transform.position = gameObject.transform.position;


        BulletScript bulletScript = bullet.GetComponent<BulletScript>();
        bulletScript.DMG_data.dmgAmount += playerStats.playerStrength;

        Vector2 shoot_dir = currentFacingRotation == 180 ? -1 * (Vector2)transform.right : (Vector2)transform.right;
        bulletScript.dir = shoot_dir;
        bulletScript.initialVel = playerscript.rb2d.linearVelocity;

        if (bulletScript is PiercingBullet pierceBullet)
        {
            if (pierceBullet.pierce != float.MaxValue)
            {
                pierceBullet.Initialize(playerStats);
            }
        }

        bullet.SetActive(true);

    }
}