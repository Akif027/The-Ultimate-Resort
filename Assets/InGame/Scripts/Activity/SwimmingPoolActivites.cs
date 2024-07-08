using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmingPoolActivites : MonoBehaviour
{
    public List<RelaxChairs> relaxChairs = new List<RelaxChairs>();
    public List<PoolPlay> Pool = new List<PoolPlay>();

    [SerializeField] QueueManager queueManager;

    void Start()
    {
        queueManager = GameManager.Instance.GetManager<QueueManager>() as QueueManager;
        EventManager.SubscribeSwimmingPoolRequest(HandleRequest);
    }
    void Update()
    {

        if (queueManager.customersPoolList.Count > 0)
        {
            foreach (GameObject c in queueManager.customersPoolList)
            {
                customer C_instance = c.GetComponent<customer>();
                HandleRequest(C_instance);
            }
        }

    }
    void HandleRequest(customer customerInstance)
    {
        PoolPlay p = AssignPool();
        RelaxChairs r = AssignRelaxChair();

        if (p != null)
        {
            AssignToDestination(customerInstance, p.gameObject, p);
        }
        else if (r != null)
        {
            AssignToDestination(customerInstance, r.gameObject, r);
        }
        else
        {
            Debug.LogError("No available pool or chair for assigning.");
        }
    }

    PoolPlay AssignPool()
    {
        foreach (var p in Pool)
        {
            if (!p.IsOccupied)
            {
                return p;
            }
        }
        return null;
    }

    RelaxChairs AssignRelaxChair()
    {
        foreach (var r in relaxChairs)
        {
            if (!r.IsOccupied)
            {
                return r;
            }
        }
        return null;
    }

    void AssignToDestination(customer customerInstance, GameObject destination, MonoBehaviour activity)
    {
        customerInstance.AssginDestination(destination.transform);
        customerInstance.SetDestination();

        if (activity is PoolPlay pool)
        {
            pool.Occupy(customerInstance);
        }
        else if (activity is RelaxChairs chair)
        {
            chair.Occupy(customerInstance);
        }

        queueManager.RemoveCustomerFromSwimmingPoolListAndQueue(customerInstance.gameObject);
    }

    void OnDestroy()
    {
        EventManager.UnsubscribeSwimmingPoolRequest(HandleRequest);
    }
}
