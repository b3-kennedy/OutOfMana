using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbShield : Shield
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Projectile>() || other.GetComponent<ArcingProjectile>())
        {
            var health = owner.GetComponent<PlayerHealth>();
            if (other.GetComponent<Projectile>())
            {
                health.GainHealth(other.GetComponent<Projectile>().damage);
            }
            else if (other.GetComponent<ArcingProjectile>())
            {
                health.GainHealth(other.GetComponent<ArcingProjectile>().damage);
            }
            
            Destroy(other.gameObject);
            Destroy(gameObject);
            

        }
    }
}
