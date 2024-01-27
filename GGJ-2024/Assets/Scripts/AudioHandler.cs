using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SOUND_EFFECT
{
    STRETCH,
    SQUEAK,
    INFLATE,
    BOP
}

public class AudioHandler : MonoBehaviour
{
    [Header("Audio Player")]
    [SerializeField] public AudioSource player;

    [Header("Sound Effect Audio Clips")]
    [SerializeField] public AudioClip stretchEffect;
    [SerializeField] public AudioClip squeakEffect;
    [SerializeField] public AudioClip inflateEffect;
    [SerializeField] public AudioClip bopEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GetComponent<AudioSource>();
        }

        // Hard code variables to prevent slip ups.
        player.loop = false;
        player.playOnAwake = false;
        player.mute = false;
    }

    /// <summary>
    /// To play a sound effect just invoke method with enum variable for which sound you require.
    /// </summary>
    /// <param name="sound"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void PlaySound(SOUND_EFFECT sound)
    {
        if (!player.isPlaying)
        {
            switch (sound)
            {
                case (SOUND_EFFECT.STRETCH):
                    InitPlayer(stretchEffect);
                    break;
                case SOUND_EFFECT.SQUEAK:
                    InitPlayer(squeakEffect);
                    break;
                case SOUND_EFFECT.INFLATE:
                    InitPlayer(inflateEffect);
                    break;
                case SOUND_EFFECT.BOP:
                    InitPlayer(bopEffect);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(sound), sound, null);
            }  
        }
    }

    private void InitPlayer(AudioClip effect)
    {
        player.clip = effect;
        player.Play();
    }
}
