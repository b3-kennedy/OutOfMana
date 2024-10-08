
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
    GameObject proj;

    public KeyCode orb1Key;
    public KeyCode orb2Key;
    public KeyCode orb3Key;
    public KeyCode castKey;


    public GameObject orb1;
    public GameObject orb2;
    public GameObject orb3;

    public Orb[] orbs;

    int orb1Count;
    int orb2Count;
    int orb3Count;

    public SpellBook book;

    public NetworkVariable<int> orbIndex;

    List<GameObject> serverOrbs = new List<GameObject>();

    int localOrbIndex;

    // Start is called before the first frame update
    void Start()
    {

    }

    public override void OnNetworkSpawn()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        if (IsHost)
        {
            if (Input.GetKeyDown(orb1Key))
            {
                SpawnOrb1ServerRpc(orbIndex.Value);

            }

            if (Input.GetKeyDown(orb2Key))
            {
                SpawnOrb2ServerRpc(orbIndex.Value);

            }

            if (Input.GetKeyDown(orb3Key))
            {
                SpawnOrb3ServerRpc(orbIndex.Value);

            }

            if (Input.GetKeyDown(castKey) && orbs[0].orbObj != null)
            {
                CountOrbsServerRpc();
                ClearOrbsServerRpc();
            }
        }
        else
        {
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







        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    SpawnProjectileServerRpc();
        //}
    }

    void ClearLocalOrbs()
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

    [ServerRpc]
    void ClearServerOrbsServerRpc()
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
    void OrbBoundCheckServerRpc()
    {
        if (orbIndex.Value >= 3)
        {
            orbIndex.Value = 0;
        }
    }

    [ServerRpc]
    void ClearOrbsServerRpc()
    {
        orbIndex.Value = 0;
        for (int i = 0; i < orbs.Length; i++)
        {
            if (orbs[i].orbObj != null)
            {
                Destroy(orbs[i].orbObj);
            }
        }
    }

    [ServerRpc]
    void SpawnOrb1ServerRpc(int orbI)
    {
        if (orbI >= 3)
        {
            orbI = 0;
        }
        OrbBoundCheckServerRpc();
        if (orbs[orbIndex.Value].orbObj != null)
        {
            Destroy(orbs[orbIndex.Value].orbObj);
        }

        orbs[orbI].orbObj = Instantiate(orb1, orbs[orbIndex.Value].orbPos.position, Quaternion.identity);
        orbs[orbI].orbObj.GetComponent<NetworkObject>().Spawn();

        SetOrbClientRpc(orbs[orbI].orbObj.GetComponent<NetworkObject>().NetworkObjectId, orbI);
        orbIndex.Value++;
    }

    [ServerRpc]
    void SpawnOrb2ServerRpc(int orbI)
    {
        if (orbI >= 3)
        {
            orbI = 0;
        }
        OrbBoundCheckServerRpc();
        if (orbs[orbIndex.Value].orbObj != null)
        {
            Destroy(orbs[orbIndex.Value].orbObj);
        }

        orbs[orbI].orbObj = Instantiate(orb2, orbs[orbIndex.Value].orbPos.position, Quaternion.identity);
        orbs[orbI].orbObj.GetComponent<NetworkObject>().Spawn();
        SetOrbClientRpc(orbs[orbI].orbObj.GetComponent<NetworkObject>().NetworkObjectId, orbI);
        orbIndex.Value++;
    }

    [ServerRpc]
    void SpawnOrb3ServerRpc(int orbI)
    {
        if (orbI >= 3)
        {
            orbI = 0;
        }
        OrbBoundCheckServerRpc();
        if (orbs[orbIndex.Value].orbObj != null)
        {
            Destroy(orbs[orbIndex.Value].orbObj);
        }
        orbs[orbI].orbObj = Instantiate(orb3, orbs[orbIndex.Value].orbPos.position, Quaternion.identity);
        orbs[orbI].orbObj.GetComponent<NetworkObject>().Spawn();
        SetOrbClientRpc(orbs[orbI].orbObj.GetComponent<NetworkObject>().NetworkObjectId, orbI);
        orbIndex.Value++;
    }

    [ClientRpc]
    void SetOrbClientRpc(ulong id, int index)
    {
        // Check if the player’s NetworkObject exists in the SpawnManager
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(id, out var orbNetObject))
        {
            var orbObj = orbNetObject.gameObject;
            orbs[index].orbObj = orbObj;
        }
    }

    [ServerRpc]
    void CountOrbsServerRpc()
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

        CastSpell(orb1Count, orb2Count, orb3Count);
    }

    [ServerRpc]
    void CountOrbsFromClientServerRpc(int orb1, int orb2, int orb3)
    {
        CastSpell(orb1, orb2, orb3);
    }

    void CastSpell(int orb1Count, int orb2Count, int orb3Count)
    {
        ClearOrbsServerRpc();
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

    [ServerRpc]
    void SpawnProjectileServerRpc()
    {
        SpawnProjectileClientRpc();
    }

    [ClientRpc]
    void SpawnProjectileClientRpc()
    {
        proj = Instantiate(projectile, projectileSpawn.position, Quaternion.identity);
        proj.GetComponent<Projectile>().dir = GetComponent<PlayerManager>().dir;
    }
}