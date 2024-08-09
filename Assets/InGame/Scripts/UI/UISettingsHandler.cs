using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UISettingsHandler : MonoBehaviour
{
    // Assign these in the Inspector
    public Button settingsButton;  // This will be the button to rotate
    public GameObject objectToMove;
    public Button soundOnButton;  // Button to show when sound is on
    public Button soundOffButton; // Button to show when sound is off
    public Button exitButton;     // Button to exit the application

    // Rotation and movement parameters
    public float rotationDuration = 1f;
    public float rotationAngle = 90f;
    public float moveDuration = 1f;
    public float moveDistance = 25f;

    private bool isToggled = false;  // Track the current state
    private bool isSoundOn = true;   // Track sound state

    void Start()
    {
        // Ensure button has an onClick listener
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(OnSettingsButtonClick);
        }

        // Ensure sound buttons have onClick listeners
        if (soundOnButton != null)
        {
            soundOnButton.onClick.AddListener(ToggleSound);
        }
        if (soundOffButton != null)
        {
            soundOffButton.onClick.AddListener(ToggleSound);
        }

        // Ensure exit button has an onClick listener
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitApplication);
        }

        // Load sound state from PlayerPrefs
        isSoundOn = PlayerPrefs.GetInt("SoundState", 1) == 1;
        UpdateSoundButtons();
    }

    void OnSettingsButtonClick()
    {
        if (isToggled)
        {
            // Rotate back to the original position
            settingsButton.transform.DORotate(new Vector3(0, 0, 0), rotationDuration, RotateMode.FastBeyond360)
                                  .SetEase(Ease.InOutSine);

            // Move the object back to its original position
            if (objectToMove != null)
            {
                objectToMove.transform.DOMoveX(objectToMove.transform.position.x - moveDistance, moveDuration)
                                      .SetEase(Ease.InOutSine);
            }
        }
        else
        {
            // Rotate to the specified angle
            settingsButton.transform.DORotate(new Vector3(0, 0, rotationAngle), rotationDuration, RotateMode.FastBeyond360)
                                  .SetEase(Ease.InOutSine);

            // Move the object to the specified position
            if (objectToMove != null)
            {
                objectToMove.transform.DOMoveX(objectToMove.transform.position.x + moveDistance, moveDuration)
                                      .SetEase(Ease.InOutSine);
            }
        }

        // Play button click sound
        if (isSoundOn)
        {
            SoundManager.Instance.PlaySoundButtonClick();
        }

        // Toggle the state
        isToggled = !isToggled;
    }

    void ToggleSound()
    {
        // Toggle sound state
        isSoundOn = !isSoundOn;

        // Save sound state to PlayerPrefs
        PlayerPrefs.SetInt("SoundState", isSoundOn ? 1 : 0);

        // Update audio settings
        AudioListener.volume = isSoundOn ? 1 : 0;

        // Update button visibility
        UpdateSoundButtons();
    }

    void UpdateSoundButtons()
    {
        if (isSoundOn)
        {
            soundOnButton.gameObject.SetActive(true);
            soundOffButton.gameObject.SetActive(false);
        }
        else
        {
            soundOnButton.gameObject.SetActive(false);
            soundOffButton.gameObject.SetActive(true);
        }
    }

    void ExitApplication()
    {
        Application.Quit();

        // If running in the editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
