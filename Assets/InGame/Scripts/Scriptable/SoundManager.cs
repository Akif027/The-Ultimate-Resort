using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SoundManager", fileName = "SoundManager")]
public class SoundManager : ScriptableObject
{
    private static SoundManager instance;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<SoundManager>("SoundManager");
            }
            return instance;
        }
    }

    public AudioClip buttonClick;
    public AudioClip getMoney;
    public AudioClip collectMoney;
    public AudioClip upgrade;
    public AudioClip getObject;
    public AudioClip machineGet;
    public AudioClip PopSound;
    public void PlaySoundButtonClick()
    {
        SoundController.Instance.PlaySFXClip(buttonClick, 1);
    }
    public void PlayPopSound(Vector3 point)
    {
        SoundController.Instance.PlaySfx3D(PopSound, point, 1);
    }


    public void PlayCollectMoneySound(Vector3 point)
    {
        SoundController.Instance.PlaySfx3D(collectMoney, point, 1);
    }

    public void PlayGetMoneySound(Vector3 point)
    {
        SoundController.Instance.PlaySfx3D(getMoney, point, 1);
    }

    public void PlayUpgrade(Vector3 point)
    {
        SoundController.Instance.PlaySfx3D(upgrade, point, 1);
    }

    public void PlayGetObject(Vector3 point)
    {
        SoundController.Instance.PlaySfx3D(getObject, point, 0.5f);
    }

    public void PlayMachineGet(Vector3 point)
    {
        SoundController.Instance.PlaySfx3D(machineGet, point, 1);
    }
}
