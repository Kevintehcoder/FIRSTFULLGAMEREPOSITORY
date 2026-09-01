using UnityEngine;

public class Shop_ButtonManager : MonoBehaviour
{
    [SerializeField] ShopData shopData;
    protected ButtonCosts buttonCost;
    protected PlayerData playerData;
    [HideInInspector] protected PlayerShopData playerShopData;

    CoinTextScript costScript;
    [SerializeField] private StatType displayType;

    public int currentCost;
    float increaseAmount;
    protected virtual void Start()
    {
        playerShopData = shopData.shopData;
        buttonCost = shopData.buttonCosts;

        playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().playerData;
        costScript = GetComponentInChildren<CoinTextScript>();


        currentCost = FindCostRef(displayType);
        increaseAmount = FindIncreaseRef(displayType);
    }

    public void OnClickUpgrade()
    {
        IncreasePlayerStats(displayType);
    }


    void IncreasePlayerStats(StatType type)    
    {

        if (PlayerShopData.coins >= currentCost)
        {
            IncreaseStat(type,increaseAmount);
            PlayerShopData.coins -= currentCost;

            currentCost += 8;
            increaseAmount *= 1.5f;
        }

        costScript.ChangeCoinAmount(displayType);
    }

    int FindCostRef(StatType type)
    {
        switch (type)
        { 
            case StatType.StrengthCost:
                return buttonCost.StrengthCost;

            case StatType.PierceCost:
                return buttonCost.PierceCost;

            case StatType.SpeedCost:
                return buttonCost.SpeedCost;

            default:
                throw new System.ArgumentOutOfRangeException(nameof(type), "Unsupported StatType");

        }

    }

    float FindIncreaseRef(StatType type)
    {
        switch (type)
        {
            case StatType.StrengthCost:
                return playerShopData.currentStrengthincrease;

            case StatType.PierceCost:
                return playerShopData.currentPierceincrease;

            case StatType.SpeedCost:
                return playerShopData.currentSpeedincrease;

            default:
                throw new System.ArgumentOutOfRangeException(nameof(type), "Unsupported StatType");

        }

    }

    void IncreaseStat(StatType type, float amount)
    {
        switch (type)
        {
            case StatType.StrengthCost:
                playerData.playerStrength += amount;
                break;

            case StatType.PierceCost:
                 playerData.Pierce += amount;
                break;

            case StatType.SpeedCost:
                 playerData.Speed += amount;
                break;


        }

    }

}
