using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaLeech : Debuff
{
    public float manaAmount;
    [HideInInspector] public GameObject owner;
    public override void Apply()
    {
        Debug.Log("mana leech");
        owner.GetComponent<PlayerMana>().GainMana(manaAmount);
        GetComponent<PlayerMana>().UseMana(manaAmount);
        float value = GetComponent<PlayerMana>().currentMana.Value - manaAmount;
        if(value < 0)
        {
            float distBelow = Mathf.Abs(value);
            GetComponent<PlayerHealth>().TakeDamage(distBelow);
        }
        Destroy(this);
    }
}
