using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnDOT : Debuff
{
    PlayerHealth health;
    public float burnDamage;
    public float burnInterval;
    float timer;


    public override void Apply()
    {
        health = GetComponent<PlayerHealth>();
        Destroy(this, duration);
    }


    // Update is called once per frame
    void Update()
    {
        if(health != null)
        {
            timer += Time.deltaTime;
            if (timer >= burnInterval)
            {
                health.TakeDamage(burnDamage);
                timer = 0;
            }
        }

        

    }
}
