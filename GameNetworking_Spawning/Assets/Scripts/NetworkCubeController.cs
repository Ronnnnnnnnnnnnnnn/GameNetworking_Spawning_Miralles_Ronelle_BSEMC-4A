using Unity.Netcode;
using UnityEngine;

public class NetworkCubeController : NetworkBehaviour
{
    [SerializeField] private float rotationSpeed = 90f;

    private void Update()
    {
        if (!IsServer)
            return;

        transform.Rotate(
            Vector3.up,
            rotationSpeed * Time.deltaTime
        );
    }
}