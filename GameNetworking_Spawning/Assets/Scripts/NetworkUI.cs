using Unity.Netcode;
using UnityEngine;
using TMPro;

public class NetworkUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text statusText;

    [Header("Spawner")]
    [SerializeField] private NetworkSpawner networkSpawner;

    private void Start()
    {
        UpdateStatus();

        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }

    public void StartHost()
    {
        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("Host started.");
            UpdateStatus();
        }
        else
        {
            Debug.LogError("Failed to start Host.");
        }
    }

    public void StartClient()
    {
        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Client started.");
            UpdateStatus();
        }
        else
        {
            Debug.LogError("Failed to start Client.");
        }
    }

    public void SpawnObject()
    {
        if (networkSpawner == null)
        {
            Debug.LogError("NetworkSpawner is not assigned.");
            return;
        }

        networkSpawner.SpawnObject();
    }

    private void OnServerStarted()
    {
        UpdateStatus();
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log("Client connected: " + clientId);
        UpdateStatus();
    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log("Client disconnected: " + clientId);
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        if (statusText == null)
            return;

        if (NetworkManager.Singleton == null)
        {
            statusText.text = "Status: NetworkManager Missing";
            return;
        }

        if (NetworkManager.Singleton.IsHost)
        {
            statusText.text = "Status: HOST";
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            statusText.text = "Status: CLIENT";
        }
        else
        {
            statusText.text = "Status: OFFLINE";
        }
    }
}