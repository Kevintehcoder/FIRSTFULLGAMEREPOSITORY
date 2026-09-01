using UnityEngine;

public class DeadZoneScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 7)
        {
            switch(collision.gameObject.layer)
            {
                case 6:
                case 11:
                case 12:
                    collision.gameObject.transform.parent.transform.position = new Vector3(10000, 100000, 0);
                    collision.gameObject.SetActive(false);
                    break;
            }

        }
    }
}
