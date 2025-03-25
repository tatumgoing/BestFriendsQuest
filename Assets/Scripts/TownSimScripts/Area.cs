using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area : MonoBehaviour
{
    TownMusicPlayer musicPlayer;

    [SerializeField] Sound associatedTrack;
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
        if (associatedTrack != null)
        {
            musicPlayer.PlayNewTrack(associatedTrack);
        }
        else
        {
            musicPlayer.StopCurrentTrack();
        }
    }
    private void OnDisable()
    {
        if (associatedEnvironment != null) {
            associatedEnvironment.SetActive(false);
        }
    }
}

    
