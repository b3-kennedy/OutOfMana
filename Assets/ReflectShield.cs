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
            Debug.Log("refelc");
            var proj = other.GetComponent<Projectile>();
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
            Debug.Log(proj.dir);

        }
    }
}
