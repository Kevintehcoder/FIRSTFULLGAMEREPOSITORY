using System;


public class PlayerShopData
{
    private static int _coin;
    public static int coins 
    {
        get
        {
            return _coin;
        }
        set 
        {
            _coin = value;
            OnCoinsChange?.Invoke(_coin);
        }
    }

    static public event Action<int> OnCoinsChange;



    public float currentStrengthincrease = 5;
    public float currentSpeedincrease = 5;
    public float currentPierceincrease = 5;
}
