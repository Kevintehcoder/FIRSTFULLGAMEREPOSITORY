using UnityEngine;
using UnityEngine.InputSystem;

public class NightBladeScript : BasicSwordScript
{
    int count = 0;
    protected override void Awake()
    {
        base.Awake();
        if (playerCTRL == null)
        {
            playerCTRL = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        }
    }
    private void OnEnable()
    {
      
       playerCTRL.actions["Attack"].performed += SpawnArc;
        
    }

    private void OnDisable()
    {
        playerCTRL.actions["Attack"].performed -= SpawnArc;
    }

    void SpawnArc(InputAction.CallbackContext context)
    {
        Vector2 Mousepos = Camera.main.ScreenToWorldPoint(playerCTRL.actions["Aim"].ReadValue<Vector2>());
        Vector2 AbsoluteshootingDir = (Mousepos - (Vector2)transform.position).normalized;

        float shootingAngle = Mathf.Atan2(AbsoluteshootingDir.y, AbsoluteshootingDir.x) * Mathf.Rad2Deg;


        bool isFacingLeft = transform.lossyScale.x < 0;

        if (!isFacingLeft)
        {
            
            shootingAngle = Mathf.Clamp(shootingAngle, -90f, 90f);
        }
        else
        {
            if (shootingAngle > 0)
            { 
                shootingAngle = Mathf.Clamp(shootingAngle, 90f, 180f);
            }
            else
            {
                shootingAngle = Mathf.Clamp(shootingAngle, -180f, -90f);
            }
        }


        Quaternion Spawnrotation = Quaternion.Euler(0,0 ,shootingAngle);

        GameObject SpawnedArc = ObjectPoolManager.instance.GetObject(Object.Arc);


        SpawnedArc.transform.position = gameObject.transform.position;  
        SpawnedArc.transform.rotation = Spawnrotation;

        Vector2 clampedShootingDir = new Vector2(Mathf.Cos(shootingAngle * Mathf.Deg2Rad), Mathf.Sin(shootingAngle * Mathf.Deg2Rad));
        SpawnedArc.GetComponent<BasicArcScript>().dir = clampedShootingDir;

        SpawnedArc.SetActive(true);

        count += 1;

        if(count == 2)
        {

        }
    }

    


}
