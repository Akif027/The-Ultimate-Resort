using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SwimmingPoolActivites : MonoBehaviour
{
    public List<RelaxChairs> relaxChairs = new List<RelaxChairs>();
    public List<PoolPlay> Pool = new List<PoolPlay>();
    public Material targetMaterial;

    [SerializeField] Color Dirty;
    [SerializeField] Color Clean;
    [SerializeField] QueueManager queueManager;
    [SerializeField] UnityEvent OnDirty;

    public static int CoustomerPlayCount;

    private bool isDirty = false;

    private bool notHandlingRequest = false;
    void Start()
    {
        CoustomerPlayCount = 0;
        ChangeColor(Clean);
        queueManager = GameManager.Instance.GetManager<QueueManager>() as QueueManager;
        EventManager.SubscribeSwimmingPoolRequest(HandleRequest);
    }
    void Update()
    {

        if (queueManager.customersPoolList.Count > 0)
        {
            if (CoustomerPlayCount >= 3)
            {
                return;
            }
            foreach (GameObject c in queueManager.customersPoolList)
            {
                customer C_instance = c.GetComponent<customer>();
                HandleRequest(C_instance);
            }
        }

        if (CoustomerPlayCount >= 3) //make it dirty
        {


            if (!isDirty)
            {
                DirtyPool();
                OnDirty?.Invoke();
                isDirty = true;
            }
        }

    }
    public void DirtyPool() { ChangeColor(Dirty); }

    public void CleanPool() { ChangeColor(Clean); CoustomerPlayCount = 0; }
    void HandleRequest(customer customerInstance)
    {

        if (CoustomerPlayCount >= 3)
        {
            return;
        }
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
    public void ChangeColor(Color newColor)
    {
        if (targetMaterial != null)
        {
            targetMaterial.color = newColor;
        }
        else
        {
            Debug.LogError("Target material is not assigned.");
        }
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
