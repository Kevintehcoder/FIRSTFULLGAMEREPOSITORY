using UnityEngine;

public class BasicSword_Anim : MonoBehaviour
{
    protected BasicSwordScript parentScript;
    protected void Start()
    {
        parentScript = GetComponentInParent<BasicSwordScript>();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        parentScript.CheckCollision(collision); 
    }

    protected void StopAttack()
    {
        ref bool attacking = ref GetComponentInParent<BasicSwordScript>().attacking;
        attacking = false;
    }
}
