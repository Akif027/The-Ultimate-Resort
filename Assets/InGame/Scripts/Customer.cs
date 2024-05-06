using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : GameManager
{
    // Customer queue ID
    [SerializeField]
    int id;

    // Room alloted to the customer
    [SerializeField]
    GameObject room;

    /**
    Room
    Grade = 'A' Charges = 200
    Grade = 'B' Charges = 500
    Grade = 'A' Charges = 1000
    **/

    [SerializeField]
    char grade;

    [SerializeField] NavMeshAgent navMeshAgent;


    protected override void Initialize()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

    }
    public void Room_Allot(GameObject room, char grade)
    {
        navMeshAgent.SetDestination(room.transform.position);
    }

    //ontriiger bed
}
