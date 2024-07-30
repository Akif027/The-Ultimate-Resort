using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ReceptionManager : MonoBehaviour
{
   // Managers
   [Header("Managers")]
   [SerializeField] private QueueManager queueManager;
   [SerializeField] private RoomManager roomManager;
   [SerializeField] private UIManager uIManager;

   // Customer related
   [Header("Customer Related")]
   [SerializeField] private GameObject waitingCustomerObj;
   [SerializeField] private MoneyPlace moneyPlace;
   [SerializeField] private int handOutKeyTime = 2;
   public int CustomerHandled = 0;

   // UI elements
   [Header("UI Elements")]
   public Button acceptCustomerButton;
   private RadialProgressBar radialProgressBar;

   // Initialization
   void Start()
   {
      // Subscribe to events
      EventManager.SubscribeRoomRequest(HandleRoomRequest);

      // Initialize managers
      queueManager = GameManager.Instance.GetManager<QueueManager>() as QueueManager;
      roomManager = GameManager.Instance.GetManager<RoomManager>() as RoomManager;
      uIManager = GameManager.Instance.GetManager<UIManager>() as UIManager;

      // Initialize UI elements
      acceptCustomerButton = uIManager.GetUIElement<GameObject>("Key").GetComponent<Button>();
      uIManager.HideUIElement("Key");
      radialProgressBar = acceptCustomerButton.gameObject.GetComponent<RadialProgressBar>();
   }

   // Event Handlers
   void HandleRoomRequest(GameObject customerObj)
   {
      waitingCustomerObj = customerObj;
      acceptCustomerButton.onClick.AddListener(AssignRoomToCustomer);
   }

   private void AssignRoomToCustomer()
   {
      if (roomManager.HasAvailableRooms() && waitingCustomerObj != null)
      {
         customer c = waitingCustomerObj.GetComponent<customer>();
         if (c.RoomAllotted && c == null)
         {
            return;
         }
         Debug.Log($"{queueManager.customerQueue.Count} Customers are requesting room");
         radialProgressBar.ActivateCountDown(handOutKeyTime);
         radialProgressBar?.OnComplete.RemoveAllListeners();
         radialProgressBar?.OnComplete.AddListener(Assign);
      }
      else if (!roomManager.HasAvailableRooms())
      {
         Debug.LogWarning("NO ROOM AVAILABLE!");
      }
   }

   public void Assign()
   {
      RoomData assignedRoom = roomManager.AssignRoom();
      customer c = waitingCustomerObj.GetComponent<customer>();
      if (assignedRoom != null)
      {
         assignedRoom.room.ChangeState(RoomDescript.CheckIn);
         Debug.Log("Room assigned");

         if (c != null)
         {
            c.AssginDestination(assignedRoom.room.RoomDesitnation);
            c.AssginRoomData(assignedRoom);
            c.ChangeState(CustomerState.MovingToRoom);
            queueManager.RemoveCustomerFromQueue(waitingCustomerObj);
            waitingCustomerObj = null;
            moneyPlace.PlaceMoney();
            CustomerHandled++;
         }
      }
   }

   // Trigger Handlers
   void OnTriggerStay(Collider other)
   {
      // Check if the other object has a specific tag
      if (other.CompareTag("Player"))
      {
         // Perform actions while the player is inside the trigger
         if (waitingCustomerObj != null && !waitingCustomerObj.GetComponent<customer>().RoomAllotted)
         {
            uIManager.ShowUIElement("Key");
         }
         else
         {
            uIManager.HideUIElement("Key");
         }
      }
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         uIManager.HideUIElement("Key");
      }
   }

   // Cleanup
   void OnDestroy()
   {
      EventManager.UnsubscribeRoomRequest(HandleRoomRequest);
   }
}
