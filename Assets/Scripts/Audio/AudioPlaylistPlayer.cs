using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioPlaylistPlayer : MonoBehaviour
{
    [SerializeField] private AudioPlayType playAudioMode;
    //[SerializeField] private bool playOnAwake = false;
    
    // Opcja do wymuszenia innego dzwieku przy kazdym odtworzeniu - nie zaimplementowane
    //[Header("Play Random Settings")]
    //[SerializeField] private bool enforceUniqueClip;
    
    [Header("Play Single Mode Settings")]
    [Tooltip("Set to specified clip number in Play Single Mode.")]
    [SerializeField] private int clipToPlay;
    
    private AudioSource audioSource;
    private AudioPlaylist clipPlaylist;
    
    private AudioClip audioClipToPlay;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        clipPlaylist = GetComponent<AudioPlaylist>();
    }
    
    public void PlayAudio()
    {
        if (clipPlaylist == null)
        {
            Debug.LogError("AudioPlaylist not found on this GameObject.");
            return;
        }
        
        SelectAudioClip();
        //Debug.Log($"Playing audio clip {audioClipToPlay}");
        audioSource.PlayOneShot(audioClipToPlay);
    }

    private void SelectAudioClip()
    {
        switch (playAudioMode)
        {
            case AudioPlayType.Random:
                // if (enforceUniqueClip)
                // {
                //     //int[] listOfClips;
                //     Debug.LogError("Not implemented yet.");
                // }
                // else
                // {
                //     audioClipToPlay = clipPlaylist.clips[Random.Range(0, clipPlaylist.clips.Length)];
                // }
                
                audioClipToPlay = clipPlaylist.clips[Random.Range(0, clipPlaylist.clips.Length)];
                break;

            case AudioPlayType.PlaySingle:
                if (clipToPlay < 0 || clipToPlay >= clipPlaylist.clips.Length)
                {
                    Debug.LogError("Wrong clip number to play, not playing anything");
                    return;
                }

                audioClipToPlay = clipPlaylist.clips[clipToPlay];
                break;

            case AudioPlayType.LoopSingle:
                Debug.LogWarning("Not implemented looping single clip, not playing anything");

                break;

            default:
                Debug.LogWarning("Not playing anything");
                throw new ArgumentOutOfRangeException();
        }
    }
}

enum AudioPlayType
{
    Random,
    PlaySingle,
    LoopSingle
}
