using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    TownMusicPlayer musicPlayer;

    [SerializeField] private bool _dontStop;
    public Sound associatedTrack;
    [SerializeField] GameObject associatedEnvironment;

    private void Start()
    {
        musicPlayer = TownMusicPlayer.i;
    }
    private void OnEnable()
    {
        StartCoroutine(StartMusic());
    }

    private IEnumerator StartMusic()
    {
        yield return new WaitForEndOfFrame();

        if (associatedEnvironment != null)
        {
            associatedEnvironment.SetActive(true);
        }

        if (associatedTrack == null) 
        {
            if (!_dontStop) musicPlayer.StopCurrentTrack();
        }
        else if (musicPlayer.currentTrack == null)
        {
            musicPlayer.PlayNewTrack(associatedTrack);
        }
        else if(musicPlayer.currentTrack.TrackName != associatedTrack.TrackName)
        {
            musicPlayer.PlayNewTrack(associatedTrack);
        }
    }

    private void OnDisable()
    {
        if (associatedEnvironment != null) {
            associatedEnvironment.SetActive(false);
        }
    }
}

    
