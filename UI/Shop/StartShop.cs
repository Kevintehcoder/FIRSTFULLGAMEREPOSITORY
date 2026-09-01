using UnityEngine;

public class StartShop : MonoBehaviour
{
    [SerializeField] GameObject ShopUI;

    PlayerScript playerscript;
    void Start()
    {
        playerscript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            ShopUI.SetActive(true);
            playerscript.playerCTRLS.actions["Attack"].Disable();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            ShopUI.SetActive(false);
            playerscript.playerCTRLS.actions["Attack"].Enable();
        }
    }
}
