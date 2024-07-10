using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OntriggerEnter : MonoBehaviour
{
    public UnityEvent OntriggerEvent;
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the player
        if (other.gameObject.CompareTag("Customer"))
        {
            OntriggerEvent?.Invoke();
        }
    }
}
