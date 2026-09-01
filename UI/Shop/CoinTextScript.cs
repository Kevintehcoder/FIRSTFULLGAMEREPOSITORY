
using UnityEngine;

public enum StatType
{
    Coins,
    StrengthCost,
    PierceCost,
    SpeedCost
}

public class CoinTextScript : MonoBehaviour
{
    int CoinAmount;

    [SerializeField] StatType statType;
    TMPro.TextMeshProUGUI text;
    void Awake()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (statType == StatType.Coins)
        {
            PlayerShopData.OnCoinsChange += ChangePlayerCoins;
            ChangePlayerCoins(PlayerShopData.coins);
        }
        
    }

    private void OnDisable()
    {
        if (statType == StatType.Coins)
        {
            PlayerShopData.OnCoinsChange -= ChangePlayerCoins;
        }

    }

    public void ChangeCoinAmount(StatType displayType)
    {
        if (statType != StatType.Coins)
        {

            CoinAmount = GetComponentInParent<Shop_ButtonManager>().currentCost;


            text.text = $"{CoinAmount}";
        }

    }

    public void ChangePlayerCoins(int PlayerCoins)
    {
        text.text = $"{PlayerCoins}";
    }
}
