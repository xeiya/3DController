using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private DamageReceiver damageReceiver;

    private Image healthBar;
    
    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    void Update()
    {
        //set the flll amount using the damage reciever's health percent
        healthBar.fillAmount = damageReceiver.GetHealthPercent();
    }
}
