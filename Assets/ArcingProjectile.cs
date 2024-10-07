using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcingProjectile : MonoBehaviour
{

    public GameObject owner;

    public float damage;
    public float priority;

    public float flightDuration = 1f; // Time it takes to reach the target
    public float arcHeight = 5f; // Height of the arc

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float timeElapsed;



    void Start()
    {
        if(owner != null)
        {
            startPosition = owner.GetComponent<SpellManager>().projectileSpawn.position;
            targetPosition = owner.GetComponent<PlayerManager>().otherPos;
        }
        timeElapsed = 0f;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        // Calculate the percentage of time passed
        float progress = timeElapsed / flightDuration;

        // Interpolate the horizontal position
        Vector2 horizontalPosition = Vector2.Lerp(startPosition, targetPosition, progress);

        // Calculate the height at this point in time using a parabola (sinusoidal arc)
        float height = Mathf.Sin(Mathf.PI * progress) * arcHeight;

        // Update the projectile's position
        transform.position = new Vector2(horizontalPosition.x, horizontalPosition.y + height);

        // Destroy the projectile when it reaches the target
        if (progress >= 1f)
        {
            Destroy(gameObject);
        }
    }

    void ApplyDebuff(GameObject other)
    {
        if (GetComponent<BurnDOT>())
        {
            var burn = other.AddComponent<BurnDOT>();
            burn.duration = GetComponent<BurnDOT>().duration;
            burn.burnInterval = GetComponent<BurnDOT>().burnInterval;
            burn.burnDamage = GetComponent<BurnDOT>().burnDamage;
            other.GetComponent<BurnDOT>().Apply();

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerHealth>())
        {
            Debug.Log("hit");
            var health = other.GetComponent<PlayerHealth>();
            health.TakeDamage(damage);

            if (GetComponent<Debuff>())
            {
                ApplyDebuff(other.gameObject);
            }

            Destroy(gameObject);
        }
        else if (other.GetComponent<Projectile>() || other.GetComponent<ArcingProjectile>())
        {
            
            if (other.GetComponent<ArcingProjectile>())
            {
                var proj = other.GetComponent<ArcingProjectile>();
                if (priority == proj.priority)
                {
                    Destroy(proj.gameObject);
                    Destroy(gameObject);
                }
                else if (priority > proj.priority)
                {
                    Destroy(proj.gameObject);
                }
                else if (priority < proj.priority)
                {
                    Destroy(gameObject);
                }
            }
            else if (other.GetComponent<Projectile>())
            {
                var proj = other.GetComponent<Projectile>();
                if (priority == proj.priority)
                {
                    Destroy(proj.gameObject);
                    Destroy(gameObject);
                }
                else if (priority > proj.priority)
                {
                    Destroy(proj.gameObject);
                }
                else if (priority < proj.priority)
                {
                    Destroy(gameObject);
                }
            }

        }
    }
}
