using System.Collections;
using UnityEngine;

public class ShootEnemy_Script : BasicEnemyScript
{
    [SerializeField] float detectionRange;

    [SerializeField] Transform shooterPos;

    protected override void Start()
    {
        base.Start();
  
    }

    private void OnEnable()
    {
        StartCoroutine(ShootAtPlayer());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(shooterPos.position, detectionRange);
    }

    public void ShootBullet()
    {
        Vector2 shootDir = Vlogic.FindDirection(shooterPos.position, focus.position);

        GameObject bullet = ObjectPoolManager.instance.GetObject(Object.EyeBossBullet);

        bullet.GetComponent<BulletScript>().DMG_data.dmgAmount = DMG_Data.dmgAmount;
        bullet.GetComponent<BulletScript>().dir = shootDir;

        bullet.transform.position = gameObject.transform.position;
        bullet.SetActive(true);
    }

    public IEnumerator ShootAtPlayer()
    {
        while (true)
        {
            if (Vector2.Distance(shooterPos.position, focus.position) < detectionRange)
            {
                ShootBullet();

                yield return new WaitForSeconds(0.5f);
            }

            yield return null;

        }
    }
}
