using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnClick : MonoBehaviour
{
    Button button; // Assign this in the Inspector


    private void Start()
    {
        button = GetComponent<Button>();
        // Subscribe to the button's click event
        button.onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        VibrationManager.Vibrateit();
        // Call the method to play the sound
        SoundManager.Instance.PlaySoundButtonClick();
    }
}
