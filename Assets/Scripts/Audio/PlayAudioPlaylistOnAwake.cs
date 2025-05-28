using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioPlaylistOnAwake : MonoBehaviour
{
    private AudioPlaylistPlayer audioPlayer;
    private void Awake()
    {
        audioPlayer = GetComponent<AudioPlaylistPlayer>();
        audioPlayer.PlayAudio();
    }
}
