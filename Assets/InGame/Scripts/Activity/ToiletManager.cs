using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletManager : MonoBehaviour
{


    public List<Toilet> toilets = new List<Toilet>();

    [SerializeField] QueueManager queueManager;

    void Start()
    {
        queueManager = GameManager.Instance.GetManager<QueueManager>() as QueueManager;
        EventManager.SubscribeToiletRequest(HandleToiletRequest);
    }


    void Update()
    {

        if (queueManager.customersToiletList.Count > 0 && AssignToilet() != null)
        {
            foreach (GameObject c in queueManager.customersToiletList)
            {
                customer C_instance = c.GetComponent<customer>();
                HandleToiletRequest(C_instance);
            }
        }

    }
    void HandleToiletRequest(customer customerInstance)
    {
        Toilet t = AssignToilet();
        if (t == null)
        {

            return;
        }

        if (customerInstance != null)
        {
            customerInstance.AssginDestination(t.gameObject.transform);
            customerInstance.SetDestination();
            t.Occupy(customerInstance);
            queueManager.RemoveCustomerFromToiletQueue(customerInstance.gameObject);
            //   TimerManager.Instance.ScheduleAction(toiletTime, () => ReleaseToilet(t, customerInstance));
            //  Debug.LogError("working");
        }


    }
    public Toilet AssignToilet()
    {
        foreach (var toilet in toilets)
        {
            if (!toilet.IsOccupied)
            {
                //  toilet.Occupy();
                return toilet;
            }
        }
        return null; // No available toilets
    }

    public void ReleaseToilet(Toilet toilet, customer c)
    {
        toilet.Vacate();
        c.ChangeState(CustomerState.Exiting);
    }


    void OnDestroy()
    {
        EventManager.UnsubscribeToiletRequest(HandleToiletRequest);
    }
}
