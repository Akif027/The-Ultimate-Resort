using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceptionManager : GameManager
{


    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "customer")
        {
            RoomData room = Available_Room();
            other.gameObject.GetComponent<Customer>().Room_Allot(room.RoomObj, room.grade);
        }
    }

    RoomData Available_Room()
    {
        foreach (var room in RoomManager.instance.roomDatas)
        {
            if (room.isClean == true && room.isAllot == false)
            {
                return room;
            }
        }
        return null;
    }
}
