using UnityEngine;

public class SwordManager : MonoBehaviour
{
    [SerializeField] GameObject UnequippedSprite;
    [SerializeField] GameObject EquippedSprite;

    private void OnEnable()
    {
        GetComponentInParent<Inventory>().inventory.Add(EquippedSprite);
        UnequippedSprite.SetActive(true);
        EquippedSprite.SetActive(false);
    }

    void Update()
    {
        if (EquippedSprite.activeInHierarchy == false)
        {
            UnequippedSprite.SetActive(true);
        }
        else 
        {
            UnequippedSprite.SetActive(false);
        }
    }
}
