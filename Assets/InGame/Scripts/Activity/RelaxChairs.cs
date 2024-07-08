using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class RelaxChairs : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;

    [SerializeField] float relaxTime = 10f;
    [SerializeField] customer customer;

    [SerializeField] Transform SitInPosition;
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
        customer.Animator.ChangeState(AnimationState.Idle);
        customer = null;


    }


    void OnTriggerEnter(Collider collider)
    {
        if (!IsOccupied) return;

        if (collider.tag == "Customer")
        {

            TimerManager.Instance.ScheduleAction(relaxTime, Vacate);
            customer.transform.SetPositionAndRotation(SitInPosition.position, SitInPosition.rotation);
            customer.GetComponent<NavMeshAgent>().Warp(SitInPosition.position);
            customer.Animator.ChangeState(AnimationState.Sit);

        }
    }


}
