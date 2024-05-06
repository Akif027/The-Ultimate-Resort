using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceptionManager : GameManager
{


    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "customer")
        {
            other.gameObject.GetComponent<Customer>().Room_Allot(Available_Room());
        }
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
}
