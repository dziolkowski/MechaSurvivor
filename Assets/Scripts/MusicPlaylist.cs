using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MusicPlaylist : MonoBehaviour
{
    [SerializeField] private bool playAndLoop = true;
    [SerializeField] private AudioClip[] musicTracks;

    [Tooltip("Music tracks - 1")]
    [SerializeField] private float[] swapMusic;

    private AudioSource audioSource;
    private int currentClipIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource.isPlaying) {
            audioSource.Stop();
        }

        if (musicTracks.Length > 0) {
            audioSource.clip = musicTracks[currentClipIndex];
            audioSource.Play();
        }
    }
    private void Update() {
        if (playAndLoop) {
            // If playAndLoop is true, check if the current clip has finished playing
            if (!audioSource.isPlaying) {
                ChangeAudioClip();
            }
        }
        else {
            // If playAndLoop is false, check if the current clip has finished playing
            if (!audioSource.isPlaying) {
                ChangeAudioClip();
            }
            else {
                // Check if the current time matches the swapMusic times
                if (currentClipIndex < swapMusic.Length && Time.time >= swapMusic[currentClipIndex] * 60) {
                    ChangeAudioClip();
                }
            }
        }
    }

    private void ChangeAudioClip() {
        currentClipIndex++;

        // Check if the next clip index is within bounds
        if (currentClipIndex < musicTracks.Length) {
            audioSource.clip = musicTracks[currentClipIndex];
            audioSource.Play();
        }
        else { // If the last clip has finished playing
            currentClipIndex = 0; // Reset to the first clip
            audioSource.clip = musicTracks[currentClipIndex];
            audioSource.Play();
        }
    }
}
