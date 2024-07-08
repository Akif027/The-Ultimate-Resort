using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelaxChairs : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;

    [SerializeField] float relaxTime = 10f;
    [SerializeField] customer customer;
    public void Occupy(customer c)
    {
        customer = c;
        IsOccupied = true;

    }

    public void Vacate()
    {
        if (customer == null) return;
        IsOccupied = false;
        customer.ChangeState(CustomerState.Exiting);
        customer = null;
    }


    void OnTriggerEnter(Collider collider)
    {
        if (!IsOccupied) return;

        if (collider.tag == "Customer")
        {

            TimerManager.Instance.ScheduleAction(relaxTime, Vacate);
        }
    }


}
