using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PoolPlay : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;

    [SerializeField] float playTime = 10f;
    [SerializeField] float swimDistance = 5f; // Distance for swimming forward and backward
    [SerializeField] customer customer;
    [SerializeField] Transform poolCenter; // Center of the pool area

    private Vector3 pointA;
    private Vector3 pointB;
    private bool movingToPointA = true;

    void Start()
    {
        // Ensure the pool center is assigned
        if (poolCenter == null)
        {
            Debug.LogError("Pool center is not assigned!");
        }

        // Define the two points for straight-line swimming
        pointA = poolCenter.position + poolCenter.forward * swimDistance;
        pointB = poolCenter.position - poolCenter.forward * swimDistance;
    }

    public void Occupy(customer c)
    {
        customer = c;
        IsOccupied = true;

        // Start the swimming routine

    }

    public void Vacate()
    {
        if (customer == null) return;
        IsOccupied = false;
        customer.ChangeState(CustomerState.Exiting);
        customer.Animator.ChangeState(AnimationState.Idle);
        customer = null;
        StopCoroutine(SwimAround()); // Stop the swimming routine when vacated
        SwimmingPoolActivites.CoustomerPlayCount++;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!IsOccupied) return;
        if (collider.tag == "Customer")
        {
            StartCoroutine(SwimAround());
            TimerManager.Instance.ScheduleAction(playTime, Vacate);

        }
    }

    IEnumerator SwimAround()
    {
        NavMeshAgent agent = customer.GetComponent<NavMeshAgent>();
        customer.Animator.ChangeState(AnimationState.Swim);
        while (IsOccupied)
        {
            SetStraightLineDestination(agent);
            yield return new WaitForSeconds(2f); // Adjust this to control how often the agent picks a new destination
        }
    }

    void SetStraightLineDestination(NavMeshAgent agent)
    {
        if (movingToPointA)
        {
            agent.SetDestination(pointA);
        }
        else
        {
            agent.SetDestination(pointB);
        }
        movingToPointA = !movingToPointA; // Toggle the destination
    }
}
