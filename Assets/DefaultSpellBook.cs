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
    DefaultQQQ qqq;
    DefaultEEW eew;
    DefaultWWE eww;
    DefaultQWW qww;
    DefaultQQE qqe;


    private void Start()
    {
        eee = GetComponent<DefaultEEE>();
        www = GetComponent<DefaultWWW>();
        qwe = GetComponent<DefaultQWE>();
        qqq = GetComponent<DefaultQQQ>();
        eew = GetComponent<DefaultEEW>(); 
        eww = GetComponent<DefaultWWE>();
        qww = GetComponent<DefaultQWW>();
        qqe = GetComponent<DefaultQQE>();

    }

    public override void EEE()
    {
        if (!eee.onCd)
        {
            SpawnProjectileServerRpc(0,0,0);
            player.GetComponent<PlayerMana>().UseMana(eee.manaCost);
            eee.onCd = true;

        }
        
    }

    public override void WWW()
    {
        if (!www.onCd)
        {
            SpawnProjectileServerRpc(1,0,0);
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

    public override void QQQ()
    {
        if (!qqq.onCd)
        {
            SpawnShieldServerRpc(1);
            player.GetComponent<PlayerMana>().UseMana(qqq.manaCost);
            qqq.onCd = true;
        }
    }

    public override void EEW()
    {
        if (!eew.onCd)
        {
            SpawnProjectileServerRpc(2,0,0);
            player.GetComponent<PlayerMana>().UseMana(eew.manaCost);
            eew.onCd = true;
        }
    }

    public override void EWW()
    {
        if (!eww.onCd)
        {
            eww.book = this;
            eww.BeginCharge();
            player.GetComponent<PlayerMana>().UseMana(eww.manaCost);
            eww.onCd = true;
        }
    }

    public override void QWW()
    {
        if (!qww.onCd)
        {
            SpawnProjectileServerRpc(4,0,0);
        }
    }

    public override void QQE()
    {
        if (!qqe.onCd)
        {
            qqe.book = this;
            qqe.OnActivate();
            qqe.onCd = true;
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void SpawnProjectileServerRpc(int index, float offsetX, float offsetY)
    {
        SpawnProjectileClientRpc(index, offsetX, offsetY);
    }

    [ClientRpc]
    void SpawnProjectileClientRpc(int index, float offsetX, float offsetY)
    {
        var proj = Instantiate(projectiles[index].prefab, player.GetComponent<SpellManager>().projectileSpawn.position + new Vector3(offsetX, offsetY), Quaternion.identity);
        if (proj.GetComponent<Projectile>())
        {
            proj.GetComponent<Projectile>().dir = player.GetComponent<PlayerManager>().dir;
            proj.GetComponent<Projectile>().owner = player.gameObject;
        }
        else if (proj.GetComponent<ArcingProjectile>())
        {
            proj.GetComponent<ArcingProjectile>().owner = player.gameObject;
        }

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
