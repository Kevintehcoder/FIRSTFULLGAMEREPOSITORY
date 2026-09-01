using UnityEngine;

public class BouncyBall_Script : MonoBehaviour
{
    [SerializeField] Damage_Data DMG_data;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        { 
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

            damageable.TakeDamage(DMG_data.dmgAmount, DMG_data.dmgTypeName);
        }
    }
}
