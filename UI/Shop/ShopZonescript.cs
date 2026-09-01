using UnityEngine;

public class ShopZonescript : MonoBehaviour
{
    [SerializeField] GameObject Entrance;
    [SerializeField] GameObject Shop;
    [SerializeField] ShopData ShopData;
    void Start()
    {
        ShopData.OnShopStateChanged += SetShop;
    }



    public void SetShop(bool isOpen)
    {

        Shop.SetActive(isOpen);
        Entrance.SetActive(!isOpen);
        
        
    }
}
