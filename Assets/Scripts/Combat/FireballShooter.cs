using UnityEngine;

public class FireballShooter : MonoBehaviour
{
    [Header("Fireball Settings")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    public float fireballLifetime = 5f;
    private float nextFireTime;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextFireTime)
        {
            ShootFireball();
        }
    }

    void ShootFireball()
    {
        nextFireTime = Time.time + 1f / fireRate;
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);

        // Set the fireball's forward velocity (using the particle system for visual speed)
        ParticleSystem fireballParticles = fireball.GetComponent<ParticleSystem>();
        if (fireballParticles != null)
        {
            // Adjust the particle system's velocity (if needed)
            var mainModule = fireballParticles.main;
            mainModule.startLifetime = fireballLifetime;
        }

        AudioManager.instance.PlaySound(AudioManager.instance.fireSound);

        Destroy(fireball, fireballLifetime);
    }
}