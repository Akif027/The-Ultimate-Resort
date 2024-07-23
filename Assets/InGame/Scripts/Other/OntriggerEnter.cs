
using UnityEngine;
using UnityEngine.Events;

public class OntriggerEnter : MonoBehaviour
{
    public UnityEvent OntriggerEvent;


    public bool _CustomerReturnToPool = false;

    public bool _TriggerPlayer = false;
    private void OnTriggerEnter(Collider other)
    {
        if (_CustomerReturnToPool)
        {

            // Check if the entering object is the player
            if (other.gameObject.CompareTag("Customer"))
            {
                OntriggerEvent?.Invoke();
                if (_CustomerReturnToPool)
                {
                    ObjectPool.Instance.ReturnObjectToPool(other.gameObject, "Customer");
                }
            }
        }


        if (_TriggerPlayer)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                OntriggerEvent?.Invoke();

            }


        }
    }



    public void Destroyobj(GameObject g)
    {

        Destroy(g);
    }

}
