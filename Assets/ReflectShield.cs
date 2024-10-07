using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ReflectShield : Shield
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Projectile>())
        {
            var proj = other.GetComponent<Projectile>();
            proj.owner = owner;
            if (proj.dir.x == 1)
            {
                proj.dir = new Vector3(-1, 0, 0);
            }
            else if(proj.dir.x == -1) 
            {
                proj.dir = new Vector3(1, 0, 0);
            }
            proj.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            proj.GetComponent<Rigidbody2D>().AddForce(proj.dir * proj.force, ForceMode2D.Impulse);
            Destroy(gameObject);

        }
        else if (other.GetComponent<ArcingProjectile>())
        {
            Debug.Log("here");
            var proj = Instantiate(other.gameObject, other.ClosestPoint(transform.position), Quaternion.identity);
            proj.GetComponent<ArcingProjectile>().owner = owner;


            Destroy(gameObject);
            Destroy(other.gameObject);

        }
    }
}
