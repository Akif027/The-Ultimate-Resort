using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : Manager
{
    public static RoomManager instance;
    public Dictionary<int, Room> RoomContainer;
    public List<Room> room = new List<Room>();

    public List<RoomData> roomData = new List<RoomData>();

    [SerializeField] UIManager uIManager;


    public static RoomManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Find the existing instance in the scene
                instance = FindObjectOfType<RoomManager>();

                // If not found, create a new instance
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    instance = obj.AddComponent<RoomManager>();
                }
            }
            return instance;
        }
    }


    public override void Init()
    {

        if (instance == null)
        {
            instance = this;
            //   Don't DestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        RoomContainer = new Dictionary<int, Room>();

        InitializeRooms();

        // ToggleRoom(1, true);
        ToggleMultipleRooms(UpgradeManager.Instance.UpgradeDataValues().Rooms, true);
    }


    private void InitializeRooms()
    {

        int roomNumber = 1;
        foreach (Room r in room)
        {
            r.gameObject.SetActive(false);
            r.RoomNumber = roomNumber; // Assign the current room number
            roomData.Add(new RoomData(roomNumber, r));
            CleanAnim[] anim = r.GetComponentsInChildren<CleanAnim>();

            RoomContainer.Add(roomNumber, r);
            foreach (var a in anim)
            {
                roomData[roomNumber - 1].animationHolders.Add(new AnimationHolder(a));
            }

            roomNumber++; // Increment the room number for the next room
        }


    }


    public void ToggleRoom(int roomNumber, bool activate)
    {
        if (RoomContainer.ContainsKey(roomNumber))
        {
            Room roomToToggle = RoomContainer[roomNumber];
            if (activate)
            {
                // Assuming your Room class has an Activate method
                roomToToggle.gameObject.SetActive(true);
            }
            else
            {
                // Assuming your Room class has a Deactivate method
                roomToToggle.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError($"Room number {roomNumber} not found.");
        }
    }

    public RoomData FindRoomData(int roomNumber)
    {
        foreach (var data in roomData)
        {
            if (data.RoomNumber == roomNumber)
                return data;
        }
        return null;
    }
    public bool HasAvailableRooms()
    {
        foreach (var roomData in roomData)
        {
            if (!roomData.isAllot && roomData.isClean)
            {
                return true;
            }
        }
        return false;
    }

    public RoomData AssignRoom()
    {
        foreach (var roomData in roomData)
        {
            if (!roomData.isAllot && roomData.isClean)
            {
                roomData.isAllot = true; // Mark the room as allocated
                return roomData;
            }
        }
        return null; // Return null if no rooms are available
    }
    public void FreeRoom(int roomNumber)
    {
        var roomData_ = roomData.Find(r => r.RoomNumber == roomNumber);
        if (roomData_ != null)
        {
            roomData_.isAllot = false; // Mark the room as free
            // Optionally, set the room to dirty or clean based on game logic
            roomData_.isClean = false; // Assuming the room becomes dirty after use
        }
    }
    public void ToggleMultipleRooms(int numberOfRooms, bool enable)
    {
        int enabledRooms = 0; // Counter for tracking the number of rooms enabled/disabled

        // Iterate through all room data entries
        foreach (var roomData in roomData)
        {
            // Check if the room meets any specific criteria before enabling/disabling
            // For example, you might check if the room is clean and not currently allotted
            if (!roomData.isAllot && roomData.isClean)
            {
                // Find the corresponding Room object in RoomContainer using RoomNumber
                if (RoomContainer.TryGetValue(roomData.RoomNumber, out Room room))
                {
                    room.gameObject.SetActive(enable);
                    enabledRooms++;

                    // Break the loop if the target number of rooms has been enabled/disabled
                    if (enabledRooms >= numberOfRooms)
                        break;
                }
            }
        }

        // Log an error if not enough eligible rooms were found
        if (enabledRooms < numberOfRooms)
        {
            Debug.LogError($"Not enough eligible rooms to enable/disable. Found {enabledRooms}, but requested {numberOfRooms}.");
        }
    }

}
[System.Serializable]
public class RoomData
{
    public string RoomName;
    public int RoomNumber;
    public Room room;
    public bool isClean;
    public bool isAllot;


    public List<AnimationHolder> animationHolders;
    public RoomData(int Rno, Room _room)
    {
        RoomNumber = Rno;
        isAllot = false;
        isClean = true;
        room = _room;
        animationHolders = new List<AnimationHolder>();
    }

    public void PlayAllCleanAnimation()
    {
        foreach (var a in animationHolders)
        {
            a.CleanAnimation();
        }
        isClean = true;
    }

    public void PlayAllDirtyAnimation()
    {
        foreach (var a in animationHolders)
        {
            a.DirtyAnimation();
        }
        isClean = false;
    }
}