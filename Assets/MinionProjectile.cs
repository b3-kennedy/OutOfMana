using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class MinionProjectile : MonoBehaviour
{
    public GameObject player;
    public GameObject projectile;
    public float interval;
    public Transform spawnPos;
    public float duration;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, duration);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= interval)
        {
            SpawnProjectileServerRpc();
            timer = 0;
        }
    }

    [ServerRpc]
    void SpawnProjectileServerRpc()
    {
        SpawnProjectileClientRpc();
    }

    [ClientRpc]
    void SpawnProjectileClientRpc()
    {
        var proj = Instantiate(projectile, spawnPos.position, Quaternion.identity);
        proj.GetComponent<Projectile>().dir = player.GetComponent<PlayerManager>().dir;
        Debug.Log(player.GetComponent<PlayerManager>().dir);
        proj.GetComponent<Projectile>().owner = player.gameObject;
    }
}
