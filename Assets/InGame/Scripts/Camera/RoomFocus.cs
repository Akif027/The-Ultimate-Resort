using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFocus : MonoBehaviour
{
    [SerializeField] Transform RoomAngle;
    // public bool _canfocus = false;

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         _canfocus = !_canfocus; // Toggle _canfocus state when the player enters

    //         if (_canfocus)
    //         {

    //         }
    //         else
    //         {
    //             AllPurposeCamera.Instance.ResetCamera();
    //         }
    //     }
    // }



    public void FocusRoom()
    {

        AllPurposeCamera.Instance.ActivateCameraAtPosition(RoomAngle);

    }

    public void ResetRoomFocus()
    {

        AllPurposeCamera.Instance.ResetCamera();

    }
}
