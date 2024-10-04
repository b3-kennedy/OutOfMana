using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingHolder : MonoBehaviour
{
    public ulong ping;

    public static PingHolder Instance;

    private void Start()
    {
        Instance = this;
    }
}
