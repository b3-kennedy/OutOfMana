using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Projectile : NetworkBehaviour
{
    public GameObject owner;
    public int priority;
    public float damage;
    Rigidbody2D rb;
    float timer;
    bool hasFired = false;
    ulong ping;
    bool stop = false;
    public float force;
    [HideInInspector] public Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ping = PingHolder.Instance.ping;
        Debug.Log((ping / 1000f) / 2f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasFired)
        {
            timer += Time.deltaTime;
            if (timer >= (PingHolder.Instance.ping / 1000f) / 2f)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                hasFired = true;
                timer = 0;
            }
        }

    }

    private void FixedUpdate()
    {
        if (!stop && hasFired)
        {
            rb.AddForce(dir * force, ForceMode2D.Impulse);
            hasFired = false;
            stop = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerHealth>())
        {
            Debug.Log("hit");
            var health = other.GetComponent<PlayerHealth>();
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.GetComponent<Projectile>())
        {
            var proj = other.GetComponent<Projectile>();
            if (priority == proj.priority)
            {
                Destroy(proj.gameObject);
                Destroy(gameObject);
            }
            else if(priority > proj.priority)
            {
                Destroy(proj.gameObject);
            }
            else if(priority < proj.priority)
            {
                Destroy(gameObject);
            }
        }
    }
}
