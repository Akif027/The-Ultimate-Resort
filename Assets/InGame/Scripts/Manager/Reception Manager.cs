using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ReceptionManager : MonoBehaviour
{

   [SerializeField] QueueManager queueManager;
   [SerializeField] RoomManager roomManager;
   [SerializeField] UIManager uIManager;
   [SerializeField] GameObject WaitingCustomerObj;

   public Button AcceptCustomerB;
   void Start()
   {

      EventManager.SubscribeRoomRequest(HandleRoomRequest);
      queueManager = GameManager.Instance.GetManager<QueueManager>() as QueueManager;
      roomManager = GameManager.Instance.GetManager<RoomManager>() as RoomManager;
      uIManager = GameManager.Instance.GetManager<UIManager>() as UIManager;
      AcceptCustomerB = uIManager.GetUIElement<GameObject>("AcceptCustomerB").GetComponent<Button>();
      uIManager.HideUIElement("AcceptCustomerB");


   }
   void HandleRoomRequest(GameObject customerObj)
   {
      WaitingCustomerObj = customerObj;
      AcceptCustomerB.onClick.AddListener(AssignRoomToCustomer);


   }
   private void AssignRoomToCustomer()
   {
      if (roomManager.HasAvailableRooms())
      {
         customer c = WaitingCustomerObj.GetComponent<customer>();
         if (c.RoomAllotted && c == null)
         {
            return;
         }
         Debug.Log($"{queueManager.customerQueue.Count} Customers is requesting room");
         RoomData assignedRoom = roomManager.AssignRoom();
         if (assignedRoom != null)
         {
            assignedRoom.room.ChangeState(RoomDescript.CheckIn);
            Debug.Log("room assigned");

            if (c != null)
            {
               c.AssginDestination(assignedRoom.room.RoomDesitnation);
               c.AssginRoomData(assignedRoom);
               c.ChangeState(CustomerState.MovingToRoom);
               queueManager.RemoveCustomerFromQueue(WaitingCustomerObj);
               WaitingCustomerObj = null;

            }

         }

      }
      else
      {
         Debug.LogError("NO ROOM AVAILABLE!");

      }
   }
   void OnTriggerStay(Collider other)
   {
      // Check if the other object has a specific tag
      if (other.CompareTag("Player"))
      {
         // Perform actions while the player is inside the trigger
         if (WaitingCustomerObj != null && !WaitingCustomerObj.GetComponent<customer>().RoomAllotted)
         {

            uIManager.ShowUIElement("AcceptCustomerB");
         }
         else
         {

            uIManager.HideUIElement("AcceptCustomerB");
         }

      }
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         uIManager.HideUIElement("AcceptCustomerB");
      }
   }

   void OnDestroy()
   {
      EventManager.UnsubscribeRoomRequest(HandleRoomRequest);
   }


}
