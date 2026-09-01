using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasicSwordScript : WeaponSprite
{
    public LayerMask ObjectstoTarget;
    public Damage_Data damageData;

    protected PlayerData playerData;

    Animator animator;

    public bool attacking = false;

    protected virtual void Awake()
    {
   
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogError($"Hey! {gameObject.name} is missing an Animator component on itself or its children!", this);
        }
    }

    protected override void Start()
    {
        base.Start();
        PlayerScript playerscript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        playerData = playerscript.playerData;

        playerCTRL.actions["Attack"].performed += StartAttackAnimation;
    }

    protected override void Update()
    {
        if (attacking == false)
        {
            base.Update();
        }

    }


    public void CheckCollision(Collider2D collision)
    {
        if (attacking == true && ((1 << collision.gameObject.layer) & ObjectstoTarget) != 0)
        {
            Debug.LogWarning("Gameobject was hit by sword");

            IDamageable damageable = collision.GetComponent<IDamageable>();

            if (damageable != null)
            {
                float dmg = damageData.dmgAmount + playerData.playerStrength;
                damageable.TakeDamage(dmg, damageData.dmgTypeName);
            }     
        }
    }

    protected void StartAttackAnimation(InputAction.CallbackContext context)
    {
        animator.ResetTrigger("Attacking");
        attacking = true;

        animator.SetTrigger("Attacking");
    }


}
