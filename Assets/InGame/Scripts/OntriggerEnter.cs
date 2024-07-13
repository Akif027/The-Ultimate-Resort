using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class OntriggerEnter : MonoBehaviour
{
    public UnityEvent OntriggerEvent;

    public bool canDestroy = false;
    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering object is the player
        if (other.gameObject.CompareTag("Customer"))
        {
            OntriggerEvent?.Invoke();
            if (canDestroy)
            {
                Destroyobj(other.gameObject);
            }
        }
    }
    public void Destroyobj(GameObject g)
    {

        Destroy(g);
    }

}
