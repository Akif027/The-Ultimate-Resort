
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

using UnityEngine.AI;
public class Room : MonoBehaviour
{
    public int RoomNumber;
    public Transform RoomDesitnation;
    public Transform Exit;
    [SerializeField] Transform SleepInPosition;

    [SerializeField] Transform SleepingRoofPos;

    [SerializeField] GameObject Rooflayer;
    [SerializeField] UIManager uIManager;
    [SerializeField] CleanAnim[] cleanAnim;
    [SerializeField]
    RoomDescript RoomState = RoomDescript.RoomEmpty;

    RoomManager roomManager;

    RoomData roomData;


    void Start()
    {
        roomManager = GetComponentInParent<RoomManager>();
        roomData = roomManager.FindRoomData(RoomNumber);
        uIManager = GameManager.Instance.GetManager<UIManager>() as UIManager;
        Rooflayer = uIManager.GetUIElement<GameObject>("roomRoof" + RoomNumber);
        Rooflayer.transform.SetPositionAndRotation(SleepingRoofPos.position, SleepingRoofPos.rotation);
        Rooflayer.SetActive(false);

    }


    void Update()
    {

        switch (RoomState)
        {

            case RoomDescript.CheckIn:
                RoomCheckIn();
                break;
            case RoomDescript.CheckOut:
                RoomCheckOut();
                break;

        }


    }

    public void RoomCheckIn()
    {


    }
    bool CallOnce = false;
    public void RoomCheckOut()
    {
        if (!CallOnce)
        {

            RoomState = RoomDescript.RoomEmpty;
            foreach (CleanAnim c in cleanAnim)
            {
                c.PlaceCleaningSign();
            }
            roomData.PlayAllDirtyAnimation();
            //  CallOnce = true;

        }


    }
    public void SleepIn(GameObject C) // Assuming C has a NavMeshAgent component
    {
        // C.transform.SetParent(SleepInPosition);

        C.transform.SetPositionAndRotation(SleepInPosition.position, SleepInPosition.rotation);
        NavMeshAgent agent = C.GetComponent<NavMeshAgent>();
        Rooflayer.SetActive(true);
        if (agent == null)
        {
            Debug.LogError("Customer GameObject must have a NavMeshAgent component.");
            return;
        }

        // Warp the agent to the desired position smoothly
        agent.Warp(SleepInPosition.position);
    }
    public void SleepOver(GameObject C) // call this function and set the customer Ontop of bed
    {

        C.transform.SetParent(null);
        NavMeshAgent agent = C.GetComponent<NavMeshAgent>();
        Rooflayer.SetActive(false);
        if (agent == null)
        {
            Debug.LogError("Customer GameObject must have a NavMeshAgent component.");
            return;
        }

        // Warp the agent to the desired position smoothly
        agent.Warp(Exit.position);
        RoomState = RoomDescript.CheckOut;
    }

    public void FreeRoomToEmtyOnclean()
    {
        if (IsEverythingCleaned())
        {
            roomManager.FreeRoom(RoomNumber);
        }
    }
    public bool IsEverythingCleaned()
    {
        foreach (var cleanAnim in cleanAnim)
        {
            if (!cleanAnim.IsCleaningComplete)
            {
                return false; // Not everything is cleaned yet
            }
        }
        return true; // Everything is cleaned
    }
    public void ChangeState(RoomDescript newState)
    {
        RoomState = newState;
    }
}


public enum RoomDescript
{
    RoomEmpty,
    CheckIn,
    CheckOut

}