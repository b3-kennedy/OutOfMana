using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    DefaultQQW qqw;
    DefaultEEQ qee;


    bool findPlayer = false;


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
        qqw = GetComponent<DefaultQQW>();
        qee = GetComponent<DefaultEEQ>();

        spells.Add(eee);
        spells.Add(www);
        spells.Add(qwe);
        spells.Add(qqq);
        spells.Add(eew);
        spells.Add(eww);
        spells.Add(qww);
        spells.Add(qqe);
        spells.Add(qqw);
        spells.Add(qee);




        
    }

    public override void BookUpdate()
    {
        if(player != null && !findPlayer)
        {
            qee.player = player.gameObject;
            var spellManager = player.GetComponent<SpellManager>();
            eee.icon = spellManager.eeeIcon;
            qqq.icon = spellManager.qqqIcon;
            www.icon = spellManager.wwwIcon;
            qqe.icon = spellManager.qqeIcon;
            qqw.icon = spellManager.qqwIcon;
            eww.icon = spellManager.wweIcon;
            qww.icon = spellManager.wwqIcon;
            eew.icon = spellManager.eewIcon;
            qee.icon = spellManager.eeqIcon;
            qwe.icon = spellManager.qweIcon;

            eee.cdText = eee.icon.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            qqq.cdText = qqq.icon.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            www.cdText = www.icon.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            qqe.cdText = qqe.icon.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            qqw.cdText = qqw.icon.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            eww.cdText = eww.icon.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            qww.cdText = qww.icon.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            eew.cdText = eew.icon.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            qee.cdText = qee.icon.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            qwe.cdText = qwe.icon.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            findPlayer = true;
        }
    }

    public override void EEE()
    {
        if (!eee.onCd.Value && mana.currentMana.Value >= eee.manaCost)
        {
            SpawnProjectileServerRpc(0,0,0);
            player.GetComponent<PlayerMana>().UseMana(eee.manaCost);
            eee.onCd.Value = true;
            SetLastUsedSpellServerRpc(0);

        }
        
    }

    public override void WWW()
    {
        if (!www.onCd.Value && mana.currentMana.Value >= www.manaCost)
        {
            SpawnProjectileServerRpc(1,0,0);
            player.GetComponent<PlayerMana>().UseMana(www.manaCost);
            www.onCd.Value = true;
            SetLastUsedSpellServerRpc(1);
        }
        
    }

    public override void QWE()
    {
        if (!qwe.onCd.Value && mana.currentMana.Value >= qwe.manaCost)
        {
            SpawnShieldServerRpc(0);
            player.GetComponent<PlayerMana>().UseMana(qwe.manaCost);
            qwe.onCd.Value = true;
            SetLastUsedSpellServerRpc(2);
        }
        
    }

    public override void QQQ()
    {
        if (!qqq.onCd.Value && mana.currentMana.Value >= qqq.manaCost)
        {
            SpawnShieldServerRpc(1);
            player.GetComponent<PlayerMana>().UseMana(qqq.manaCost);
            qqq.onCd.Value = true;
            SetLastUsedSpellServerRpc(3);
        }
    }

    public override void EEW()
    {
        if (!eew.onCd.Value && mana.currentMana.Value >= eew.manaCost)
        {
            SpawnProjectileServerRpc(2,0,0);
            player.GetComponent<PlayerMana>().UseMana(eew.manaCost);
            eew.onCd.Value = true;
            SetLastUsedSpellServerRpc(4);
        }
    }

    public override void EWW()
    {
        if (!eww.onCd.Value && mana.currentMana.Value >= eww.manaCost)
        {
            eww.book = this;
            eww.BeginCharge();
            player.GetComponent<PlayerMana>().UseMana(eww.manaCost);
            eww.onCd.Value = true;
            SetLastUsedSpellServerRpc(5);
        }
    }

    public override void QWW()
    {
        if (!qww.onCd.Value && mana.currentMana.Value >= qww.manaCost)
        {
            SpawnProjectileServerRpc(4,0,0);
            player.GetComponent<PlayerMana>().UseMana(qww.manaCost);
            qww.onCd.Value = true;
            SetLastUsedSpellServerRpc(6);
        }
    }

    public override void QQE()
    {
        if (!qqe.onCd.Value && mana.currentMana.Value >= qqe.manaCost)
        {
            qqe.book = this;
            qqe.OnActivate();
            qqe.onCd.Value = true;
            SetLastUsedSpellServerRpc(7);
        }
    }

    public override void QQW()
    {
        if(!qqw.onCd.Value && mana.currentMana.Value >= qqw.manaCost)
        {
            SpawnProjectileServerRpc(5, 0, 0);
            player.GetComponent<PlayerMana>().UseMana(qqw.manaCost);
            qqw.onCd.Value = true;
            SetLastUsedSpellServerRpc(8);
        }
    }

    public override void QEE()
    {
        if(!qee.onCd.Value && mana.currentMana.Value >= qee.manaCost)
        {
            qee.player = player.gameObject;
            qee.SpawnMinion();
            player.GetComponent<PlayerMana>().UseMana(qee.manaCost);
            qee.onCd.Value = true;
            SetLastUsedSpellServerRpc(9);
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
