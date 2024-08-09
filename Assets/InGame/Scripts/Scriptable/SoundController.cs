using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : Singleton<SoundController>
{

    public static readonly string SFX_KEY = "sfxmode";
    public static readonly string SOUND_KEY = "soundmode";

    public AudioSource music;
    public AudioSource sfx;

    public static bool SfxEnable
    {
        get
        {
            return IPlayerPrefs.GetBool(SFX_KEY, true);
        }
        set
        {
            IPlayerPrefs.SetBool(SFX_KEY, value);

        }
    }

    public static bool SoundEnable
    {
        get
        {
            return IPlayerPrefs.GetBool(SOUND_KEY, true);
        }
        set
        {
            IPlayerPrefs.SetBool(SOUND_KEY, value);

        }
    }

    public void Awake()
    {
        music.mute = !SoundEnable;
        sfx.mute = !SfxEnable;
    }


    private void OnChange(object data)
    {
        music.mute = !SoundEnable;
        sfx.mute = !SfxEnable;
    }

    public void SetMusicVolume(float volume)
    {
        music.volume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        sfx.volume = volume;
    }

    public void PlaySFXClip(AudioClip audio, float vol)
    {
        sfx.PlayOneShot(audio, vol);
    }

    public void PlayMusicClip(AudioClip audio, float vol)
    {
        music.clip = audio;
        music.Play();
    }

    public void PlaySfx3D(AudioClip audio, Vector3 point, float vol = 1)
    {
        GameObject g = Instantiate(sfx.gameObject);
        g.transform.position = point;
        AudioSource audioSource = g.GetComponent<AudioSource>();
        audioSource.clip = audio;
        audioSource.volume = vol;
        audioSource.Play();
        Destroy(g, audio.length);
    }
}
