using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Sprite icon;
    public GameObject player;
    public virtual void Use() { }
}
