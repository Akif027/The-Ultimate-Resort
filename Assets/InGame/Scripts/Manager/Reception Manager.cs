using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReceptionManager : MonoBehaviour
{

   [SerializeField] QueueManager queueManager;
   [SerializeField] RoomManager roomManager;
   void Start()
   {

      EventManager.Subscribe(HandleRoomRequest);
      queueManager = GameManager.Instance.GetManager<QueueManager>() as QueueManager;
      roomManager = GameManager.Instance.GetManager<RoomManager>() as RoomManager;

   }
   void HandleRoomRequest(GameObject customerObj)
   {


      if (roomManager.HasAvailableRooms())
      {
         Debug.Log($"{queueManager.customerQueue.Count} Customers is requesting room");
         RoomData assignedRoom = roomManager.AssignRoom();
         if (assignedRoom != null)
         {
            Debug.Log("room assigned");
            customer c = customerObj.GetComponent<customer>();
            if (c != null)
            {
               c.roomDestination = assignedRoom.room.RoomDesitnation;
               c.ChangeState(CustomerState.MovingToRoom);
               queueManager.RemoveCustomerFromQueue(customerObj);

            }


         }

      }
      else
      {
         // Add the customer to a waiting queue
         //   queueManager.customerQueue.Enqueue(customerObj);

      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         Debug.Log("Player arrived at reception.");
         // Call a method to handle the reception interaction
         HandleReceptionInteraction();
      }
   }
   private void HandleReceptionInteraction() // will invoke a button when click assign the room to the customer
   {
      // Implementation of the interaction logic goes here
   }
   void OnDestroy()
   {
      EventManager.Unsubscribe(HandleRoomRequest);
   }
}
