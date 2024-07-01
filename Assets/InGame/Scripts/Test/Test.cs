using System.Collections;
using System.Collections.Generic;
using Timers;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{

    public void DisableTHis()
    {
        gameObject.SetActive(false);


    }

    // Start is called before the first frame update
    void Start()
    {
        TimersManager.SetTimer(this, 5f, DisableTHis);






    }

    void Update()
    {

        Debug.Log(TimersManager.ElapsedTime(TimersManager.SetTimer(this, 5f, DisableTHis)));
    }
}
