using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSprite : MonoBehaviour
{
    public MiscLogicManager miscLogic;

    protected float currentFacingRotation;

    public float shootingArcMax; 
    public float shootingArcMin;

    protected PlayerInput playerCTRL;

    protected virtual void Start()
    {
        playerCTRL =  GetComponentInParent<PlayerScript>().playerCTRLS;
    }

    protected virtual void Update()
    {
        currentFacingRotation = transform.lossyScale.x < 0 ? 180f : 0f;

        rotateSprite();

    }

    public void rotateSprite()
    {
        Vector2 mouseScreenPos = playerCTRL.actions["Aim"].ReadValue<Vector2>();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, 10f));

        transform.rotation = miscLogic.RelativeFollowFocus(transform.position, mouseWorldPos, currentFacingRotation, shootingArcMin, shootingArcMax);
    }
}
