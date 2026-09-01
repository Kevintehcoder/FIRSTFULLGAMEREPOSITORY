using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header("Bullet setting")]
    public float speed;

    public Damage_Data DMG_data;
    [SerializeField] LayerMask mask;

    [Header("Set by parent")]
    public Vector2 dir;
    public Vector2 initialVel;

    [HideInInspector]public Rigidbody2D rb2d;

    [SerializeField]protected Object objectType;
    protected virtual void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();

    }
    protected virtual void OnEnable()
    {
        if (rb2d != null)
        {
            rb2d.linearVelocity = Vector2.zero;
        }

    }

    protected virtual void OnDisable()
    {
        ObjectPoolManager.instance.EnqueueObject(objectType, gameObject);
    }


    protected virtual void FixedUpdate()
    {
        rb2d.linearVelocity = dir * speed + initialVel;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & mask) != 0)
        {
            Debug.Log("Object is in mask");

            IDamageable DamageableObject = collision.gameObject.GetComponent<IDamageable>();
            if (DamageableObject != null)
            {
                DamageableObject.TakeDamage(DMG_data.dmgAmount, DMG_data.dmgTypeName);
            }

            onHitObject();
            
        }

    }

    public virtual void onHitObject()
    {
        gameObject.SetActive(false);
        
    }


}
