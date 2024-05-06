using System;
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
    RoomData room = null;

    /**
    Room
    Grade = 'A' Charges = 200
    Grade = 'B' Charges = 500
    Grade = 'A' Charges = 1000
    **/

    [SerializeField]
    char grade;


    [SerializeField] NavMeshAgent navMeshAgent;

    [SerializeField] bool isRoomReached = false;
    protected override void Initialize()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

    }
    public void Room_Allot(RoomData room)
    {
        this.room = room;
        grade = room.grade;
        room.isAllot = true;
        navMeshAgent.SetDestination(room.RoomDoor.transform.position);
        Debug.Log("Room Alloted");
    }

    protected override void UpdateGame()
    {
        //checking if the customer get the room or not
        if (room != null && isRoomReached != true)
        {
            //check if the customer reached the room or not
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("Room reached");
                    isRoomReached = true;
                    StartCoroutine(waitTimer(RoomManager.instance.roomWaitTime, Room_checkOut));

                }
            }
        }
    }

    void Room_checkOut()
    {
        Debug.Log("Room checkout");
        room.isAllot = false;
        room.PlayAllDirtyAnimation();
        navMeshAgent.SetDestination(RoomManager.instance.EndPoint.transform.position);
    }

    IEnumerator waitTimer(float time, Action action)
    {
        Debug.Log("Wait in room");
        yield return new WaitForSeconds(time);
        action();
    }
}
