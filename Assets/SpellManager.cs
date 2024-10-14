
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[System.Serializable]
public class Orb
{
    public GameObject orbObj;
    public Transform orbPos;
}

public class SpellManager : NetworkBehaviour
{
    public GameObject projectile;
    public Transform projectileSpawn;
    public Transform minionSpawn;

    GameObject proj;

    public KeyCode orb1Key;
    public KeyCode orb2Key;
    public KeyCode orb3Key;
    public KeyCode castKey;


    public GameObject orb1;
    public GameObject orb2;
    public GameObject orb3;

    public Orb[] orbs;

    public SpellBook book;

    public NetworkVariable<int> orbIndex;

    List<GameObject> serverOrbs = new List<GameObject>();

    int localOrbIndex;

    public NetworkVariable<bool> canCast;

    public GameObject eeeIcon;
    public GameObject qqqIcon;
    public GameObject wwwIcon;
    public GameObject qqeIcon;
    public GameObject qqwIcon;
    public GameObject wweIcon;
    public GameObject wwqIcon;
    public GameObject eewIcon;
    public GameObject eeqIcon;
    public GameObject qweIcon;

    public Transform iconParent;

    // Start is called before the first frame update
    void Start()
    {

    }

    public override void OnNetworkSpawn()
    {
        canCast.Value = true;

        if (!IsOwner)
        {
            iconParent.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner || !canCast.Value) return;

        if (Input.GetKeyDown(orb1Key))
        {
            Vector2 pos = LocalOrb(orb1);
            SpawnOrbServerRpc(1, pos.x, pos.y);

        }

        if (Input.GetKeyDown(orb2Key))
        {
            Vector2 pos = LocalOrb(orb2);
            SpawnOrbServerRpc(2, pos.x, pos.y);

        }

        if (Input.GetKeyDown(orb3Key))
        {
            Vector2 pos = LocalOrb(orb3);
            SpawnOrbServerRpc(3, pos.x, pos.y);

        }

        if (Input.GetKeyDown(castKey) && orbs[0].orbObj != null)
        {
            CountOrbLocal();
            ClearLocalOrbs();
            ClearServerOrbsServerRpc();
        }
    }

    public void ClearLocalOrbs()
    {
        for (int i = 0; i < orbs.Length; i++)
        {
            if (orbs[i].orbObj != null)
            {
                Destroy(orbs[i].orbObj);
            }
        }
    }

    void CountOrbLocal()
    {
        int orb1Count = 0;
        int orb2Count = 0;
        int orb3Count = 0;

        for (int i = 0; i < orbs.Length; i++)
        {
            if (orbs[i] != null)
            {
                if (orbs[i].orbObj.GetComponent<OrbType>().type == OrbType.Type.FIRST)
                {
                    orb1Count++;
                }
                else if (orbs[i].orbObj.GetComponent<OrbType>().type == OrbType.Type.SECOND)
                {
                    orb2Count++;
                }
                else if (orbs[i].orbObj.GetComponent<OrbType>().type == OrbType.Type.THIRD)
                {
                    orb3Count++;
                }
            }

        }
        CountOrbsFromClientServerRpc(orb1Count, orb2Count, orb3Count);
    }


    [ServerRpc]
    void SpawnOrbServerRpc(int orbType, float x, float y)
    {
        switch (orbType)
        {
            case 1:
                var orbObj = Instantiate(orb1, new Vector3(x,y), Quaternion.identity);
                orbObj.GetComponent<NetworkObject>().Spawn();
                serverOrbs.Add(orbObj);
                break;
            case 2:
                var orbObj2 = Instantiate(orb2, new Vector3(x, y), Quaternion.identity);
                orbObj2.GetComponent<NetworkObject>().Spawn();
                serverOrbs.Add(orbObj2);
                break;
            case 3:
                var orbObj3 = Instantiate(orb3, new Vector3(x, y), Quaternion.identity);
                orbObj3.GetComponent<NetworkObject>().Spawn();
                serverOrbs.Add(orbObj3);
                break;

        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void ClearServerOrbsServerRpc()
    {
        foreach (var orb in serverOrbs)
        {
            Destroy(orb);
        }
    }


    private Vector2 LocalOrb(GameObject orbPrefab)
    {
        // Predict where the next orb should be placed based on the local index
        var predictedIndex = localOrbIndex; // Use the local index for prediction
        if (predictedIndex >= 3)
        {
            predictedIndex = 0; // Wrap around to prevent index out of bounds
        }

        // Destroy any existing orb at the predicted index (client-side)
        if (orbs[predictedIndex].orbObj != null)
        {
            Destroy(orbs[predictedIndex].orbObj);
        }

        // Spawn the predicted orb locally for immediate feedback
        orbs[predictedIndex].orbObj = Instantiate(orbPrefab, orbs[predictedIndex].orbPos.position, Quaternion.identity);

        // Update the local prediction index (without touching orbIndex, which is controlled by the server)
        localOrbIndex = (localOrbIndex + 1) % 3;

        return new Vector2(orbs[predictedIndex].orbObj.transform.position.x, orbs[predictedIndex].orbObj.transform.position.y);
    }

    [ServerRpc]
    void CountOrbsFromClientServerRpc(int orb1, int orb2, int orb3)
    {
        CastSpell(orb1, orb2, orb3);
    }

    void CastSpell(int orb1Count, int orb2Count, int orb3Count)
    {
        if (orb1Count == 3)
        {
            book.QQQ();
            return;
        }
        else if (orb2Count == 3)
        {
            book.WWW();
            return;
        }
        else if (orb3Count == 3)
        {
            book.EEE();
            return;
        }
        else if (orb1Count == 1 && orb3Count == 2)
        {
            book.QEE();
            return;
        }
        else if (orb1Count == 1 && orb2Count == 2)
        {
            book.QWW();
            return;
        }
        else if (orb1Count == 2 && orb2Count == 1)
        {
            book.QQW();
            return;
        }
        else if (orb1Count == 2 && orb3Count == 1)
        {
            book.QQE();
            return;
        }
        else if (orb2Count == 2 && orb3Count == 1)
        {
            book.EWW();
            return;
        }
        else if (orb2Count == 1 && orb3Count == 2)
        {
            book.EEW();
            return;
        }
        else if (orb1Count == 1 && orb2Count == 1 && orb3Count == 1)
        {
            book.QWE();
            return;
        }
    }
}