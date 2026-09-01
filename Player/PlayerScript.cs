using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerScript : MonoBehaviour, IDamageable, Iexploded
{
    [SerializeField] DamageLogic Dlogic;
    [SerializeField] GameObject SettingsUI;

    public PlayerData playerData = new();
    public PlayerInput playerCTRLS;

    public Rigidbody2D rb2d;
    [SerializeField] GunTarget Weapon;
    [SerializeField] GameObject sprintBar;
    SprintBar sprintBarScript;

    Vector2 dir;

    bool isSprintHeld = false;
    bool isSprinting = false;
    bool exploded = false;
    public bool ShootGun = false;




    float currentXscale;

    public Action hpChanged;
    void Awake()
    {
        playerData.currentHp = playerData.MaxHp;

        rb2d = GetComponent<Rigidbody2D>();
        playerCTRLS = GetComponent<PlayerInput>();

        currentXscale = transform.localScale.x;

        playerCTRLS.actions.Enable();

    }

    void Start()
    {
        sprintBarScript = sprintBar.GetComponent<SprintBar>();
    }

    private void OnEnable()
    {
        playerCTRLS.onActionTriggered += OnMoves;
        playerCTRLS.onActionTriggered += OnCancel;
        playerCTRLS.onActionTriggered += OnAttack;
        playerCTRLS.onActionTriggered += OnSprint;
    }
    private void OnDisable()
    {
        playerCTRLS.onActionTriggered -= OnMoves;
        playerCTRLS.onActionTriggered -= OnCancel;
        playerCTRLS.onActionTriggered -= OnSprint;
        playerCTRLS.onActionTriggered -= OnAttack;
    }



    void Update()
    {
        if (ShootGun == true && Weapon.gameObject.activeInHierarchy == true)
        {
            Weapon.TryShoot();
        }


        if (isSprintHeld)
        {
            if (sprintBarScript.fillAmount > 0)
            {
                sprintBarScript.UpdateImage();
                isSprinting = true;
            }
            else
            {
                isSprinting = false;
            }
        }
        else
        {
            sprintBarScript.UpdateImage_Regen();

            if (sprintBarScript.fillAmount == 1)
            {
                sprintBar.SetActive(false);
            }
        }



    }

    void FixedUpdate()
    {
        if (!exploded)
        {
            float multiplier = (isSprinting) ? 1.5f : 1;
            rb2d.linearVelocity = dir * (playerData.Speed * multiplier);
        }
    }

    public void OnMoves(InputAction.CallbackContext context)
    {
        if (context.action.name == "Moves")
        {
            dir = context.ReadValue<Vector2>();

            if (dir.x < 0)
            {
                transform.localScale = new Vector2(-currentXscale, transform.localScale.y);

            }
            else if (dir.x > 0)
            {
                transform.localScale = new Vector2(currentXscale, transform.localScale.y);

            }
        }
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
        if (context.action.name == "Cancel")
        {
            if (context.action.actionMap.name == "UI_Ctrl" && context.started)
            { 
                Debug.Log("Switching to Player_Ctrls");

                playerCTRLS.SwitchCurrentActionMap("Player_Ctrls");
                SettingsUI.SetActive(false);
            }
            else if (context.action.actionMap.name == "Player_Ctrls" && context.started)
            {
                Debug.Log("Switching to UI_Ctrl");

                playerCTRLS.SwitchCurrentActionMap("UI_Ctrl");
                SettingsUI.SetActive(true);
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.action.name == "Attack" && context.performed)
        {
            ShootGun = true;
        }
        else if (context.action.name == "Attack" && context.canceled)
        {
            ShootGun = false;
        }

    }

    public void OnSprint(InputAction.CallbackContext context)
    {

        if (context.action.name == "Sprint")
        {
            if (context.started)
            {
                sprintBar.SetActive(true);
            }

            if (context.performed)
            {
                isSprintHeld = true;
                
            }
            
            if(context.canceled)
            {
                isSprintHeld = false;

                isSprinting = false;

            }
        }

    }


    public void TakeDamage(float dmg, string damageType)
    {
        float totaldmg = Dlogic.CalcDamage(dmg, damageType);

        playerData.currentHp -= totaldmg;

        //Debug.LogWarning($"Player is taking DMG: {playerData.currentHp}");

        hpChanged?.Invoke();
        if (playerData.currentHp <= 0)
        {
            Debug.LogWarning("Player Died");
        }
    }

    public void Explode()
    {
        exploded = true;

        StartCoroutine(Exploded());
    }
    IEnumerator Exploded()
    {
        yield return new WaitForSeconds(0.2f);
        exploded = false;
    }


}
