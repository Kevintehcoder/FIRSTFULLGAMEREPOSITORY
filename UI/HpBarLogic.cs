using UnityEngine;
using UnityEngine.UI;

public class HpBarLogic : MonoBehaviour
{
    [SerializeField] GameObject focus;
    BasicObjectStats focusStats;
    Image hp_Bar;
   
    void Start()
    {
        hp_Bar = GetComponent<Image>();

        if (focus.name == "Player")
        {
            PlayerScript playerScript = focus.GetComponentInChildren<PlayerScript>();
            focusStats = playerScript.playerData;
            playerScript.hpChanged += UpdateHealthBar;
        }
        else
        { 
            focusStats = focus.GetComponentInChildren<BasicEnemyScript>().enemyData;
        }

        

    }



    public void UpdateHealthBar()
    {
        float playerHP_percent = focusStats.currentHp / focusStats.MaxHp;

        hp_Bar.fillAmount = playerHP_percent;

        if (hp_Bar.fillAmount > 0.25 && hp_Bar.fillAmount < 0.75)
        {
            hp_Bar.color = Color.yellow;
        }
        else if (hp_Bar.fillAmount < 0.25)
        {
            hp_Bar.color = Color.red;
        }
    }
}
