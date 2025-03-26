using UnityEngine;
using UnityEngine.Events;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] private float healthMax;

    //A UnityEvent lets us plug behaviours into the inspector that will run when the event is invoked
    public UnityEvent onOutofHealth;

    private float healthCurrent;

    private void Start()
    {
        //Start at full health
        healthCurrent = healthMax;
    }

    public void TakeDamage(float damage) 
    {
        //Our new health is our old health minus damage, never going below 0
        healthCurrent = Mathf.Clamp(healthCurrent - damage, 0, healthMax);
        
        //if we run out of health
        if (healthCurrent <= 0) 
        {
            EndOfaLife();
        }
    }

    public void Heal(float amount) 
    {
        healthCurrent = Mathf.Clamp(healthCurrent + amount, 0, healthMax);
    }

    private void EndOfaLife() 
    {
        Debug.Log(name + " has died");

        //.Invoke() means "run all the behavious attached to this event"
        onOutofHealth.Invoke();
    }

    /// <summary>
    /// Returns a value between 0 and 1 represnting how full the health is.
    /// </summary>
    /// <returns></returns>
    public float GetHealthPercent() 
    {
        return healthCurrent / healthMax;
    }
}
