using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : GameManager
{
    public int RoomNumber;


    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "customer")
        {
            other.gameObject.GetComponent<Customer>().Room_reached();
        }
    }
}
