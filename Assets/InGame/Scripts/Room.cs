
using UnityEngine;

using UnityEngine.AI;
public class Room : MonoBehaviour
{
    public int RoomNumber;
    public Transform RoomDesitnation;
    public Transform Exit;
    [SerializeField] Transform SleepInPosition;



    public void SleepIn(GameObject C) // Assuming C has a NavMeshAgent component
    {
        // C.transform.SetParent(SleepInPosition);
        C.transform.SetPositionAndRotation(SleepInPosition.position, SleepInPosition.rotation);
        NavMeshAgent agent = C.GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("Customer GameObject must have a NavMeshAgent component.");
            return;
        }

        // Warp the agent to the desired position smoothly
        agent.Warp(SleepInPosition.position);
    }
    public void SleepOver(GameObject C) // call this function and set the customer Ontop of bed
    {
        C.transform.SetParent(null);
        NavMeshAgent agent = C.GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("Customer GameObject must have a NavMeshAgent component.");
            return;
        }

        // Warp the agent to the desired position smoothly
        agent.Warp(Exit.position);
    }
}
