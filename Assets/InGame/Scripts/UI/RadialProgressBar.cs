
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class RadialProgressBar : MonoBehaviour
{
    public bool isActive = false;


    private float indicatorTimer;
    private float maxIndicatorTimer;

    [SerializeField] Image radialProgressBar;

    public UnityEvent OnComplete;

    private bool canProceed = true; // Add this flag to control the countdown

    void Update()
    {
        if (isActive && canProceed) // Only decrement the timer if isActive and canProceed is true
        {

            indicatorTimer -= Time.deltaTime;
            radialProgressBar.fillAmount = (indicatorTimer / maxIndicatorTimer);
            if (indicatorTimer <= 0)
            {
                StopCountdown();
                OnComplete?.Invoke();
            }
        }
    }

    public void PauseCountdown()
    {
        canProceed = false;
    }

    public void ResumeCountdown()
    {
        canProceed = true;
    }
    public void ActivateCountDown(float countdownTime)
    {

        isActive = true;
        maxIndicatorTimer = countdownTime;
        indicatorTimer = maxIndicatorTimer;
    }

    public void StopCountdown()
    {

        isActive = false;
        ObjectPool.Instance.ReturnObjectToPool(gameObject, "CleaningProgress");

    }


}
