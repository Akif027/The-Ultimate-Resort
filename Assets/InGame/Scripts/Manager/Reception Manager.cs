using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceptionManager : GameManager
{


    RoomData roomAlloted = null;
    public GameObject Customer = null;

    protected override void OnTriggerEnter(Collider other)
    {

        if (other.tag == "customer")
        {
            //  StartCoroutine(waitRoom(other.gameObject));
            Customer = other.gameObject;

        }
    }

    public void AllocateRoom()
    {
        StartCoroutine(waitRoom(Customer));

    }
    RoomData Available_Room()
    {
        foreach (var room in RoomManager.instance.roomData)
        {
            if (room.isClean == true && room.isAllot == false)
            {
                return room;
            }
        }
        return null;
    }

    IEnumerator waitRoom(GameObject customer)
    {
        while (roomAlloted == null)
        {
            yield return new WaitForSeconds(1);
            roomAlloted = Available_Room();
        }
        customer.GetComponent<Customer>().Room_Allot(roomAlloted);
    }
}
