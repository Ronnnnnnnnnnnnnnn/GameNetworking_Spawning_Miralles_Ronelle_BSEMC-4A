using Unity.Netcode;
using UnityEngine;

public class NetworkSpawner : NetworkBehaviour
{
    [Header("Network Prefab")]
    [SerializeField] private GameObject networkPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private float despawnDelay = 10f;

    private NetworkObject spawnedObject;

    public void SpawnObject()
    {
        // Only the server/host is allowed to spawn network objects.
        if (!IsServer)
        {
            Debug.Log("Only the Host/Server can spawn objects.");
            return;
        }

        // Prevent multiple objects from being spawned at once.
        if (spawnedObject != null && spawnedObject.IsSpawned)
        {
            Debug.Log("An object is already spawned.");
            return;
        }

        // 1. Instantiate the prefab.
        GameObject newObject = Instantiate(
            networkPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        // 2. Configure the object.
        spawnedObject = newObject.GetComponent<NetworkObject>();

        if (spawnedObject == null)
        {
            Debug.LogError("The prefab does not contain a NetworkObject!");
            Destroy(newObject);
            return;
        }

        // 3. Network spawn.
        spawnedObject.Spawn();

        Debug.Log("Network object spawned!");

        // Automatically despawn after the specified time.
        Invoke(nameof(DespawnObject), despawnDelay);
    }

    private void DespawnObject()
    {
        if (!IsServer)
            return;

        if (spawnedObject != null && spawnedObject.IsSpawned)
        {
            spawnedObject.Despawn(true);

            Debug.Log("Network object despawned.");

            spawnedObject = null;
        }
    }
}