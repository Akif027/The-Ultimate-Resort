using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AI;
public class customer : MonoBehaviour
{


   public string customerId; // Unique identifier for each custome
   public bool RoomAllotted = false;
   public bool ToiletAllocated = false;
   [SerializeField] Animation animator;
   [SerializeField] float Sleepingtime = 5f;


   [SerializeField]
   private float probExitingRoom = 0.2f; // Adjusted to half since there are only two states
   [SerializeField]
   private float probGoingToilet = 0.9f; // Adjusted to half since there are only two states
   [SerializeField]
   private float totalProbability = 1.0f; // Sum of the two probabilities


   [SerializeField] CustomerState currentState = CustomerState.waiting;
   private NavMeshAgent agent; // Reference to the NavMeshAgent component
   private RoomData roomData;
   private QueueManager queueManager;
   public Transform NextDestination = null;




   #region Unity
   void Start()
   {
      queueManager = GameManager.Instance.GetManager<QueueManager>() as QueueManager;
      ChangeState(CustomerState.waiting);
      // Initialize the NavMeshAgent
      agent = GetComponent<NavMeshAgent>();
      animator = GetComponent<Animation>();

   }
   private void CustomerAnimationWalkAndIdle()
   {

      if (agent.hasPath && agent.remainingDistance > agent.stoppingDistance + 0.1f)
      {
         if (currentState == CustomerState.GoingToilet)
         {
            animator.AnimationPlay("Walktoilet", true);
            animator.AnimationPlay("ToiletIdle", false);


         }
         else
         {
            animator.AnimationPlay("ToiletIdle", false);
            animator.AnimationPlay("Walktoilet", false);
            animator.AnimationPlay("isWalking", true);
         }


      }
      else
      {
         if (currentState == CustomerState.GoingToilet)
         {

            animator.AnimationPlay("ToiletIdle", true);
            animator.AnimationPlay("Walktoilet", false);

         }
         else
         {

            animator.AnimationPlay("isWalking", false);
         }




      }
   }
   void Update()
   {

      CustomerAnimationWalkAndIdle();
      switch (currentState)
      {

         case CustomerState.MovingToRoom:
            MoveToRoom();
            break;
         case CustomerState.Sleeping:
            Sleep();
            break;
         case CustomerState.Exiting:
            ExitRoom();
            break;
         case CustomerState.GoingToilet:
            GoToilet();
            break;
      }
   }


   void OnTriggerEnter(Collider collider)
   {
      if (collider.tag == "Reception")
      {
         RequestRoom();

         Debug.Log("room Requested");
      }

      if (collider.tag == "ToBed")
      {
         ChangeState(CustomerState.Sleeping);
      }
      // if (collider.tag == "Toilet")
      // {
      //    RequestToilet();
      // }
   }

   #endregion

   #region Customer Behaviour logic
   private void MoveToRoom()
   {
      RoomAllotted = true;
      agent.destination = NextDestination.position;

   }

   private void Sleep()
   {
      roomData.room.SleepIn(gameObject);
      animator.AnimationPlay("Sleeping");
      animator.AnimationPlay("isWalking", false);

      TimerManager.Instance?.ScheduleAction(Sleepingtime, WakeUp);
   }

   private void WakeUp()
   {

      roomData.room.SleepOver(gameObject);
      // animator.AnimationPlay("Idle", true);
      ChangeState(CustomerState.GoingToilet);
      // RandomizeState();
   }
   private void ExitRoom()
   {

      agent.destination = DestinationManager.Instance.GetDestination("Exit").position;

   }
   private bool hasBeenAddedToToiletQueue = false;
   private void GoToilet()
   {


      if (!hasBeenAddedToToiletQueue)
      {
         queueManager.AddCustomerToToiletQueueAndList(gameObject);
         RequestToilet();
         hasBeenAddedToToiletQueue = true; // Set the flag to true after adding the customer to the queue
      }



   }

   private bool hasRandomizedState = false;
   public void RandomizeState()
   {
      if (hasRandomizedState)
      {
         // Prevent re-entry
         return;
      }

      // For example:
      float randomValue = UnityEngine.Random.value * totalProbability;

      if (randomValue <= probExitingRoom) ChangeState(CustomerState.Exiting);
      else ChangeState(CustomerState.GoingToilet);

      // Mark as done
      hasRandomizedState = true;
   }


   public void ChangeState(CustomerState newState)
   {
      currentState = newState;
   }
   void RequestRoom()
   {
      EventManager.InvokeRoomRequested(gameObject);
   }
   void RequestToilet()
   {
      EventManager.InvokeToiletRequested(this);
   }

   #endregion
   public void AssginRoomData(RoomData _roomData)
   {

      roomData = _roomData;
   }

   public void SetDestination()
   {

      agent.destination = NextDestination.position;
   }

   public void AssginDestination(Transform _destination)
   {

      NextDestination = _destination;
   }



}
public enum CustomerState
{
   waiting,
   MovingToRoom,
   Sleeping,
   Exiting,
   GoingToilet
}
