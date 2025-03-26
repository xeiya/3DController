using UnityEngine;

public class DamageSource : MonoBehaviour
{
    public enum Apply 
    { 
        Instant,
        OverTime
    }

    public enum Type 
    { 
        Standard,
        Fire,
        Ice,
        Poison,
        Bleed,
        Shock,
        Explosion,
        Radiation
    }
    
    [SerializeField] private float damageAmount;
    [SerializeField] private Apply apply;
    [SerializeField] private Type type;

    private void DealDamage(GameObject target) 
    {
        //If we find the damage reveiver component, AND the target is not on the same layer as us
        if (target.GetComponent<DamageReceiver>() && target.layer != gameObject.layer) 
        {
            target.GetComponent<DamageReceiver>().TakeDamage(damageAmount);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        DealDamage(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        DealDamage(other.gameObject);
    }

    private void OnParticleCollision(GameObject other)
    {
        DealDamage(other);
    }
}
