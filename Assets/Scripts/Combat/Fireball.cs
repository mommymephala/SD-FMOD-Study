using UnityEngine;

public class Fireball : MonoBehaviour
{
    public ParticleSystem fireballParticles;
    public int damage = 10;

    void Start()
    {
        if (fireballParticles == null)
        {
            fireballParticles = GetComponent<ParticleSystem>();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log($"Fireball hit: {other.name}");

        var health = other.GetComponent<Health>();
        var runeStone = other.GetComponent<RuneStone>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
        if (runeStone != null)
        {
            runeStone.ActivateRune();
        }

        // Destroy the fireball (optional, or handle visual effects)
        // Destroy(gameObject);
    }
}