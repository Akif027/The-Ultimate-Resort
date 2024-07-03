using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AI;
public class customer : MonoBehaviour
{


   public string customerId; // Unique identifier for each custome
   public bool RoomAllotted = false;

   [SerializeField] Animation animator;
   [SerializeField] float Sleepingtime = 5f;


   [SerializeField]
   private float probExitingRoom = 0.9f; // Adjusted to half since there are only two states
   [SerializeField]
   private float probGoingToilet = 0.3f; // Adjusted to half since there are only two states
   [SerializeField]
   private float totalProbability = 1.0f; // Sum of the two probabilities


   [SerializeField] CustomerState currentState = CustomerState.waiting;
   private NavMeshAgent agent; // Reference to the NavMeshAgent component
   private RoomData roomData;
   private Transform roomDestination = null;




   #region Unity
   void Start()
   {

      ChangeState(CustomerState.waiting);
      // Initialize the NavMeshAgent
      agent = GetComponent<NavMeshAgent>();
      animator = GetComponent<Animation>();

   }

   void Update()
   {

      if (agent.hasPath && agent.remainingDistance > agent.stoppingDistance + 0.1f)
      {

         // The customer is moving, play the walking animation
         animator.AnimationPlay("isWalking", true);

      }
      else
      {

         animator.AnimationPlay("isWalking", false);

         // The customer is idle or not moving significantly, stop the walking animation

      }
      switch (currentState)
      {

         case CustomerState.MovingToRoom:
            MoveToRoom();
            break;
         case CustomerState.Sleeping:
            Sleep();
            break;
         case CustomerState.ExitingRoom:
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

   }

   #endregion

   #region Customer Behaviour logic
   private void MoveToRoom()
   {
      RoomAllotted = true;
      agent.destination = roomDestination.position;

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
      animator.AnimationPlay("Idle", true);

      RandomizeState();
   }
   private void ExitRoom()
   {

      agent.destination = DestinationManager.Instance.GetDestination("Exit").position;

   }
   private void GoToilet()
   {
      agent.destination = DestinationManager.Instance.GetDestination("Toilet").position;

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

      if (randomValue <= probExitingRoom) ChangeState(CustomerState.ExitingRoom);
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

   #endregion
   public void AssginRoomData(RoomData _roomData)
   {

      roomData = _roomData;
   }


   public void AssginRoomDestination(Transform _roomDestination)
   {

      roomDestination = _roomDestination;
   }



}
public enum CustomerState
{
   waiting,
   MovingToRoom,
   Sleeping,
   ExitingRoom,
   GoingToilet
}
