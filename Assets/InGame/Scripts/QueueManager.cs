using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class QueueManager : MonoBehaviour
{
    public Transform[] points;
    //public Transform returnPoint;
    public List<NavMeshAgent> availableAgents = new List<NavMeshAgent>();

    public Transform getPoint()
    {
        //if empty assing.

        //count of ava
        return points[availableAgents.Count];
    }

}