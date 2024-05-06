using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : GameManager
{
    public Dictionary<int, Room> RoomContainer;
    public List<Room> room = new List<Room>();

    public List<RoomData> roomDatas = new List<RoomData>();
    protected override void Initialize()
    {
        base.Initialize();
        RoomContainer = new Dictionary<int, Room>();

        InitializeRooms();

        //ToggleRoom(1, true);
        ToggleMultipleRooms(1, true);
    }
    private void InitializeRooms()
    {

        int roomNumber = 1;
        foreach (Room r in room)
        {
            r.gameObject.SetActive(false);
            r.RoomNumber = roomNumber; // Assign the current room number
            roomDatas.Add(new RoomData(roomNumber, r.gameObject));
            CleanAnim[] anim = r.GetComponentsInChildren<CleanAnim>();

            RoomContainer.Add(roomNumber, r);
            foreach (var a in anim)
            {
                roomDatas[roomNumber - 1].animationHolders.Add(new AnimationHolder(a));

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


    public void ToggleMultipleRooms(int numberOfRooms, bool enable)
    {
        if (RoomContainer.Count < numberOfRooms)
        {
            Debug.LogError("Not enough rooms to enable/disable.");
            return;
        }

        int roomNumber = 1; // Start with room number 1
        foreach (var roomEntry in RoomContainer)
        {
            if (roomNumber <= numberOfRooms)
            {
                roomEntry.Value.gameObject.SetActive(enable);
            }
            else
            {
                break; // Exit the loop once the specified number of rooms have been processed
            }
            roomNumber++;
        }
    }
}
[System.Serializable]
public class RoomData
{
    public string RoomName;
    public int RoomNumber;
    public GameObject RoomObj;

    public List<AnimationHolder> animationHolders;
    public RoomData(int Rno, GameObject Roomobj)
    {
        RoomNumber = Rno;
        RoomObj = Roomobj;
        animationHolders = new List<AnimationHolder>();
    }

    public void PlayAllCleanAnimation()
    {
        foreach (var a in animationHolders)
        {
            a.CleanAnimtion();
        }
    }

    public void PlayAllDirtyAnimation()
    {
        foreach (var a in animationHolders)
        {
            a.DirtyAnimtion();
        }
    }
}