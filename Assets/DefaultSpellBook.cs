using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[System.Serializable]
public class  ProjectilesArrayElement
{
    public string projName;
    public GameObject prefab;
}

[System.Serializable]
public class ShieldsArrayElement
{
    public string shieldName;
    public GameObject prefab;
}


public class DefaultSpellBook : SpellBook
{

    public ProjectilesArrayElement[] projectiles;
    public ShieldsArrayElement[] shields;
    DefaultEEE eee;
    DefaultWWW www;
    DefaultQWE qwe;


    private void Start()
    {
        eee = GetComponent<DefaultEEE>();
        www = GetComponent<DefaultWWW>();
        qwe = GetComponent<DefaultQWE>();
    }

    public override void EEE()
    {
        if (!eee.onCd)
        {
            SpawnProjectileServerRpc(0);
            player.GetComponent<PlayerMana>().UseMana(eee.manaCost);
            eee.onCd = true;

        }
        
    }

    public override void WWW()
    {
        if (!www.onCd)
        {
            SpawnProjectileServerRpc(1);
            player.GetComponent<PlayerMana>().UseMana(www.manaCost);
            www.onCd = true;
        }
        
    }

    public override void QWE()
    {
        if (!qwe.onCd)
        {
            SpawnShieldServerRpc(0);
            player.GetComponent<PlayerMana>().UseMana(qwe.manaCost);
            qwe.onCd = true;
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnProjectileServerRpc(int index)
    {
        SpawnProjectileClientRpc(index);
    }

    [ClientRpc]
    void SpawnProjectileClientRpc(int index)
    {
        var proj = Instantiate(projectiles[index].prefab, player.GetComponent<SpellManager>().projectileSpawn.position, Quaternion.identity);
        proj.GetComponent<Projectile>().dir = player.GetComponent<PlayerManager>().dir;
        proj.GetComponent<Projectile>().owner = player.gameObject;
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnShieldServerRpc(int index)
    {
        SpawnShieldClientRpc(index);
    }

    [ClientRpc]
    void SpawnShieldClientRpc(int index)
    {
        var shield = Instantiate(shields[index].prefab, player.position, Quaternion.identity);
        shield.GetComponent<Shield>().owner = player.gameObject;

    }


}
