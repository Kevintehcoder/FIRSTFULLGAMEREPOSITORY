using UnityEngine;
using System.Collections.Generic;

public class ShopManager: MonoBehaviour
{
    public static ShopManager instance;

    public PlayerShopData shopData = new();
    public ButtonCosts buttonCosts = new();

    [SerializeField]List<GameObject> buttons = new();

    private void Awake()
    {
        instance = this;
    }

}
