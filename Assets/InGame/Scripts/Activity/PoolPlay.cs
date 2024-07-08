using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PoolPlay : MonoBehaviour
{
    public bool IsOccupied { get; private set; } = false;

    [SerializeField] float playTime = 10f;
    [SerializeField] float swimRadius = 5f;
    [SerializeField] customer customer;
    [SerializeField] Transform poolCenter; // Center of the pool area

    void Start()
    {
        // Ensure the pool center is assigned
        if (poolCenter == null)
        {
            Debug.LogError("Pool center is not assigned!");
        }
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

    }

    void OnTriggerEnter(Collider collider)
    {
        if (!IsOccupied) return;
        if (collider.tag == "Customer")
        {
            TimerManager.Instance.ScheduleAction(playTime, Vacate);
            StartCoroutine(SwimAround());

        }
    }

    IEnumerator SwimAround()
    {
        NavMeshAgent agent = customer.GetComponent<NavMeshAgent>();
        customer.Animator.ChangeState(AnimationState.Swim);
        while (IsOccupied)
        {
            SetRandomDestination(agent);
            yield return new WaitForSeconds(2f); // Adjust this to control how often the agent picks a new destination
        }
    }

    void SetRandomDestination(NavMeshAgent agent)
    {
        Vector3 randomDirection = Random.insideUnitSphere * swimRadius;
        randomDirection += poolCenter.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, swimRadius, 1))
        {
            agent.SetDestination(hit.position);
        }
    }
}
