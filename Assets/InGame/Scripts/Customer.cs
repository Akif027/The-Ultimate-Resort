using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UltimateResort;
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
        //Inform Next customer in queue
        Boarding.Instance.NextCustomer();
    }

    public void Room_reached()
    {
        StartCoroutine(waitTimer(RoomManager.instance.roomWaitTime, Room_checkOut));
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


    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "endPoint")
        {
            Boarding.Instance.CustomerDeparture();
            Destroy(this.gameObject);
        }
    }
}
