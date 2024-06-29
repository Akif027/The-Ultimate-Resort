using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationManager : Singleton<DestinationManager>
{
    [SerializeField]
    private List<GameObject> destinationObjects = new List<GameObject>();
    private Dictionary<string, Transform> destinations = new Dictionary<string, Transform>();


    private void Start()
    {
        // Initialize the destinations dictionary based on the assigned GameObjects
        foreach (GameObject destinationObject in destinationObjects)
        {
            string destinationId = destinationObject.name; // Using the GameObject's name as the ID
            if (!destinations.ContainsKey(destinationId))
            {
                destinations[destinationId] = destinationObject.transform;
            }
            else
            {
                Debug.LogWarning($"Destination with ID '{destinationId}' already exists.");
            }
        }
    }
    // Method to add a new destination
    public void AddDestination(string destinationId, Transform destinationTransform)
    {
        if (!destinations.ContainsKey(destinationId))
        {
            destinations[destinationId] = destinationTransform;
        }
        else
        {
            Debug.LogWarning($"Destination with ID '{destinationId}' already exists.");
        }
    }

    // Method to remove a destination
    public void RemoveDestination(string destinationId)
    {
        if (destinations.ContainsKey(destinationId))
        {
            destinations.Remove(destinationId);
        }
        else
        {
            Debug.LogWarning($"Destination with ID '{destinationId}' does not exist.");
        }
    }

    // Method to get a destination by its ID
    public Transform GetDestination(string destinationId)
    {
        if (destinations.TryGetValue(destinationId, out Transform destination))
        {
            return destination;
        }
        else
        {
            Debug.LogWarning($"Destination with ID '{destinationId}' does not exist.");
            return null;
        }
    }
    private void OnDestroy()
    {
        // Clear all destinations when the script instance is destroyed
        destinations.Clear();
        destinationObjects.Clear();
    }
}
