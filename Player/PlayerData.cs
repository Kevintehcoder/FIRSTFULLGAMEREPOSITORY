
[System.Serializable]
public class PlayerData: BasicObjectStats
{
    public PlayerData() 
    {
        Speed = 15;
        MaxHp = 100;
    }


    public float playerStrength = 10f;

    public float playerDefense = 0f;

    public float Range = 10;

    public float Pierce = 1;

}
